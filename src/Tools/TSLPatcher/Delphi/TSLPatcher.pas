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
  StrUtils;

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
begin
  ConfigFile := FTSLPatchDataPath + 'install.ini';
  
  if not FileExists(ConfigFile) then
  begin
    LogError(Format('Unable to load the %s file! Make sure the "tslpatchdata" folder is located in the same folder as this application.', ['install.ini']));
    Exit;
  end;
  
  // TODO: STUB - Load configuration from install.ini (TSLPatcher.exe: reverse engineering in progress)
  // Parse INI file structure
end;

procedure TMainForm.LoadInstructions;
var
  InfoFile: string;
begin
  InfoFile := FTSLPatchDataPath + 'install.txt';
  
  if not FileExists(InfoFile) then
  begin
    LogWarning(Format('Unable to load the instructions text! Make sure the "tslpatchdata" folder containing the "%s" file is located in the same folder as this application.', ['install.txt']));
    Exit;
  end;
  
  // Load instructions text
  // TODO: STUB - Load and display instructions (TSLPatcher.exe: reverse engineering in progress)
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
  // TODO: STUB - Load configuration from INI file (TSLPatcher.exe: reverse engineering in progress)
  // Parse install.ini structure
end;

{ TTwoDAPatcher }

procedure TTwoDAPatcher.PatchFile(const AFileName: string; const AModifications: TStrings);
begin
  // TODO: STUB - Implement 2DA file patching (TSLPatcher.exe: reverse engineering in progress)
  // Based on string analysis, this should support:
  // - New rows
  // - Modified rows
  // - New columns
  // - Exclusive row checking
  // - Label index matching
  raise Exception.Create('2DA patching: Reverse engineering in progress');
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
begin
  // TODO: STUB - Implement script compilation (TSLPatcher.exe: reverse engineering in progress)
  // Based on string analysis:
  // - Locate nwnsscomp.exe in TSLPatchData folder
  // - Compile NSS to NCS
  // - Handle compilation errors
  
  NWNSSCompPath := ExtractFilePath(Application.ExeName) + 'tslpatchdata\nwnsscomp.exe';
  
  if not FileExists(NWNSSCompPath) then
  begin
    raise Exception.Create('Could not locate nwnsscomp.exe in the TSLPatchData folder! Unable to compile scripts!');
  end;
  
  // TODO: Execute nwnsscomp.exe with proper parameters
  raise Exception.Create('Script compilation: Reverse engineering in progress');
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
begin
  // TODO: STUB - Create backup file (TSLPatcher.exe: reverse engineering in progress)
  Result := '';
end;

procedure TBackupManager.RestoreBackup(const ABackupFile: string; const ATargetFile: string);
begin
  // TODO: STUB - Restore from backup (TSLPatcher.exe: reverse engineering in progress)
end;

end.

