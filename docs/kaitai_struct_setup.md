# Kaitai Struct Compiler Setup

This document describes how to set up and use the Kaitai Struct compiler for BWM format definitions.

## Prerequisites

1. **Java 8 or later** - Required to run the Kaitai Struct compiler
   - Download from: https://adoptium.net/
   - Verify installation: `java -version`

## Installation

### Automatic Setup (Recommended)

Run the setup script:

```powershell
pwsh -ExecutionPolicy Bypass -File scripts/SetupKaitaiCompiler.ps1
```

This will:
- Check for Java installation
- Download Kaitai Struct Compiler v0.10
- Install to `%USERPROFILE%\.kaitai\kaitai-struct-compiler.jar`
- Set up environment for tests

### Manual Installation

1. Download Kaitai Struct Compiler from:
   https://github.com/kaitai-io/kaitai_struct_compiler/releases

2. Extract the JAR file to a location of your choice

3. Set environment variable:
   ```powershell
   $env:KAITAI_COMPILER_JAR = "C:\path\to\kaitai-struct-compiler.jar"
   ```

## Supported Languages

The Kaitai Struct compiler supports compilation to the following languages (13 total):

1. **python** - Python 3.x
2. **java** - Java 8+
3. **javascript** - JavaScript (Node.js or browser)
4. **csharp** - C# (.NET)
5. **cpp_stl** - C++ (STL)
6. **go** - Go
7. **ruby** - Ruby
8. **php** - PHP
9. **rust** - Rust
10. **swift** - Swift
11. **lua** - Lua
12. **nim** - Nim
13. **perl** - Perl

## Usage

### Compile BWM.ksy to a single language:

```bash
java -jar "%USERPROFILE%\.kaitai\kaitai-struct-compiler.jar" -t python src/Andastra/Parsing/Resource/Formats/BWM/BWM.ksy -d output/python/
```

### Compile to multiple languages:

```bash
java -jar "%USERPROFILE%\.kaitai\kaitai-struct-compiler.jar" -t python -t java -t javascript -t csharp src/Andastra/Parsing/Resource/Formats/BWM/BWM.ksy -d output/
```

### Compile all supported languages:

```bash
java -jar "%USERPROFILE%\.kaitai\kaitai-struct-compiler.jar" -t python -t java -t javascript -t csharp -t cpp_stl -t go -t ruby -t php -t rust -t swift -t lua -t nim -t perl src/Andastra/Parsing/Resource/Formats/BWM/BWM.ksy -d output/
```

## Testing

The test suite includes comprehensive tests for Kaitai Struct compiler functionality:

- `BWMKaitaiCompilerTests.cs` - Tests compilation to all supported languages

Run tests:

```bash
dotnet test --filter "FullyQualifiedName~BWMKaitaiCompilerTests"
```

## Generated Parser Usage

### Python Example

```python
from bwm import Bwm

with open('test.wok', 'rb') as f:
    bwm = Bwm.from_io(f)
    print(f"Magic: {bwm.header.magic}")
    print(f"Version: {bwm.header.version}")
    print(f"Walkmesh Type: {bwm.walkmesh_properties.walkmesh_type}")
    print(f"Vertex Count: {bwm.data_table_offsets.vertex_count}")
```

### Java Example

```java
import io.kaitai.struct.ByteBufferKaitaiStream;
import io.kaitai.struct.KaitaiStream;
import Bwm;

// ...
KaitaiStream ks = new ByteBufferKaitaiStream(data);
Bwm bwm = new Bwm(ks);
System.out.println("Magic: " + bwm.header().magic());
System.out.println("Version: " + bwm.header().version());
```

### JavaScript Example

```javascript
const Bwm = require('./bwm');

const fs = require('fs');
const data = fs.readFileSync('test.wok');
const bwm = new Bwm(new KaitaiStream(data));
console.log('Magic:', bwm.header.magic);
console.log('Version:', bwm.header.version);
```

## CI/CD Integration

For CI/CD pipelines, install the compiler in your workflow:

```yaml
- name: Setup Java
  uses: actions/setup-java@v5
  with:
    distribution: temurin
    java-version: '17'

- name: Install Kaitai Struct Compiler
  run: |
    wget https://github.com/kaitai-io/kaitai_struct_compiler/releases/download/0.10/kaitai-struct-compiler-0.10.zip
    unzip kaitai-struct-compiler-0.10.zip
    export KAITAI_COMPILER_JAR=$(pwd)/kaitai-struct-compiler-0.10/kaitai-struct-compiler.jar

- name: Compile BWM.ksy
  run: |
    java -jar $KAITAI_COMPILER_JAR -t python -t java -t javascript src/Andastra/Parsing/Resource/Formats/BWM/BWM.ksy -d compiled/
```

## Troubleshooting

### "Java not found"
- Install Java 8 or later from https://adoptium.net/
- Ensure Java is in your PATH: `java -version`

### "Kaitai compiler not found"
- Run the setup script: `scripts/SetupKaitaiCompiler.ps1`
- Or set `KAITAI_COMPILER_JAR` environment variable to the JAR path

### Compilation errors for specific languages
- Some languages may require additional runtime libraries
- Check Kaitai Struct documentation for language-specific requirements
- Tests will skip languages that fail to compile gracefully

## References

- Kaitai Struct Documentation: https://doc.kaitai.io/
- Kaitai Struct Compiler Releases: https://github.com/kaitai-io/kaitai_struct_compiler/releases
- BWM Format Documentation: `vendor/PyKotor/wiki/BWM-File-Format.md`

