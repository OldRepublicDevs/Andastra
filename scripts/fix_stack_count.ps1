$file = "src/TSLPatcher.tests/Formats/NCSCompilerTests.cs"
$content = Get-Content $file -Raw

# Fix .Stack[Stack.Count - N] patterns
# Need to capture the snapshot variable first
$content = $content -replace 'interpreter\.StackSnapshots\[interpreter\.StackSnapshots\.Count - (\d+)\]\.Stack\[Stack\.Count - (\d+)\]', 'interpreter.StackSnapshots[interpreter.StackSnapshots.Count - $1].Stack[interpreter.StackSnapshots[interpreter.StackSnapshots.Count - $1].Stack.Count - $2]'

Set-Content $file -Value $content -NoNewline

