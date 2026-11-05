<#
  Genera, a partir de la BD de desarrollo (SOLO LECTURA):
    1) ..\BaseDatos\SIGAM_Instalacion.sql  -> script unico de instalacion (estructura + datos minimos)
    2) .\Limpieza_Duplicados_BD_64PR.sql    -> script para dejar la BD de desarrollo con la MISMA estructura
                                               (elimina FK / CHECK / UNIQUE duplicados). NO se ejecuta aca.

  Reglas:
    - El nombre de la BD no va fijo: se usa el token {{NOMBRE_BD}} (lo reemplaza el configurador).
    - Una sola FK por relacion (se prioriza la ON DELETE CASCADE, que es la que espera el codigo),
      un solo CHECK por definicion y una sola UNIQUE por columnas. Si la conservada tenia nombre
      generado por el sistema, se le asigna un nombre explicito.
    - Datos: Patente_64PR, Modulo_64PR y TipoEvento_64PR completos; Roles_64PR solo Basico (ID 1,
      requerido por EliminarRol) y el rol del admin; RolPatente_64PR solo las del rol del admin;
      USUARIO_64PR solo 'admin' (PrimeraVez = 1); VersionBDJP86 con la version indicada.
    - DV_64PR / DVH_64PR quedan vacias: el configurador recalcula el DV al terminar.

  Uso:  powershell -ExecutionPolicy Bypass -File GenerarScriptInstalacion.ps1 [-Servidor .] [-BaseOrigen BD_64PR] [-Version 2.0]
#>
param(
    [string]$Servidor = '.',
    [string]$BaseOrigen = 'BD_64PR',
    [string]$Version = '2.0',
    [string]$LoginAdmin = 'admin',
    [int]$IdRolBasico = 1
)

$ErrorActionPreference = 'Stop'
$token = '{{NOMBRE_BD}}'
$marcaConexion = '-- @@SIGAM:CONECTAR_BD@@'
$aqui = $PSScriptRoot
$salidaInstalacion = [IO.Path]::GetFullPath((Join-Path $aqui '..\BaseDatos\SIGAM_Instalacion.sql'))
$salidaLimpieza = Join-Path $aqui 'Limpieza_Duplicados_BD_64PR.sql'
$inv = [Globalization.CultureInfo]::InvariantCulture

$dirActual = Get-Location
Import-Module 'C:\Program Files\Microsoft SQL Server\170\Tools\PowerShell\Modules\SQLPS\SQLPS.psd1' -DisableNameChecking 3>$null
Set-Location $dirActual

# ---------------------------------------------------------------- utilidades
$cn = New-Object System.Data.SqlClient.SqlConnection "Data Source=$Servidor;Initial Catalog=$BaseOrigen;Integrated Security=True;TrustServerCertificate=True;Application Name=GeneradorScriptSIGAM"
$cn.Open()
function Consultar([string]$sql) {
    $da = New-Object System.Data.SqlClient.SqlDataAdapter($sql, $cn)
    $dt = New-Object System.Data.DataTable
    [void]$da.Fill($dt)
    return , $dt
}
function Escalar([string]$sql) {
    $cmd = $cn.CreateCommand(); $cmd.CommandText = $sql
    return $cmd.ExecuteScalar()
}
function Literal($v) {
    if ($v -is [DBNull] -or $null -eq $v) { return 'NULL' }
    if ($v -is [byte[]]) { return '0x' + (($v | ForEach-Object { $_.ToString('X2') }) -join '') }
    if ($v -is [bool]) { if ($v) { return '1' } else { return '0' } }
    if ($v -is [string]) { return "N'" + $v.Replace("'", "''") + "'" }
    if ($v -is [datetime]) { return "'" + $v.ToString('yyyy-MM-ddTHH:mm:ss.fff', $inv) + "'" }
    if ($v -is [timespan]) { return "'" + $v.ToString() + "'" }
    if ($v -is [guid]) { return "'" + $v.ToString() + "'" }
    return ([IFormattable]$v).ToString($null, $inv)
}
function Limpio([string]$s) { return ($s -replace '[\[\]]', '') -replace ',', '_' }

$sb = New-Object System.Text.StringBuilder
function Lote([string]$texto) { [void]$sb.AppendLine($texto.TrimEnd()); [void]$sb.AppendLine('GO') }
function Comentario([string]$texto) { [void]$sb.AppendLine(''); [void]$sb.AppendLine('-- ' + ('=' * 70)); [void]$sb.AppendLine('-- ' + $texto); [void]$sb.AppendLine('-- ' + ('=' * 70)) }

$colacion = [string](Escalar "SELECT CONVERT(sysname, DATABASEPROPERTYEX(DB_NAME(), 'Collation'))")
$idRolAdmin = Escalar "SELECT Rol FROM dbo.USUARIO_64PR WHERE Login = N'$LoginAdmin'"
if ($null -eq $idRolAdmin) { throw "No existe el usuario '$LoginAdmin' en $BaseOrigen." }
$famAdmin = Escalar "SELECT COUNT(*) FROM dbo.RolFamilia_64PR WHERE ID_Rol = $idRolAdmin"
if ($famAdmin -gt 0) { Write-Warning "El rol del admin tiene $famAdmin familias asignadas: NO se copian (las familias quedan vacias)." }

# ---------------------------------------------------------------- cabecera + creacion de la BD
[void]$sb.AppendLine("/*")
[void]$sb.AppendLine("   SIGAM - Script de instalacion de la base de datos (version $Version)")
[void]$sb.AppendLine("   Generado: $((Get-Date).ToString('yyyy-MM-dd HH:mm', $inv))")
[void]$sb.AppendLine("")
[void]$sb.AppendLine("   - $token se reemplaza por el nombre de la base de datos antes de ejecutar.")
[void]$sb.AppendLine("   - Los lotes previos a la marca '$marcaConexion' se ejecutan contra master;")
[void]$sb.AppendLine("     el resto, contra la base de datos creada.")
[void]$sb.AppendLine("   - El digito verificador (DV_64PR / DVH_64PR) se calcula al finalizar la instalacion.")
[void]$sb.AppendLine("*/")
Lote "USE [master]"
Lote "IF DB_ID(N'$token') IS NOT NULL`r`n    RAISERROR(N'La base de datos $token ya existe. Debe eliminarse antes de ejecutar este script.', 16, 1);"
Lote "CREATE DATABASE [$token] COLLATE $colacion;"
[void]$sb.AppendLine($marcaConexion)
Lote "USE [$token]"

# ---------------------------------------------------------------- estructura (SMO)
$srv = New-Object Microsoft.SqlServer.Management.Smo.Server $Servidor
$db = $srv.Databases[$BaseOrigen]
$db.Tables.Refresh(); $db.StoredProcedures.Refresh()

## SMO solo genera las columnas (incluye IDENTITY). Todas las restricciones e indices se arman abajo
## desde el catalogo, con nombre explicito y sin opciones de una version particular de SQL Server.
$opTabla = New-Object Microsoft.SqlServer.Management.Smo.ScriptingOptions
$opTabla.DriAll = $false; $opTabla.DriPrimaryKey = $false; $opTabla.DriDefaults = $false
$opTabla.DriUniqueKeys = $false; $opTabla.DriChecks = $false; $opTabla.DriForeignKeys = $false
$opTabla.Indexes = $false; $opTabla.ClusteredIndexes = $false; $opTabla.NonClusteredIndexes = $false
$opTabla.Triggers = $false; $opTabla.NoCollation = $true; $opTabla.NoFileGroup = $true
$opTabla.SchemaQualify = $true; $opTabla.IncludeHeaders = $false

$tablas = @($db.Tables | Where-Object { -not $_.IsSystemObject } | Sort-Object Name)
Comentario "TABLAS ($($tablas.Count))"
foreach ($t in $tablas) { foreach ($s in $t.Script($opTabla)) { Lote $s } }

$limp = New-Object System.Collections.Generic.List[string]   # sentencias del script de limpieza
$resumen = New-Object System.Collections.Generic.List[string]
$renombres = New-Object System.Collections.Generic.List[string]

function Elegir($grupo, [scriptblock]$prioridad) {
    return @($grupo.Group | Sort-Object -Property @{ Expression = $prioridad }, @{ Expression = { [int]$_.sysn } }, @{ Expression = { [int]$_.oid } })[0]
}
function RegistrarLimpieza($grupo, $elegida, [string]$nombreFinal) {
    foreach ($o in $grupo.Group) {
        if ($o.oid -ne $elegida.oid) { $limp.Add("ALTER TABLE [dbo].[$($o.tabla)] DROP CONSTRAINT [$($o.nombre)];") }
    }
    if ($elegida.nombre -ne $nombreFinal) {
        $limp.Add("EXEC sp_rename N'dbo.[$($elegida.nombre)]', N'$nombreFinal', N'OBJECT';")
        $renombres.Add("$($elegida.nombre) -> $nombreFinal")
    }
    if ($grupo.Count -gt 1) {
        $linea = "{0,-26} {1,-40} duplicados: {2,2} -> se conserva {3}" -f @($grupo.Group[0].tabla, $grupo.Name.Split('|')[1], ($grupo.Count - 1), $nombreFinal)
        $resumen.Add($linea)
    }
}

# PRIMARY KEY (una por tabla; las de nombre generado por el sistema pasan a PK_<tabla>)
$pk = Consultar @"
SELECT kc.object_id oid, OBJECT_NAME(kc.parent_object_id) tabla, kc.name nombre, kc.is_system_named sysn, i.type_desc tipo,
  STUFF((SELECT ',' + QUOTENAME(COL_NAME(ic.object_id, ic.column_id)) FROM sys.index_columns ic
         WHERE ic.object_id = kc.parent_object_id AND ic.index_id = kc.unique_index_id ORDER BY ic.key_ordinal FOR XML PATH('')), 1, 1, '') cols
FROM sys.key_constraints kc JOIN sys.indexes i ON i.object_id = kc.parent_object_id AND i.index_id = kc.unique_index_id
WHERE kc.type = 'PK'
"@
$sentPK = New-Object System.Collections.Generic.List[string]
foreach ($g in ($pk.Rows | Group-Object { "$($_.tabla)|PK" } | Sort-Object Name)) {
    $e = Elegir $g { 0 }
    $nombre = if ($e.sysn) { "PK_$($e.tabla)" } else { $e.nombre }
    $sentPK.Add("ALTER TABLE [dbo].[$($e.tabla)] ADD CONSTRAINT [$nombre] PRIMARY KEY $($e.tipo) ($($e.cols));")
    RegistrarLimpieza $g $e $nombre
}
Comentario "CLAVES PRIMARIAS ($($sentPK.Count))"
Lote ($sentPK -join "`r`n")

# DEFAULT (las de nombre generado por el sistema pasan a DF_<tabla>_<columna>)
$df = Consultar @"
SELECT dc.object_id oid, OBJECT_NAME(dc.parent_object_id) tabla, dc.name nombre, dc.is_system_named sysn, dc.definition def,
  COL_NAME(dc.parent_object_id, dc.parent_column_id) col
FROM sys.default_constraints dc
"@
$sentDF = New-Object System.Collections.Generic.List[string]
foreach ($g in ($df.Rows | Group-Object { "$($_.tabla)|$($_.col)" } | Sort-Object Name)) {
    $e = Elegir $g { 0 }
    $nombre = if ($e.sysn) { "DF_$($e.tabla)_$($e.col)" } else { $e.nombre }
    $sentDF.Add("ALTER TABLE [dbo].[$($e.tabla)] ADD CONSTRAINT [$nombre] DEFAULT $($e.def) FOR [$($e.col)];")
    RegistrarLimpieza $g $e $nombre
}
Comentario "VALORES POR DEFECTO ($($sentDF.Count))"
Lote ($sentDF -join "`r`n")

# ---------------------------------------------------------------- datos minimos
function InsertarDatos([string]$tabla, [string]$where, [hashtable]$forzar, [string]$orden) {
    $cols = Consultar "SELECT c.name, c.is_identity FROM sys.columns c WHERE c.object_id = OBJECT_ID(N'dbo.$tabla') AND c.is_computed = 0 ORDER BY c.column_id"
    $nombres = @($cols | ForEach-Object { $_.name })
    $tieneIdentity = @($cols | Where-Object { $_.is_identity }).Count -gt 0
    $filas = Consultar ("SELECT " + (($nombres | ForEach-Object { "[$_]" }) -join ', ') + " FROM dbo.[$tabla]" + $(if ($where) { " WHERE $where" } else { '' }) + " ORDER BY $orden")
    $lista = ($nombres | ForEach-Object { "[$_]" }) -join ', '
    $lineas = New-Object System.Collections.Generic.List[string]
    if ($tieneIdentity) { $lineas.Add("SET IDENTITY_INSERT [dbo].[$tabla] ON;") }
    foreach ($f in $filas.Rows) {
        $valores = foreach ($n in $nombres) { if ($forzar -and $forzar.ContainsKey($n)) { $forzar[$n] } else { Literal $f[$n] } }
        $lineas.Add("INSERT INTO [dbo].[$tabla] ($lista) VALUES (" + ($valores -join ', ') + ");")
    }
    if ($tieneIdentity) { $lineas.Add("SET IDENTITY_INSERT [dbo].[$tabla] OFF;") }
    Comentario "DATOS: $tabla ($($filas.Rows.Count) filas)"
    Lote ($lineas -join "`r`n")
    return $filas.Rows.Count
}
$conteo = [ordered]@{}
$conteo['Modulo_64PR'] = InsertarDatos 'Modulo_64PR' $null $null 'Id_modulo'
$conteo['TipoEvento_64PR'] = InsertarDatos 'TipoEvento_64PR' $null $null 'Id_tipo'
$conteo['Patente_64PR'] = InsertarDatos 'Patente_64PR' $null $null 'ID_Patente'
$conteo['Roles_64PR'] = InsertarDatos 'Roles_64PR' "ID_Rol IN ($IdRolBasico, $idRolAdmin)" $null 'ID_Rol'
$conteo['RolPatente_64PR'] = InsertarDatos 'RolPatente_64PR' "ID_Rol = $idRolAdmin" $null 'ID_Rol, ID_Patente'
$conteo['USUARIO_64PR'] = InsertarDatos 'USUARIO_64PR' "Login = N'$LoginAdmin'" @{ PrimeraVez = '1'; Intentos = '0'; Bloqueo = '0' } 'DNI'
Comentario "DATOS: VersionBDJP86 (1 fila)"
Lote "INSERT INTO [dbo].[VersionBDJP86] ([Version], [Descripcion]) VALUES ('$Version', N'Instalacion SIGAM $Version');"
$conteo['VersionBDJP86'] = 1

# ---------------------------------------------------------------- restricciones deduplicadas
# UNIQUE
$uq = Consultar @"
SELECT kc.object_id oid, OBJECT_NAME(kc.parent_object_id) tabla, kc.name nombre, kc.is_system_named sysn, i.type_desc tipo,
  STUFF((SELECT ',' + QUOTENAME(COL_NAME(ic.object_id, ic.column_id)) FROM sys.index_columns ic
         WHERE ic.object_id = kc.parent_object_id AND ic.index_id = kc.unique_index_id ORDER BY ic.key_ordinal FOR XML PATH('')), 1, 1, '') cols
FROM sys.key_constraints kc JOIN sys.indexes i ON i.object_id = kc.parent_object_id AND i.index_id = kc.unique_index_id
WHERE kc.type = 'UQ'
"@
$sentUQ = New-Object System.Collections.Generic.List[string]
foreach ($g in ($uq.Rows | Group-Object { "$($_.tabla)|$($_.cols)" } | Sort-Object Name)) {
    $e = Elegir $g { 0 }
    $nombre = if ($e.sysn) { "UQ_$($e.tabla)_$(Limpio $e.cols)" } else { $e.nombre }
    $sentUQ.Add("ALTER TABLE [dbo].[$($e.tabla)] ADD CONSTRAINT [$nombre] UNIQUE $($e.tipo) ($($e.cols));")
    RegistrarLimpieza $g $e $nombre
}
Comentario "RESTRICCIONES UNIQUE ($($sentUQ.Count))"
Lote ($sentUQ -join "`r`n")

# CHECK
$ck = Consultar @"
SELECT cc.object_id oid, OBJECT_NAME(cc.parent_object_id) tabla, cc.name nombre, cc.is_system_named sysn, cc.definition def,
  ISNULL(COL_NAME(cc.parent_object_id, NULLIF(cc.parent_column_id, 0)), 'Tabla') col
FROM sys.check_constraints cc
"@
$sentCK = New-Object System.Collections.Generic.List[string]
foreach ($g in ($ck.Rows | Group-Object { "$($_.tabla)|$($_.def)" } | Sort-Object Name)) {
    $e = Elegir $g { 0 }
    $nombre = if ($e.sysn) { "CK_$($e.tabla)_$($e.col)" } else { $e.nombre }
    $sentCK.Add("ALTER TABLE [dbo].[$($e.tabla)] WITH CHECK ADD CONSTRAINT [$nombre] CHECK $($e.def);")
    RegistrarLimpieza $g $e $nombre
}
Comentario "RESTRICCIONES CHECK ($($sentCK.Count))"
Lote ($sentCK -join "`r`n")

# FOREIGN KEY
$fk = Consultar @"
SELECT f.object_id oid, OBJECT_NAME(f.parent_object_id) tabla, f.name nombre, f.is_system_named sysn,
  OBJECT_NAME(f.referenced_object_id) padre, f.delete_referential_action borrar, f.update_referential_action actualizar,
  STUFF((SELECT ',' + QUOTENAME(COL_NAME(fc.parent_object_id, fc.parent_column_id)) FROM sys.foreign_key_columns fc
         WHERE fc.constraint_object_id = f.object_id ORDER BY fc.constraint_column_id FOR XML PATH('')), 1, 1, '') cols,
  STUFF((SELECT ',' + QUOTENAME(COL_NAME(fc.referenced_object_id, fc.referenced_column_id)) FROM sys.foreign_key_columns fc
         WHERE fc.constraint_object_id = f.object_id ORDER BY fc.constraint_column_id FOR XML PATH('')), 1, 1, '') refcols
FROM sys.foreign_keys f
"@
$accion = @{ 0 = ''; 1 = 'CASCADE'; 2 = 'SET NULL'; 3 = 'SET DEFAULT' }
$sentFK = New-Object System.Collections.Generic.List[string]
foreach ($g in ($fk.Rows | Group-Object { "$($_.tabla)|$($_.cols) -> $($_.padre)$($_.refcols)" } | Sort-Object Name)) {
    $e = Elegir $g { if ([int]$_.borrar -eq 1) { 0 } else { 1 } }   # primero la ON DELETE CASCADE
    $nombre = if ($e.sysn) { "FK_$($e.tabla)_$(Limpio $e.cols)" } else { $e.nombre }
    $s = "ALTER TABLE [dbo].[$($e.tabla)] WITH CHECK ADD CONSTRAINT [$nombre] FOREIGN KEY ($($e.cols)) REFERENCES [dbo].[$($e.padre)] ($($e.refcols))"
    if ($accion[[int]$e.borrar]) { $s += " ON DELETE $($accion[[int]$e.borrar])" }
    if ($accion[[int]$e.actualizar]) { $s += " ON UPDATE $($accion[[int]$e.actualizar])" }
    $sentFK.Add($s + ';')
    RegistrarLimpieza $g $e $nombre
}
Comentario "CLAVES FORANEAS ($($sentFK.Count))"
Lote ($sentFK -join "`r`n")

# INDICES que no son restricciones (ej: indice unico filtrado de Vehiculo_BitacoraCambios)
$ix = Consultar @"
SELECT OBJECT_NAME(i.object_id) tabla, i.name nombre, i.is_unique unico, i.type_desc tipo, i.filter_definition filtro,
  STUFF((SELECT ',' + QUOTENAME(COL_NAME(ic.object_id, ic.column_id)) + CASE WHEN ic.is_descending_key = 1 THEN ' DESC' ELSE '' END
         FROM sys.index_columns ic WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND ic.is_included_column = 0
         ORDER BY ic.key_ordinal FOR XML PATH('')), 1, 1, '') cols,
  STUFF((SELECT ',' + QUOTENAME(COL_NAME(ic.object_id, ic.column_id))
         FROM sys.index_columns ic WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND ic.is_included_column = 1
         FOR XML PATH('')), 1, 1, '') incluidas
FROM sys.indexes i JOIN sys.tables t ON t.object_id = i.object_id
WHERE i.type > 0 AND i.is_primary_key = 0 AND i.is_unique_constraint = 0 AND i.is_hypothetical = 0
ORDER BY 1, 2
"@
$sentIX = New-Object System.Collections.Generic.List[string]
foreach ($r in $ix.Rows) {
    $s = "CREATE $(if ($r.unico) { 'UNIQUE ' } else { '' })$($r.tipo) INDEX [$($r.nombre)] ON [dbo].[$($r.tabla)] ($($r.cols))"
    if (-not ($r.incluidas -is [DBNull])) { $s += " INCLUDE ($($r.incluidas))" }
    if (-not ($r.filtro -is [DBNull])) { $s += " WHERE $($r.filtro)" }
    $sentIX.Add($s + ';')
}
if ($sentIX.Count -gt 0) {
    Comentario "INDICES ($($sentIX.Count))"
    Lote ($sentIX -join "`r`n")
}

# ---------------------------------------------------------------- SP y triggers (SMO)
$opModulo = New-Object Microsoft.SqlServer.Management.Smo.ScriptingOptions
$opModulo.SchemaQualify = $true; $opModulo.IncludeHeaders = $false
$sps = @($db.StoredProcedures | Where-Object { -not $_.IsSystemObject } | Sort-Object Name)
Comentario "PROCEDIMIENTOS ALMACENADOS ($($sps.Count))"
foreach ($p in $sps) { foreach ($s in $p.Script($opModulo)) { Lote $s } }
$trs = @(foreach ($t in $tablas) { $t.Triggers | Where-Object { -not $_.IsSystemObject } })
Comentario "TRIGGERS ($($trs.Count))"
foreach ($tr in $trs) { foreach ($s in $tr.Script($opModulo)) { Lote $s } }

$texto = $sb.ToString()
if ($texto -match [regex]::Escape($BaseOrigen)) { Write-Warning "El script contiene el nombre '$BaseOrigen' literal: revisar." }
[IO.Directory]::CreateDirectory((Split-Path $salidaInstalacion)) | Out-Null
[IO.File]::WriteAllText($salidaInstalacion, $texto, (New-Object System.Text.UTF8Encoding($true)))

# ---------------------------------------------------------------- script de limpieza (BD de desarrollo)
$l = New-Object System.Text.StringBuilder
[void]$l.AppendLine("/*")
[void]$l.AppendLine("   SIGAM - Limpieza de restricciones duplicadas en la BD de desarrollo ($BaseOrigen)")
[void]$l.AppendLine("   Generado: $((Get-Date).ToString('yyyy-MM-dd HH:mm', $inv)) junto con SIGAM_Instalacion.sql")
[void]$l.AppendLine("   Deja exactamente las mismas UNIQUE / CHECK / FK que el script de instalacion.")
[void]$l.AppendLine("   Todo corre en una transaccion: si algo falla, no se aplica nada.")
[void]$l.AppendLine("   Recomendado: hacer un backup de $BaseOrigen antes de ejecutarlo.")
[void]$l.AppendLine("")
[void]$l.AppendLine("   Duplicados eliminados:")
foreach ($r in $resumen) { [void]$l.AppendLine("     $r") }
[void]$l.AppendLine("")
[void]$l.AppendLine("   Restricciones con nombre generado por el sistema que pasan a tener nombre explicito")
[void]$l.AppendLine("   (el mismo que usa el script de instalacion; el codigo no referencia estos nombres):")
foreach ($r in $renombres) { [void]$l.AppendLine("     $r") }
[void]$l.AppendLine("*/")
[void]$l.AppendLine("USE [$BaseOrigen];")
[void]$l.AppendLine("GO")
[void]$l.AppendLine("SET XACT_ABORT ON;")
[void]$l.AppendLine("BEGIN TRY")
[void]$l.AppendLine("    BEGIN TRANSACTION;")
foreach ($s in $limp) { [void]$l.AppendLine("    $s") }
[void]$l.AppendLine("    COMMIT TRANSACTION;")
[void]$l.AppendLine("    PRINT 'Limpieza aplicada correctamente.';")
[void]$l.AppendLine("END TRY")
[void]$l.AppendLine("BEGIN CATCH")
[void]$l.AppendLine("    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;")
[void]$l.AppendLine("    THROW;")
[void]$l.AppendLine("END CATCH;")
[void]$l.AppendLine("GO")
[void]$l.AppendLine("-- Verificacion (esperado: $($sentPK.Count) PK, $($sentDF.Count) DEFAULT, $($sentUQ.Count) UNIQUE, $($sentCK.Count) CHECK, $($sentFK.Count) FK, 0 con nombre de sistema)")
[void]$l.AppendLine("SELECT (SELECT COUNT(*) FROM sys.key_constraints WHERE type = 'PK') AS PrimaryKeys,")
[void]$l.AppendLine("       (SELECT COUNT(*) FROM sys.default_constraints) AS Defaults,")
[void]$l.AppendLine("       (SELECT COUNT(*) FROM sys.key_constraints WHERE type = 'UQ') AS UniqueKeys,")
[void]$l.AppendLine("       (SELECT COUNT(*) FROM sys.check_constraints) AS Checks,")
[void]$l.AppendLine("       (SELECT COUNT(*) FROM sys.foreign_keys) AS ForeignKeys,")
[void]$l.AppendLine("       (SELECT COUNT(*) FROM sys.objects WHERE type IN ('PK','UQ','C','F','D') AND OBJECTPROPERTY(object_id, 'IsSystemNamed') = 1) AS ConNombreDeSistema;")
[void]$l.AppendLine("GO")
[IO.File]::WriteAllText($salidaLimpieza, $l.ToString(), (New-Object System.Text.UTF8Encoding($true)))
$cn.Close()

# ---------------------------------------------------------------- informe por consola
"Script de instalacion: $salidaInstalacion"
"Script de limpieza:    $salidaLimpieza"
"Tablas: $($tablas.Count) | SP: $($sps.Count) | Triggers: $($trs.Count) | PK: $($sentPK.Count) | DEFAULT: $($sentDF.Count) | UNIQUE: $($sentUQ.Count) | CHECK: $($sentCK.Count) | FK: $($sentFK.Count) | Indices: $($sentIX.Count)"
"Renombres: $($renombres.Count)"
"Sentencias de limpieza: $($limp.Count)"
"Filas de datos:"; $conteo.GetEnumerator() | ForEach-Object { "  {0,-18} {1}" -f $_.Key, $_.Value }
"Resumen de deduplicacion:"; $resumen | ForEach-Object { "  $_" }
