unit TSLPatcher;

{*
 * TSLPatcher - Reverse Engineered from TSLPatcher.exe
 * Game Data Patcher for KotOR/TSL
 * Supports: 2DA/TLK/GFF/NSS/SSF/ERF/RIM file patching
 * 
 * Original: TSLPatcher.exe (Delphi application)
 * Reverse Engineering Status: In Progress
 *}

interface

uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
  StdCtrls, ComCtrls, ExtCtrls, Menus, FileCtrl, ShellAPI, Registry,
  IniFiles;

type
  // Log levels: 0=None, 1=Errors, 2=Errors+Warnings, 3=Standard, 4=Debug
  TLogLevel = (llNone = 0, llErrors = 1, llErrorsWarnings = 2, llStandard = 3, llDebug = 4);

  // Patch operation types
  TPatchOperationType = (potTwoDA, potTLK, potGFF, potNSS, potSSF, potERF, potRIM, potInstall);

  // Main form class
  TMainForm = class(TForm)
    // UI Components - TODO: Complete UI component definitions
    // Based on string analysis, the form contains:
    // - Game folder selection
    // - Configuration summary
    // - Progress log (RichEdit)
    // - Settings dialog
    // - Install/Start patching button
  private
    FTSLPatchDataPath: string;
    FGamePath: string;
    FConfigFile: string;
    FInfoFile: string;
    FLogLevel: TLogLevel;
    FMakeBackups: Boolean;
    FErrorCount: Integer;
    FWarningCount: Integer;
    FProgressLog: TStrings;
    
    procedure LoadConfiguration;
    procedure LoadInstructions;
    procedure ValidateGamePath;
    procedure StartPatching;
    procedure ProcessPatchOperations;
    procedure LogMessage(const AMessage: string; ALogLevel: TLogLevel);
    procedure LogError(const AMessage: string);
    procedure LogWarning(const AMessage: string);
    procedure LogInfo(const AMessage: string);
    procedure LogDebug(const AMessage: string);
  public
    constructor Create(AOwner: TComponent); override;
    destructor Destroy; override;
    procedure Initialize;
  end;

  // Configuration manager
  TTSLPatcherConfig = class
  private
    FIniFile: TIniFile;
    FGamePath: string;
    FMakeBackups: Boolean;
    FLogLevel: TLogLevel;
    FInstructionsText: string;
  public
    constructor Create(const AConfigFile: string);
    destructor Destroy; override;
    procedure LoadFromFile(const AFileName: string);
    property GamePath: string read FGamePath write FGamePath;
    property MakeBackups: Boolean read FMakeBackups write FMakeBackups;
    property LogLevel: TLogLevel read FLogLevel write FLogLevel;
    property InstructionsText: string read FInstructionsText write FInstructionsText;
  end;

  // File format patchers
  TTwoDAPatcher = class
  public
    // TODO: STUB - Implement 2DA file patching (TSLPatcher.exe: reverse engineering in progress)
    procedure PatchFile(const AFileName: string; const AModifications: TStrings);
  end;

  TTLKPatcher = class
  public
    // TODO: STUB - Implement TLK file patching (TSLPatcher.exe: reverse engineering in progress)
    procedure AppendDialog(const AFileName: string; const AEntries: TStrings);
    procedure ModifyEntries(const AFileName: string; const AModifications: TStrings);
  end;

  TGFFPatcher = class
  public
    // TODO: STUB - Implement GFF file patching (TSLPatcher.exe: reverse engineering in progress)
    procedure PatchFile(const AFileName: string; const AFieldPath: string; const AValue: Variant);
  end;

  TNSSPatcher = class
  public
    // TODO: STUB - Implement NSS file patching and compilation (TSLPatcher.exe: reverse engineering in progress)
    procedure CompileScript(const ASourceFile: string; const AOutputFile: string);
    procedure PatchNCS(const AFileName: string; const AIntegerHacks: TStrings);
  end;

  TSSFPatcher = class
  public
    // TODO: STUB - Implement SSF file patching (TSLPatcher.exe: reverse engineering in progress)
    procedure PatchFile(const AFileName: string; const AModifications: TStrings);
  end;

  TERFPatcher = class
  public
    // TODO: STUB - Implement ERF file patching (TSLPatcher.exe: reverse engineering in progress)
    procedure PatchFile(const AFileName: string; const AModifications: TStrings);
  end;

  TRIMPatcher = class
  public
    // TODO: STUB - Implement RIM file patching (TSLPatcher.exe: reverse engineering in progress)
    procedure PatchFile(const AFileName: string; const AModifications: TStrings);
  end;

  // Backup manager
  TBackupManager = class
  public
    function CreateBackup(const AFileName: string): string;
    procedure RestoreBackup(const ABackupFile: string; const ATargetFile: string);
  end;

var
  MainForm: TMainForm;

implementation

uses
  StrUtils, TwoDAPatcher;

{ TMainForm }

constructor TMainForm.Create(AOwner: TComponent);
begin
  inherited Create(AOwner);
  FProgressLog := TStringList.Create;
  FErrorCount := 0;
  FWarningCount := 0;
  FLogLevel := llStandard;
  FMakeBackups := True;
end;

destructor TMainForm.Destroy;
begin
  FProgressLog.Free;
  inherited Destroy;
end;

procedure TMainForm.Initialize;
begin
  // Initialize the patcher
  // Get executable path
  FTSLPatchDataPath := ExtractFilePath(Application.ExeName) + 'tslpatchdata\';
  
  // Load configuration
  LoadConfiguration;
  
  // Load instructions
  LoadInstructions;
end;

procedure TMainForm.LoadConfiguration;
var
  ConfigFile: string;
  Config: TTSLPatcherConfig;
begin
  // Load configuration from install.ini (TSLPatcher.exe: reverse engineering in progress)
  // String: "Unable to load the %s file! Make sure the "tslpatchdata" folder is located in the same folder as this application."
  ConfigFile := FTSLPatchDataPath + 'install.ini';
  
  if not FileExists(ConfigFile) then
  begin
    LogError(Format('Unable to load the %s file! Make sure the "tslpatchdata" folder is located in the same folder as this application.', ['install.ini']));
    Exit;
  end;
  
  Config := TTSLPatcherConfig.Create(ConfigFile);
  try
    Config.LoadFromFile(ConfigFile);
    FGamePath := Config.GamePath;
    FMakeBackups := Config.MakeBackups;
    FLogLevel := Config.LogLevel;
    FInstructionsText := Config.InstructionsText;
  finally
    Config.Free;
  end;
end;

procedure TMainForm.LoadInstructions;
var
  InfoFile: string;
  Instructions: TStringList;
begin
  // Load and display instructions (TSLPatcher.exe: reverse engineering in progress)
  // String: "Unable to load the instructions text! Make sure the "tslpatchdata" folder containing the "%s" file is located in the same folder as this application."
  InfoFile := FTSLPatchDataPath + 'install.txt';
  
  if not FileExists(InfoFile) then
  begin
    LogWarning(Format('Unable to load the instructions text! Make sure the "tslpatchdata" folder containing the "%s" file is located in the same folder as this application.', ['install.txt']));
    Exit;
  end;
  
  Instructions := TStringList.Create;
  try
    Instructions.LoadFromFile(InfoFile);
    FInstructionsText := Instructions.Text;
    // TODO: Display in RichEdit control (UI component needs to be defined)
  finally
    Instructions.Free;
  end;
end;

procedure TMainForm.ValidateGamePath;
begin
  if FGamePath = '' then
  begin
    LogError('Error! No install path has been set!');
    Exit;
  end;
  
  if not DirectoryExists(FGamePath) then
  begin
    LogError(Format('Game path does not exist: %s', [FGamePath]));
    Exit;
  end;
  
  // Check for game executable
  // TODO: STUB - Validate game installation (TSLPatcher.exe: reverse engineering in progress)
end;

procedure TMainForm.StartPatching;
begin
  LogInfo('Patch operation started...');
  
  // Validate paths
  ValidateGamePath;
  
  if FErrorCount > 0 then
    Exit;
  
  // Process patch operations
  ProcessPatchOperations;
  
  // Display completion message
  if FErrorCount = 0 then
  begin
    if FWarningCount = 0 then
      LogInfo('The Patcher is finished. Please check the progress log for details about what has been done.')
    else
      LogWarning(Format('The Patcher is finished, but %s warnings were encountered! The Mod may or may not be properly installed. Please check the progress log for further details.', [IntToStr(FWarningCount)]));
  end
  else
  begin
    if FWarningCount = 0 then
      LogError(Format('The Patcher is finished, but %s errors were encountered! The Mod has likely not been properly installed. Please check the progress log for further details.', [IntToStr(FErrorCount)]))
    else
      LogError(Format('The Patcher is finished, but %s errors and %s warnings were encountered! The Mod most likely has not been properly installed. Please check the progress log for further details.', [IntToStr(FErrorCount), IntToStr(FWarningCount)]));
  end;
end;

procedure TMainForm.ProcessPatchOperations;
begin
  // TODO: STUB - Process all patch operations (TSLPatcher.exe: reverse engineering in progress)
  // This should:
  // 1. Load blueprint files or manual source
  // 2. Process each patch operation (2DA, TLK, GFF, NSS, SSF, ERF, RIM, Install)
  // 3. Create backups if enabled
  // 4. Apply modifications
  // 5. Compile scripts if needed
  // 6. Install files to override folder
end;

procedure TMainForm.LogMessage(const AMessage: string; ALogLevel: TLogLevel);
begin
  if Ord(ALogLevel) <= Ord(FLogLevel) then
  begin
    FProgressLog.Add(AMessage);
    // TODO: STUB - Update RichEdit control (TSLPatcher.exe: reverse engineering in progress)
  end;
end;

procedure TMainForm.LogError(const AMessage: string);
begin
  Inc(FErrorCount);
  LogMessage(Format('Error! %s', [AMessage]), llErrors);
end;

procedure TMainForm.LogWarning(const AMessage: string);
begin
  Inc(FWarningCount);
  LogMessage(Format('Warning! %s', [AMessage]), llErrorsWarnings);
end;

procedure TMainForm.LogInfo(const AMessage: string);
begin
  LogMessage(AMessage, llStandard);
end;

procedure TMainForm.LogDebug(const AMessage: string);
begin
  LogMessage(AMessage, llDebug);
end;

{ TTSLPatcherConfig }

constructor TTSLPatcherConfig.Create(const AConfigFile: string);
begin
  inherited Create;
  if FileExists(AConfigFile) then
    FIniFile := TIniFile.Create(AConfigFile)
  else
    FIniFile := nil;
end;

destructor TTSLPatcherConfig.Destroy;
begin
  FIniFile.Free;
  inherited Destroy;
end;

procedure TTSLPatcherConfig.LoadFromFile(const AFileName: string);
begin
  // Load configuration from INI file (TSLPatcher.exe: reverse engineering in progress)
  // Parse install.ini structure
  // Based on string analysis, install.ini contains:
  // - Game path
  // - Backup settings
  // - Log level
  // - Instructions text location
  
  if FIniFile = nil then
    Exit;
  
  FGamePath := FIniFile.ReadString('Settings', 'GamePath', '');
  FMakeBackups := FIniFile.ReadBool('Settings', 'MakeBackups', True);
  FLogLevel := TLogLevel(FIniFile.ReadInteger('Settings', 'LogLevel', Ord(llStandard)));
  FInstructionsText := FIniFile.ReadString('Settings', 'InstructionsText', '');
end;

{ TTwoDAPatcher }

procedure TTwoDAPatcher.PatchFile(const AFileName: string; const AModifications: TStrings);
var
  Patcher: TwoDAPatcher.TTwoDAPatcher;
  I: Integer;
  ModList: TList;
  Mod: TwoDAPatcher.TTwoDAModification;
  Parts: TStringList;
begin
  // Implement 2DA file patching (TSLPatcher.exe: 0x00470000+, 0x00480000+)
  // Based on code extraction from Ghidra memory dumps
  // Strings found: "No 2da file has been loaded", "Unable to look up column labels", etc.
  
  if not FileExists(AFileName) then
    raise Exception.Create(Format('Error! File "%s" set to be patched does not exist!', [AFileName]));
  
  Patcher := TwoDAPatcher.TTwoDAPatcher.Create(AFileName);
  try
    ModList := TList.Create;
    try
      // Parse modifications from TStrings
      Parts := TStringList.Create;
      try
        for I := 0 to AModifications.Count - 1 do
        begin
          Parts.Clear;
          Parts.Delimiter := '|';
          Parts.DelimitedText := AModifications[I];
          
          if Parts.Count >= 3 then
          begin
            Mod := TwoDAPatcher.TTwoDAModification.Create;
            Mod.RowLabel := Parts[0];
            Mod.ColumnName := Parts[1];
            Mod.NewValue := Parts[2];
            if Parts.Count >= 4 then
              Mod.Exclusive := SameText(Parts[3], 'exclusive');
            if Parts.Count >= 6 then
            begin
              Mod.MatchColumn := Parts[4];
              Mod.MatchValue := Parts[5];
            end;
            ModList.Add(Mod);
          end;
        end;
      finally
        Parts.Free;
      end;
      
      // Apply all modifications
      Patcher.ApplyModifications(ModList);
      
      // Clean up modifications
      for I := 0 to ModList.Count - 1 do
        TwoDAPatcher.TTwoDAModification(ModList[I]).Free;
    finally
      ModList.Free;
    end;
  finally
    Patcher.Free;
  end;
end;

{ TTLKPatcher }

procedure TTLKPatcher.AppendDialog(const AFileName: string; const AEntries: TStrings);
begin
  // TODO: STUB - Implement dialog TLK appending (TSLPatcher.exe: reverse engineering in progress)
  raise Exception.Create('TLK appending: Reverse engineering in progress');
end;

procedure TTLKPatcher.ModifyEntries(const AFileName: string; const AModifications: TStrings);
begin
  // TODO: STUB - Implement TLK entry modification (TSLPatcher.exe: reverse engineering in progress)
  raise Exception.Create('TLK modification: Reverse engineering in progress');
end;

{ TGFFPatcher }

procedure TGFFPatcher.PatchFile(const AFileName: string; const AFieldPath: string; const AValue: Variant);
begin
  // TODO: STUB - Implement GFF file patching (TSLPatcher.exe: reverse engineering in progress)
  // Based on string analysis, this should support:
  // - Field path matching
  // - Field label matching
  // - Value modification
  raise Exception.Create('GFF patching: Reverse engineering in progress');
end;

{ TNSSPatcher }

procedure TNSSPatcher.CompileScript(const ASourceFile: string; const AOutputFile: string);
var
  NWNSSCompPath: string;
  CommandLine: string;
  StartupInfo: TStartupInfo;
  ProcessInfo: TProcessInformation;
  ExitCode: DWORD;
begin
  // Implement script compilation (TSLPatcher.exe: reverse engineering in progress)
  // String: "Could not locate nwnsscomp.exe in the TSLPatchData folder! Unable to compile scripts!"
  // String: "Compiling modified script %s..."
  // String: "NWNNSSComp says: %s"
  // String: "Unable to find compiled version of file \"%s\"! The compilation probably failed! Skipping..."
  // String: "Script \"%s\" has no start function, assuming include file. Compile skipped..."
  
  NWNSSCompPath := ExtractFilePath(Application.ExeName) + 'tslpatchdata\nwnsscomp.exe';
  
  if not FileExists(NWNSSCompPath) then
  begin
    raise Exception.Create('Could not locate nwnsscomp.exe in the TSLPatchData folder! Unable to compile scripts!');
  end;
  
  if not FileExists(ASourceFile) then
  begin
    raise Exception.Create(Format('Unable to find processed version of file, %s, cannot compile it!', [ASourceFile]));
  end;
  
  // Build command line: nwnsscomp.exe -i <input> -o <output>
  CommandLine := Format('"%s" -i "%s" -o "%s"', [NWNSSCompPath, ASourceFile, AOutputFile]);
  
  // Initialize startup info
  FillChar(StartupInfo, SizeOf(StartupInfo), 0);
  StartupInfo.cb := SizeOf(StartupInfo);
  StartupInfo.dwFlags := STARTF_USESTDHANDLES;
  
  // Create process
  if not CreateProcess(nil, PChar(CommandLine), nil, nil, False, 0, nil, nil, StartupInfo, ProcessInfo) then
  begin
    raise Exception.Create(Format('Failed to start nwnsscomp.exe: %s', [SysErrorMessage(GetLastError)]));
  end;
  
  try
    // Wait for compilation to complete
    WaitForSingleObject(ProcessInfo.hProcess, INFINITE);
    GetExitCodeProcess(ProcessInfo.hProcess, ExitCode);
    
    if ExitCode <> 0 then
    begin
      raise Exception.Create(Format('Script compilation failed with exit code %d', [ExitCode]));
    end;
    
    // Verify output file was created
    if not FileExists(AOutputFile) then
    begin
      raise Exception.Create(Format('Unable to find compiled version of file "%s"! The compilation probably failed! Skipping...', [AOutputFile]));
    end;
  finally
    CloseHandle(ProcessInfo.hProcess);
    CloseHandle(ProcessInfo.hThread);
  end;
end;

procedure TNSSPatcher.PatchNCS(const AFileName: string; const AIntegerHacks: TStrings);
begin
  // TODO: STUB - Implement NCS file integer hacks (TSLPatcher.exe: reverse engineering in progress)
  raise Exception.Create('NCS integer hacks: Reverse engineering in progress');
end;

{ TSSFPatcher }

procedure TSSFPatcher.PatchFile(const AFileName: string; const AModifications: TStrings);
begin
  // TODO: STUB - Implement SSF file patching (TSLPatcher.exe: reverse engineering in progress)
  raise Exception.Create('SSF patching: Reverse engineering in progress');
end;

{ TERFPatcher }

procedure TERFPatcher.PatchFile(const AFileName: string; const AModifications: TStrings);
begin
  // TODO: STUB - Implement ERF file patching (TSLPatcher.exe: reverse engineering in progress)
  raise Exception.Create('ERF patching: Reverse engineering in progress');
end;

{ TRIMPatcher }

procedure TRIMPatcher.PatchFile(const AFileName: string; const AModifications: TStrings);
begin
  // TODO: STUB - Implement RIM file patching (TSLPatcher.exe: reverse engineering in progress)
  raise Exception.Create('RIM patching: Reverse engineering in progress');
end;

{ TBackupManager }

function TBackupManager.CreateBackup(const AFileName: string): string;
var
  BackupDir: string;
  BackupFileName: string;
  FileStream: TFileStream;
  BackupStream: TFileStream;
begin
  // Create backup file (TSLPatcher.exe: reverse engineering in progress)
  // String: "Saving unaltered backup copy of %s in %s."
  // String: "Saving unaltered backup copy of destination file %s in %s."
  // String: "Making backup copy of script file \"%s\" found in override..."
  
  if not FileExists(AFileName) then
    raise Exception.Create(Format('Error! File "%s" set to be patched does not exist!', [AFileName]));
  
  // Create backup directory in same folder as file
  BackupDir := ExtractFilePath(AFileName) + 'backup\';
  if not DirectoryExists(BackupDir) then
    ForceDirectories(BackupDir);
  
  // Generate backup filename with timestamp
  BackupFileName := BackupDir + ExtractFileName(AFileName) + '.bak';
  
  // Copy file to backup location
  FileStream := TFileStream.Create(AFileName, fmOpenRead);
  try
    BackupStream := TFileStream.Create(BackupFileName, fmCreate);
    try
      BackupStream.CopyFrom(FileStream, 0);
    finally
      BackupStream.Free;
    end;
  finally
    FileStream.Free;
  end;
  
  Result := BackupFileName;
end;

procedure TBackupManager.RestoreBackup(const ABackupFile: string; const ATargetFile: string);
var
  BackupStream: TFileStream;
  TargetStream: TFileStream;
begin
  // Restore from backup (TSLPatcher.exe: reverse engineering in progress)
  if not FileExists(ABackupFile) then
    raise Exception.Create(Format('Backup file "%s" does not exist!', [ABackupFile]));
  
  // Copy backup to target location
  BackupStream := TFileStream.Create(ABackupFile, fmOpenRead);
  try
    TargetStream := TFileStream.Create(ATargetFile, fmCreate);
    try
      TargetStream.CopyFrom(BackupStream, 0);
    finally
      TargetStream.Free;
    end;
  finally
    BackupStream.Free;
  end;
end;

end.

