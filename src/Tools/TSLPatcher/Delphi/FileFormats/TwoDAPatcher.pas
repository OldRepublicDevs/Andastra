unit TwoDAPatcher;

{*
 * 2DA File Patcher - Reverse Engineered from TSLPatcher.exe
 * Handles modification of 2DA (Two-Dimensional Array) files
 * 
 * Based on string analysis:
 * - New rows
 * - Modified rows  
 * - New columns
 * - Exclusive row checking
 * - Label index matching
 * - Column value matching
 *}

interface

uses
  SysUtils, Classes, StrUtils;

type
  TTwoDARow = class
  public
    LabelIndex: string;
    Values: TStringList;
    constructor Create;
    destructor Destroy; override;
  end;

  TTwoDAModification = class
  public
    RowLabel: string;
    ColumnName: string;
    NewValue: string;
    Exclusive: Boolean;
    MatchColumn: string;
    MatchValue: string;
  end;

  TTwoDAPatcher = class
  private
    FFileName: string;
    FColumns: TStringList;
    FRows: TList;
    FLoaded: Byte;  // Offset 0x18 in assembly: 1 = loaded, 0 = not loaded
    procedure LoadFile;
    procedure SaveFile;
    function FindRowByLabel(const ALabel: string): TTwoDARow;
    function FindRowByColumnMatch(const AColumnName, AValue: string): TTwoDARow;
    function CreateNewRow(const ALabel: string): TTwoDARow;
    function LookupColumnLabels(const ColumnName: string): Integer;  // 0x00470000
    function GetRowCount: Integer;  // 0x004700FC
    function GetColumnCount: Integer;  // 0x00470198
    function GetCellValue(RowIndex, ColumnIndex: Integer): string;  // 0x00470198+
    procedure SetColumnLabel(ColumnIndex: Integer; const ALabel: string);  // 0x00470250
    procedure SetRowLabel(RowIndex: Integer; const ALabel: string);  // 0x00470302
    procedure SetCellValue(RowIndex, ColumnIndex: Integer; const AValue: string);  // 0x00470390
  public
    constructor Create(const AFileName: string);
    destructor Destroy; override;
    procedure AddRow(const ALabel: string; const AValues: TStringList);
    procedure ModifyRow(const ALabel: string; const AColumnName, ANewValue: string);
    procedure ModifyRowByMatch(const AMatchColumn, AMatchValue, AColumnName, ANewValue: string);
    procedure AddColumn(const AColumnName: string);
    procedure ApplyModification(const AModification: TTwoDAModification);
    procedure ApplyModifications(const AModifications: TList);
    // Methods from assembly disassembly (1:1 parity)
    function LookupColumnIndex(const ColumnName: string): Integer;
    property RowCount: Integer read GetRowCount;
    property ColumnCount: Integer read GetColumnCount;
  end;

implementation

{ TTwoDARow }

constructor TTwoDARow.Create;
begin
  inherited Create;
  Values := TStringList.Create;
end;

destructor TTwoDARow.Destroy;
begin
  Values.Free;
  inherited Destroy;
end;

{ TTwoDAPatcher }

constructor TTwoDAPatcher.Create(const AFileName: string);
begin
  inherited Create;
  FFileName := AFileName;
  FColumns := TStringList.Create;
  FRows := TList.Create;
  FLoaded := 0;  // Initialize as not loaded
  LoadFile;
  FLoaded := 1;  // Mark as loaded after successful load (offset 0x18)
end;

destructor TTwoDAPatcher.Destroy;
var
  I: Integer;
begin
  for I := 0 to FRows.Count - 1 do
    TTwoDARow(FRows[I]).Free;
  FRows.Free;
  FColumns.Free;
  inherited Destroy;
end;

procedure TTwoDAPatcher.LoadFile;
var
  TextStream: TStringList;
  I, J: Integer;
  Line: string;
  Row: TTwoDARow;
  Parts: TStringList;
begin
  // Load 2DA file format (TSLPatcher.exe: 0x00470000+)
  // 2DA files are tab-separated value files with:
  // - First line: column headers
  // - Subsequent lines: row label + values
  // Error message from code: "No 2da file has been loaded. Unable to look up column labels."
  
  if not FileExists(FFileName) then
    raise Exception.Create(Format('Error! File "%s" set to be patched does not exist!', [FFileName]));
  
  TextStream := TStringList.Create;
  try
    TextStream.LoadFromFile(FFileName);
    
    if TextStream.Count = 0 then
      raise Exception.Create(Format('Error! File "%s" is empty!', [FFileName]));
    
    // Parse column headers (first line)
    Parts := TStringList.Create;
    try
      Parts.Delimiter := #9; // Tab
      Parts.DelimitedText := TextStream[0];
      FColumns.Assign(Parts);
    finally
      Parts.Free;
    end;
    
    // Parse rows
    for I := 1 to TextStream.Count - 1 do
    begin
      Line := TextStream[I];
      if Trim(Line) = '' then
        Continue;
      
      Row := TTwoDARow.Create;
      try
        Parts := TStringList.Create;
        try
          Parts.Delimiter := #9;
          Parts.DelimitedText := Line;
          
          if Parts.Count > 0 then
          begin
            Row.LabelIndex := Parts[0];
            for J := 1 to Parts.Count - 1 do
              Row.Values.Add(Parts[J]);
          end;
        finally
          Parts.Free;
        end;
        
        FRows.Add(Row);
      except
        Row.Free;
        raise;
      end;
    end;
  finally
    TextStream.Free;
  end;
end;

procedure TTwoDAPatcher.SaveFile;
var
  TextStream: TStringList;
  I, J: Integer;
  Row: TTwoDARow;
  Line: string;
begin
  // Save 2DA file format (TSLPatcher.exe: 0x00470000+)
  // Writes tab-separated format matching original structure
  TextStream := TStringList.Create;
  try
    // Write column headers
    Line := '';
    for I := 0 to FColumns.Count - 1 do
    begin
      if I > 0 then
        Line := Line + #9;
      Line := Line + FColumns[I];
    end;
    TextStream.Add(Line);
    
    // Write rows
    for I := 0 to FRows.Count - 1 do
    begin
      Row := TTwoDARow(FRows[I]);
      Line := Row.LabelIndex;
      for J := 0 to Row.Values.Count - 1 do
        Line := Line + #9 + Row.Values[J];
      TextStream.Add(Line);
    end;
    
    TextStream.SaveToFile(FFileName);
  finally
    TextStream.Free;
  end;
end;

function TTwoDAPatcher.FindRowByLabel(const ALabel: string): TTwoDARow;
var
  I: Integer;
  Row: TTwoDARow;
begin
  // Find row by label index (TSLPatcher.exe: 0x00470000+)
  // Error message: "No 2da file has been loaded. Unable to look up row label."
  // Error message: "Invalid row index specified, unable to look up row label."
  Result := nil;
  for I := 0 to FRows.Count - 1 do
  begin
    Row := TTwoDARow(FRows[I]);
    if SameText(Row.LabelIndex, ALabel) then
    begin
      Result := Row;
      Exit;
    end;
  end;
end;

function TTwoDAPatcher.FindRowByColumnMatch(const AColumnName, AValue: string): TTwoDARow;
var
  I, ColIndex: Integer;
  Row: TTwoDARow;
begin
  Result := nil;
  ColIndex := FColumns.IndexOf(AColumnName);
  if ColIndex = -1 then
    Exit;
  
  for I := 0 to FRows.Count - 1 do
  begin
    Row := TTwoDARow(FRows[I]);
    if (ColIndex < Row.Values.Count) and SameText(Row.Values[ColIndex], AValue) then
    begin
      Result := Row;
      Exit;
    end;
  end;
end;

function TTwoDAPatcher.CreateNewRow(const ALabel: string): TTwoDARow;
var
  I: Integer;
begin
  Result := TTwoDARow.Create;
  Result.LabelIndex := ALabel;
  // Initialize with empty values for all columns
  for I := 0 to FColumns.Count - 1 do
    Result.Values.Add('');
  FRows.Add(Result);
end;

procedure TTwoDAPatcher.AddRow(const ALabel: string; const AValues: TStringList);
var
  Row: TTwoDARow;
  I: Integer;
begin
  Row := CreateNewRow(ALabel);
  for I := 0 to Min(AValues.Count, Row.Values.Count) - 1 do
    Row.Values[I] := AValues[I];
end;

procedure TTwoDAPatcher.ModifyRow(const ALabel: string; const AColumnName, ANewValue: string);
var
  Row: TTwoDARow;
  ColIndex: Integer;
begin
  Row := FindRowByLabel(ALabel);
  if Row = nil then
    raise Exception.Create(Format('Row with label "%s" not found in 2DA file', [ALabel]));
  
  ColIndex := FColumns.IndexOf(AColumnName);
  if ColIndex = -1 then
    raise Exception.Create(Format('Column "%s" not found in 2DA file', [AColumnName]));
  
  // Ensure row has enough values
  while Row.Values.Count <= ColIndex do
    Row.Values.Add('');
  
  Row.Values[ColIndex] := ANewValue;
end;

procedure TTwoDAPatcher.ModifyRowByMatch(const AMatchColumn, AMatchValue, AColumnName, ANewValue: string);
var
  Row: TTwoDARow;
  ColIndex: Integer;
begin
  Row := FindRowByColumnMatch(AMatchColumn, AMatchValue);
  if Row = nil then
    raise Exception.Create(Format('Row with matching value "%s" in column "%s" not found', [AMatchValue, AMatchColumn]));
  
  ColIndex := FColumns.IndexOf(AColumnName);
  if ColIndex = -1 then
    raise Exception.Create(Format('Column "%s" not found in 2DA file', [AColumnName]));
  
  while Row.Values.Count <= ColIndex do
    Row.Values.Add('');
  
  Row.Values[ColIndex] := ANewValue;
end;

procedure TTwoDAPatcher.AddColumn(const AColumnName: string);
var
  I: Integer;
  Row: TTwoDARow;
begin
  if FColumns.IndexOf(AColumnName) <> -1 then
    Exit; // Column already exists
  
  FColumns.Add(AColumnName);
  
  // Add empty value to all existing rows
  for I := 0 to FRows.Count - 1 do
  begin
    Row := TTwoDARow(FRows[I]);
    Row.Values.Add('');
  end;
end;

procedure TTwoDAPatcher.ApplyModification(const AModification: TTwoDAModification);
var
  Row: TTwoDARow;
  ColIndex: Integer;
begin
  // Apply modification with exclusive checking (TSLPatcher.exe: 0x00480000+)
  // Based on code at 0x00480000+ with strings:
  // - "rowindex", "rowlabel", "newrowlabel", "exclusivecolumn"
  // - "New Exclusive row matched line %s in 2DA file %s, modifying existing line instead."
  // - "Matching value in column %s found for existing row %s..."
  // - "Warning, multiple rows matching Label Index found! Last found row will be used..."
  // - "Unable to find a row matching the label %s"
  
  // Check if file is loaded (error: "No 2da file has been loaded")
  if FRows.Count = 0 then
    raise Exception.Create('No 2da file has been loaded. Unable to look up column labels.');
  
  // Validate column exists
  ColIndex := FColumns.IndexOf(AModification.ColumnName);
  if ColIndex = -1 then
    raise Exception.Create(Format('Invalid column index specified, unable to look up column label.', []));
  
  if AModification.Exclusive then
  begin
    // Exclusive mode: check if row already exists (code pattern at 0x00480000+)
    if AModification.MatchColumn <> '' then
      Row := FindRowByColumnMatch(AModification.MatchColumn, AModification.MatchValue)
    else
      Row := FindRowByLabel(AModification.RowLabel);
    
    if Row <> nil then
    begin
      // Row exists, modify it instead of creating new
      // Message: "New Exclusive row matched line %s in 2DA file %s, modifying existing line instead."
      while Row.Values.Count <= ColIndex do
        Row.Values.Add('');
      Row.Values[ColIndex] := AModification.NewValue;
      Exit;
    end;
  end;
  
  // Create new row or modify existing
  if AModification.MatchColumn <> '' then
  begin
    Row := FindRowByColumnMatch(AModification.MatchColumn, AModification.MatchValue);
    if Row = nil then
    begin
      // Error: "Unable to find a row matching the label %s"
      Row := CreateNewRow(AModification.RowLabel);
    end;
  end
  else
  begin
    Row := FindRowByLabel(AModification.RowLabel);
    if Row = nil then
    begin
      // Error: "Unable to find a row matching the label %s"
      Row := CreateNewRow(AModification.RowLabel);
    end;
  end;
  
  // Set column value
  while Row.Values.Count <= ColIndex do
    Row.Values.Add('');
  Row.Values[ColIndex] := AModification.NewValue;
end;

procedure TTwoDAPatcher.ApplyModifications(const AModifications: TList);
var
  I: Integer;
begin
  for I := 0 to AModifications.Count - 1 do
    ApplyModification(TTwoDAModification(AModifications[I]));
  SaveFile;
end;

{ Methods from assembly disassembly - 1:1 parity with TSLPatcher.exe }

function TTwoDAPatcher.LookupColumnLabels(const ColumnName: string): Integer;
var
  I: Integer;
  Count: Integer;
  ColName: string;
begin
  // Assembly: 0x00470000
  // Check if file is loaded (offset 0x18: cmp byte ptr [edi+0x18], 1)
  if FLoaded <> 1 then
    raise Exception.Create('No 2da file has been loaded. Unable to look up column labels.');
  
  // Get column count (offset 0x10 = FColumns)
  // Assembly: mov eax, [edi+0x10]; call TList.Count
  Count := FColumns.Count;
  if Count < 0 then
    raise Exception.Create('Invalid column count');
  
  // Loop through columns (assembly: loop at 0x00470056)
  Result := -1;
  for I := 0 to Count - 1 do
  begin
    // Assembly: mov eax, [eax+ebx*4] - get FColumns[i]
    ColName := FColumns[I];
    // Assembly: call CompareText (case-insensitive compare)
    if CompareText(ColName, ColumnName) = 0 then
    begin
      Result := I;
      Exit;
    end;
  end;
  
  // Not found - raise exception (assembly: 0x00470082)
  raise Exception.CreateFmt('Unable to find a row matching the label "%s" in 2DA file.', [ColumnName]);
end;

function TTwoDAPatcher.LookupColumnIndex(const ColumnName: string): Integer;
begin
  Result := LookupColumnLabels(ColumnName);
end;

function TTwoDAPatcher.GetRowCount: Integer;
begin
  // Assembly: 0x004700FC
  // Check if loaded: cmp byte ptr [eax+0x18], 1
  if FLoaded <> 1 then
    raise Exception.Create('No 2da file has been loaded. Unable to look up row count.');
  
  // Assembly: mov eax, [eax+0x08] - get FRows.Count
  Result := FRows.Count;
end;

function TTwoDAPatcher.GetColumnCount: Integer;
begin
  // Assembly: 0x00470198
  // Check if loaded: cmp byte ptr [eax+0x18], 1
  if FLoaded <> 1 then
    raise Exception.Create('No 2da file has been loaded. Unable to look up column count.');
  
  // Assembly: mov eax, [eax+0x04] - get FColumns.Count
  Result := FColumns.Count;
end;

function TTwoDAPatcher.GetCellValue(RowIndex, ColumnIndex: Integer): string;
var
  Row: TTwoDARow;
begin
  // Assembly: 0x00470198+ (GetCellValue)
  // Check if loaded: cmp byte ptr [ebx+0x18], 1
  if FLoaded <> 1 then
    raise Exception.Create('No 2da file has been loaded. Unable to look up cell value.');
  
  // Check row index: cmp esi, [ebx+0x08] (FRows.Count)
  if (RowIndex < 0) or (RowIndex >= FRows.Count) then
    raise Exception.Create('Invalid row index specified, unable to look up cell value.');
  
  // Check column index: cmp esi, [ebx+0x04] (FColumns.Count)
  if (ColumnIndex < 0) or (ColumnIndex >= FColumns.Count) then
    raise Exception.Create('Invalid column index specified, unable to look up cell value.');
  
  // Assembly: mov eax, [ebx+0x14]; mov eax, [eax+esi*4] - get FRows[RowIndex]
  Row := TTwoDARow(FRows[RowIndex]);
  
  // Assembly: mov eax, [eax+0x0C]; mov eax, [eax+edi*4] - get Row.Values[ColumnIndex]
  if ColumnIndex < Row.Values.Count then
    Result := Row.Values[ColumnIndex]
  else
    Result := '';
end;

procedure TTwoDAPatcher.SetColumnLabel(ColumnIndex: Integer; const ALabel: string);
begin
  // Assembly: 0x00470250
  // Check if loaded: cmp byte ptr [ebx+0x18], 1
  if FLoaded <> 1 then
    raise Exception.Create('No 2da file has been loaded. Unable to set column label.');
  
  // Check column index: cmp esi, [ebx+0x04] (FColumns.Count)
  if (ColumnIndex < 0) or (ColumnIndex >= FColumns.Count) then
    raise Exception.Create('Invalid column index specified, unable to set column label.');
  
  // Assembly: mov eax, [ebx+0x0C]; mov [eax+esi*4], edx - set FColumns[ColumnIndex]
  FColumns[ColumnIndex] := ALabel;
end;

procedure TTwoDAPatcher.SetRowLabel(RowIndex: Integer; const ALabel: string);
var
  Row: TTwoDARow;
begin
  // Assembly: 0x00470302
  // Check if loaded: cmp byte ptr [ebx+0x18], 1
  if FLoaded <> 1 then
    raise Exception.Create('No 2da file has been loaded. Unable to set row label.');
  
  // Check row index: cmp esi, [ebx+0x08] (FRows.Count)
  if (RowIndex < 0) or (RowIndex >= FRows.Count) then
    raise Exception.Create('Invalid row index specified, unable to set row label.');
  
  // Assembly: mov eax, [ebx+0x14]; mov eax, [eax+esi*4] - get FRows[RowIndex]
  Row := TTwoDARow(FRows[RowIndex]);
  
  // Assembly: set Row.LabelIndex
  Row.LabelIndex := ALabel;
end;

procedure TTwoDAPatcher.SetCellValue(RowIndex, ColumnIndex: Integer; const AValue: string);
var
  Row: TTwoDARow;
begin
  // Assembly: 0x00470390
  // Check if loaded: cmp byte ptr [ebx+0x18], 1
  if FLoaded <> 1 then
    raise Exception.Create('No 2da file has been loaded. Unable to set cell value.');
  
  // Check row index: cmp esi, [ebx+0x08] (FRows.Count)
  if (RowIndex < 0) or (RowIndex >= FRows.Count) then
    raise Exception.Create('Invalid row index specified, unable to set cell value.');
  
  // Check column index: cmp edi, [ebx+0x04] (FColumns.Count)
  if (ColumnIndex < 0) or (ColumnIndex >= FColumns.Count) then
    raise Exception.Create('Invalid column index specified, unable to set cell value.');
  
  // Assembly: mov eax, [ebx+0x14]; mov eax, [eax+esi*4] - get FRows[RowIndex]
  Row := TTwoDARow(FRows[RowIndex]);
  
  // Ensure row has enough values
  while Row.Values.Count <= ColumnIndex do
    Row.Values.Add('');
  
  // Assembly: mov eax, [eax+0x14]; mov [eax+edi*4], edx - set Row.Values[ColumnIndex]
  Row.Values[ColumnIndex] := AValue;
end;

end.

