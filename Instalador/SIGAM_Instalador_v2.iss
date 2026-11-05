; ============================================================================
;  SIGAM - Instalador v2  (Inno Setup 7, instalador de 32 bits)
;  Basado en "script definitivo.iss" (v1). Se mantiene el mismo AppId (IS_64PR3)
;  para que Windows detecte la version 1.0 instalada y la actualice.
;
;  Reparto de tareas:
;   - Inno Setup: copia los archivos, crea accesos directos, verifica .NET 4.7.2
;     y SQL Server, registra el desinstalador y detecta la instalacion previa.
;   - ConfiguradorBDJP86.exe (ejecutado en [Run]): conexion, diagnostico y
;     creacion de la base de datos. Inno le indica el modo con /modo=.
;
;  Compilar (desde esta carpeta, despues de compilar la solucion en Release):
;   "%LOCALAPPDATA%\Programs\Inno Setup 7\ISCC.exe" SIGAM_Instalador_v2.iss
; ============================================================================

#define MyAppName        "SIGAM"
#define MyAppVersion     "2.0"
#define MyAppExeName     "ProyectoIS_64PR.exe"
#define MyConfigExeName  "ConfiguradorBDJP86.exe"
#define MyAppId          "IS_64PR3"
#define BinSistema       "..\ProyectoIS_64PR\bin\Release"
#define BinConfigurador  "..\ConfiguradorBDJP86\bin\Release"

[Setup]
; Mismo AppId que la v1 (que no lo declaraba: Inno usaba el AppName "IS_64PR3").
AppId={#MyAppId}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
VersionInfoVersion=2.0.0.0
VersionInfoProductName={#MyAppName}
VersionInfoDescription=Instalador de {#MyAppName}
UninstallDisplayName={#MyAppName} {#MyAppVersion}
UninstallDisplayIcon={app}\{#MyAppExeName}
SetupIconFile=..\ProyectoIS_64PR\fleetdrive.ico
; Instalacion nueva: Archivos de programa\SIGAM. Actualizacion: se reutiliza la carpeta anterior (UsePreviousAppDir).
DefaultDirName={autopf}\{#MyAppName}
UsePreviousAppDir=yes
DefaultGroupName={#MyAppName}
UsePreviousGroup=no
DisableProgramGroupPage=yes
PrivilegesRequired=admin
; El instalador es de 32 bits (SetupArchitecture por defecto = x86), pero en Windows de 64 bits instala en
; modo 64 bits como la v1: asi se encuentra su registro de desinstalacion y se usa C:\Program Files.
ArchitecturesInstallIn64BitMode=x64compatible
WizardStyle=modern
Compression=lzma2
SolidCompression=yes
OutputDir=Output
OutputBaseFilename=SIGAM_Setup_{#MyAppVersion}
CloseApplications=yes

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[CustomMessages]
spanish.SinDotNet=SIGAM necesita Microsoft .NET Framework 4.7.2 o superior, y no está instalado en este equipo.%n%nDescárguelo e instálelo desde el sitio oficial de Microsoft (busque ".NET Framework 4.7.2 Runtime") y vuelva a ejecutar este instalador.%n%nLa instalación se cancelará.
spanish.SinSqlServer=No se detectó Microsoft SQL Server instalado en este equipo.%n%nSIGAM guarda sus datos en SQL Server (por ejemplo, SQL Server Express). Este instalador no lo incluye: debe instalarse por separado.%n%nSi la base de datos va a estar en otro equipo de la red, puede continuar e indicar ese servidor al configurar la base de datos.%n%n¿Desea continuar con la instalación?
spanish.ReinstalacionDetectada=Se detectó SIGAM versión %1 instalado en este equipo.%nSe actualizarán los archivos del programa. Al finalizar se revisará la base de datos: si está correcta se conserva intacta; si tiene problemas, solo se modificará con su confirmación.
spanish.InstalacionNueva=Instalación nueva. Al finalizar se configurará la conexión con SQL Server y se creará la base de datos.
spanish.PreguntaEliminarBD=¿Desea eliminar también la base de datos de SIGAM?%n%nSi elige No (recomendado), la base de datos se conserva con todos sus datos y podrá volver a usarla si instala SIGAM nuevamente.%n%nSi elige Sí, se le mostrará el servidor y el nombre de la base de datos y deberá confirmarlo escribiendo su nombre.
spanish.ConfigurandoBD=Configurando la base de datos de SIGAM...
spanish.AbrirSigam=Abrir SIGAM
spanish.AccesoEscritorio=Crear un acceso directo en el escritorio
spanish.ConfigurarBD=Configurar base de datos

[Tasks]
Name: "desktopicon"; Description: "{cm:AccesoEscritorio}"; GroupDescription: "{cm:AdditionalIcons}"

[InstallDelete]
; Restos de la v1 que ya no forman parte del sistema
Type: files; Name: "{app}\Servicios_64PR.dll"
Type: files; Name: "{app}\*.pdb"
Type: files; Name: "{app}\Database\BD_64PR.sql"
Type: dirifempty; Name: "{app}\Database"
; Accesos directos de la v1 (se llamaban IS_64PR3)
Type: files; Name: "{autodesktop}\IS_64PR3.lnk"
Type: filesandordirs; Name: "{autoprograms}\IS_64PR3"

[Dirs]
; GestorIdioma registra en los JSON las claves de idioma faltantes: los usuarios necesitan poder modificarlos.
Name: "{app}\idiomas"; Permissions: users-modify

[Files]
; --- Sistema (salida Release de ProyectoIS_64PR) ---
Source: "{#BinSistema}\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BinSistema}\BE.dll";           DestDir: "{app}"; Flags: ignoreversion
Source: "{#BinSistema}\Bitacora.dll";     DestDir: "{app}"; Flags: ignoreversion
Source: "{#BinSistema}\BLL_64PR.dll";     DestDir: "{app}"; Flags: ignoreversion
Source: "{#BinSistema}\DAL_64PR.dll";     DestDir: "{app}"; Flags: ignoreversion
Source: "{#BinSistema}\Documentos.dll";   DestDir: "{app}"; Flags: ignoreversion
Source: "{#BinSistema}\DV.dll";           DestDir: "{app}"; Flags: ignoreversion
Source: "{#BinSistema}\Encriptacion.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BinSistema}\Idioma.dll";       DestDir: "{app}"; Flags: ignoreversion
Source: "{#BinSistema}\Mapper.dll";       DestDir: "{app}"; Flags: ignoreversion
Source: "{#BinSistema}\Respaldos.dll";    DestDir: "{app}"; Flags: ignoreversion
Source: "{#BinSistema}\Sesion.dll";       DestDir: "{app}"; Flags: ignoreversion
Source: "{#BinSistema}\idiomas\es.json";  DestDir: "{app}\idiomas"; Flags: ignoreversion
Source: "{#BinSistema}\idiomas\en.json";  DestDir: "{app}\idiomas"; Flags: ignoreversion
Source: "{#BinSistema}\Ayuda\SIGAM_AyudaEnLinea.pdf"; DestDir: "{app}\Ayuda"; Flags: ignoreversion
; Config del sistema: plantilla sin configurar (#SQLINSTANCE#). Solo se copia si no existe, para conservar
; la conexion de una instalacion anterior (el configurador la propone y la reescribe).
Source: "Plantillas\ProyectoIS_64PR.exe.config"; DestDir: "{app}"; Flags: onlyifdoesntexist
; --- Configurador de la base de datos ---
Source: "{#BinConfigurador}\{#MyConfigExeName}";        DestDir: "{app}"; Flags: ignoreversion
Source: "{#BinConfigurador}\{#MyConfigExeName}.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BinConfigurador}\BaseDatos\SIGAM_Instalacion.sql"; DestDir: "{app}\BaseDatos"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:ConfigurarBD}"; Filename: "{app}\{#MyConfigExeName}"; Parameters: "/version={#MyAppVersion}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
; Configurador de BD: corre elevado (igual que el instalador) y espera a que termine.
Filename: "{app}\{#MyConfigExeName}"; Parameters: "/modo={code:ModoConfigurador} /version={#MyAppVersion}"; StatusMsg: "{cm:ConfigurandoBD}"; Flags: waituntilterminated
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:AbrirSigam}"; Flags: postinstall nowait skipifsilent unchecked

[UninstallDelete]
; Archivos que se generan despues de instalar
Type: filesandordirs; Name: "{app}\Logs"
Type: files; Name: "{app}\*.anterior"

[Code]
const
  ClaveDesinstalacion = 'Software\Microsoft\Windows\CurrentVersion\Uninstall\{#MyAppId}_is1';
  ClaveInstanciasSQL = 'SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL';

var
  EsReinstalacion: Boolean;
  VersionAnterior: String;

{ Busca la instalacion previa (mismo AppId) en las dos vistas del registro }
function LeerVersionInstalada(var Version: String): Boolean;
begin
  Result := False;
  if IsWin64 and RegQueryStringValue(HKLM64, ClaveDesinstalacion, 'DisplayVersion', Version) then
    Result := True
  else if RegQueryStringValue(HKLM32, ClaveDesinstalacion, 'DisplayVersion', Version) then
    Result := True
  else if IsWin64 and RegKeyExists(HKLM64, ClaveDesinstalacion) then
    Result := True
  else if RegKeyExists(HKLM32, ClaveDesinstalacion) then
    Result := True;
  if Result and (Version = '') then
    Version := '?';
end;

function HaySqlServerLocal(): Boolean;
var
  Nombres: TArrayOfString;
begin
  Result := False;
  if IsWin64 and RegGetValueNames(HKLM64, ClaveInstanciasSQL, Nombres) then
    Result := GetArrayLength(Nombres) > 0;
  if (not Result) and RegGetValueNames(HKLM32, ClaveInstanciasSQL, Nombres) then
    Result := GetArrayLength(Nombres) > 0;
end;

function InitializeSetup(): Boolean;
begin
  Result := True;

  { Prerrequisito 1: .NET Framework 4.7.2 (no se empaqueta) }
  if not IsDotNetInstalled(net472, 0) then
  begin
    SuppressibleMsgBox(CustomMessage('SinDotNet'), mbCriticalError, MB_OK, IDOK);
    Result := False;
    Exit;
  end;

  { Prerrequisito 2: SQL Server (no se empaqueta; puede estar en otro equipo) }
  if not HaySqlServerLocal() then
  begin
    if SuppressibleMsgBox(CustomMessage('SinSqlServer'), mbConfirmation, MB_YESNO or MB_DEFBUTTON2, IDYES) <> IDYES then
    begin
      Result := False;
      Exit;
    end;
  end;

  EsReinstalacion := LeerVersionInstalada(VersionAnterior);
  if EsReinstalacion then
    Log('Instalacion previa detectada: version ' + VersionAnterior)
  else
    Log('No hay instalacion previa: instalacion nueva');
end;

{ Parametro /modo= del configurador }
function ModoConfigurador(Param: String): String;
begin
  if EsReinstalacion then
    Result := 'reinstalacion'
  else
    Result := 'instalacion';
end;

{ Aviso del tipo de instalacion en la pagina "Listo para instalar" }
function UpdateReadyMemo(Space, NewLine, MemoUserInfoInfo, MemoDirInfo, MemoTypeInfo,
  MemoComponentsInfo, MemoGroupInfo, MemoTasksInfo: String): String;
begin
  if EsReinstalacion then
    Result := FmtMessage(CustomMessage('ReinstalacionDetectada'), [VersionAnterior])
  else
    Result := CustomMessage('InstalacionNueva');
  Result := Result + NewLine + NewLine + MemoDirInfo;
  if MemoTasksInfo <> '' then
    Result := Result + NewLine + NewLine + MemoTasksInfo;
end;

{ Desinstalacion: preguntar si se elimina tambien la BD (por defecto NO). Si dice que si, el configurador
  muestra servidor y BD y exige escribir el nombre antes del DROP. Corre antes de borrar los archivos. }
procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
var
  Configurador: String;
  Codigo: Integer;
begin
  if CurUninstallStep = usUninstall then
  begin
    Configurador := ExpandConstant('{app}\{#MyConfigExeName}');
    if FileExists(Configurador) and
       (SuppressibleMsgBox(CustomMessage('PreguntaEliminarBD'), mbConfirmation, MB_YESNO or MB_DEFBUTTON2, IDNO) = IDYES) then
    begin
      if Exec(Configurador, '/modo=desinstalacion', ExpandConstant('{app}'), SW_SHOW, ewWaitUntilTerminated, Codigo) then
        Log('Configurador (desinstalacion) termino con codigo ' + IntToStr(Codigo))
      else
        Log('No se pudo ejecutar el configurador: ' + SysErrorMessage(Codigo));
    end
    else
      Log('Se conserva la base de datos.');
  end;
end;
