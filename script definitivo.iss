; ============================================
; Instalador IS_64PR3
; ============================================

#define MyAppName "IS_64PR3"
#define MyAppVersion "1.0"
#define MyAppExeName "ProyectoIS_64PR.exe"
#define MySourceDir "C:\Users\Alejo\source\repos\IS_64PR3\ProyectoIS_64PR\bin\Release"
#define MySqlScript "C:\Users\Alejo\source\repos\IS_64PR3\BD_64PR.sql"

[Setup]
AppName={#MyAppName}
AppVersion={#MyAppVersion}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
OutputDir=C:\Users\Alejo\Desktop\Instalador
OutputBaseFilename=Instalador_IS_64PR3
Compression=lzma
SolidCompression=yes
ArchitecturesInstallIn64BitMode=x64compatible
DisableProgramGroupPage=yes

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Files]
Source: "{#MySourceDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "{#MySqlScript}"; DestDir: "{app}\Database"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"

[Dirs]
Name: "C:\Backups_64PR"

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Ejecutar {#MyAppName}"; Flags: postinstall nowait skipifsilent unchecked

[Code]
var
  InstanciaPage: TInputQueryWizardPage;

function DetectarInstanciaSQL(): String;
var
  Nombres: TArrayOfString;
  i: Integer;
begin
  Result := '.\SQLEXPRESS'; // valor por defecto si no detecta nada

  if RegGetValueNames(HKLM, 'SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL', Nombres) then
  begin
    if GetArrayLength(Nombres) > 0 then
    begin
      for i := 0 to GetArrayLength(Nombres) - 1 do
      begin
        if Nombres[i] = 'MSSQLSERVER' then
          Result := '.'  // instancia default, sin nombre
        else
          Result := '.\' + Nombres[i]; // instancia con nombre, ej .\SQLEXPRESS
      end;
    end;
  end;
end;

procedure InitializeWizard();
begin
  InstanciaPage := CreateInputQueryPage(wpReady,
    'Configuración de Base de Datos',
    'Confirmá el nombre de la instancia de SQL Server',
    'Se detectó la siguiente instancia. Si no es correcta, corregila (podés verificarla en services.msc, mirando el nombre entre paréntesis del servicio "SQL Server"):');
  InstanciaPage.Add('Instancia de SQL Server:', False);
  InstanciaPage.Values[0] := DetectarInstanciaSQL();
end;

procedure CurStepChanged(CurStep: TSetupStep);
var
  RutaConfig: String;
  Contenido: AnsiString;
  ContenidoStr: String;
  InstanciaFinal: String;
  ResultCode: Integer;
  ComandoSQL: String;
begin
  if CurStep = ssPostInstall then
  begin
    InstanciaFinal := InstanciaPage.Values[0];
    RutaConfig := ExpandConstant('{app}\ProyectoIS_64PR.exe.config');

    ///Reemplaza el placeholder #SQLINSTANCE# en el .exe.config
    if LoadStringFromFile(RutaConfig, Contenido) then
    begin
      ContenidoStr := String(Contenido);
      StringChangeEx(ContenidoStr, '#SQLINSTANCE#', InstanciaFinal, True);
      Contenido := AnsiString(ContenidoStr);
      SaveStringToFile(RutaConfig, Contenido, False);
    end;

    ///Intenta crear la base de datos contra la instancia confirmada
    ComandoSQL := '-S ' + InstanciaFinal + ' -i "' + ExpandConstant('{app}\Database\BD_64PR.sql') + '"';

    if not Exec('sqlcmd.exe', ComandoSQL, '', SW_HIDE, ewWaitUntilTerminated, ResultCode) then
    begin
      MsgBox('No se pudo crear la base de datos automáticamente (sqlcmd no está disponible en esta PC).' + #13#10 +
             'Abrí SQL Server Management Studio, cargá el archivo:' + #13#10 +
             ExpandConstant('{app}\Database\BD_64PR.sql') + #13#10 +
             'y ejecutalo manualmente antes de iniciar la aplicación.',
             mbInformation, MB_OK);
    end
    else if ResultCode <> 0 then
    begin
      MsgBox('sqlcmd se ejecutó pero devolvió un error (código ' + IntToStr(ResultCode) + ').' + #13#10 +
             'Puede que la base ya exista, o que haya un problema de permisos.' + #13#10 +
             'Si el login falla, revisá manualmente en SSMS.',
             mbInformation, MB_OK);
    end;
  end;
end;