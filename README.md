# Andastra

A unified game engine runtime and development tooling suite for BioWare's engine families (Odyssey, Aurora, Eclipse, Infinity), built with .NET and MonoGame.

## Overview

Andastra is a .NET implementation providing a modern, cross-platform unified runtime for games built on BioWare's engine architectures. Similar in scope to projects like Xoreos, Andastra aims to create complete, faithful reimplementations of multiple BioWare engine families with a shared codebase and common abstractions.

The project unifies support for four major BioWare engine families:

- **Odyssey Engine**: Knights of the Old Republic (KOTOR), KOTOR II: The Sith Lords (TSL), Jade Empire
- **Aurora Engine**: Neverwinter Nights, Neverwinter Nights 2
- **Eclipse Engine**: Dragon Age series, Mass Effect series
- **Infinity Engine**: Baldur's Gate, Icewind Dale, Planescape: Torment

Currently, the Odyssey engine implementation is the most mature, with full support for KOTOR and TSL. The other engine families have foundational implementations with ongoing development. The project includes both the game engine runtime and a comprehensive suite of development tools for modding and content creation.

### Project Name

**Andastra** is named to reflect the project's foundational principles and its technical heritage:

- **Logical AND**: Symbolizes the intersection and integration of diverse systems, technologies, and approaches unified within this project.
- **Astraea**: In Greek mythology, Astraea is the daughter of Eos (goddess of the dawn). Similar to how Xoreos (which inspired our project's name) derives from XOR + EOS, the name "Andastra" (ASTRAea) draws inspiration from this mythological connection. This link emphasizes Andastra.NET's commitment to accuracy, justice, and the technical legacy of its predecessors.
- **Andraste**: References the Celtic war goddess, a symbol of determination and strength, and connects to BioWare lore as a prophet of "The Maker" in the Dragon Age series.

This name tributes Xoreos and reflects the project's goal of faithfully recreating BioWare's classic game engines across multiple engine families and bringing together modern .NET technologies with the precision.

### Supported Engine Families

Andastra provides a unified runtime architecture that supports multiple BioWare engine families through a common abstraction layer:

- **Odyssey Engine** (Primary Implementation): Full support for KOTOR and TSL with area rendering, navigation, scripting, dialogue, combat, and save/load systems
- **Aurora Engine** (In Development): Foundation for Neverwinter Nights and NWN2 support
- **Eclipse Engine** (In Development): Foundation for Dragon Age and Mass Effect series support
- **Infinity Engine** (In Development): Foundation for classic Infinity Engine games support

### Core Components

- **Unified Engine Runtime**: A multi-engine runtime architecture with engine-specific implementations sharing common abstractions
- **Development Tools**: A collection of utilities for modding, script compilation, file format manipulation, and content creation
- **File Format Support**: Complete parsing and manipulation support for all game file formats across engine families (GFF, 2DA, TLK, MDL, BWM, NCS, ERF, and more)

## Architecture

### Runtime Structure

The Andastra runtime is organized into a layered architecture with strict dependency rules:

```sh
┌─────────────────────────────────────────────────────────────┐
│         Andastra.Game (executable + launcher)                │
├─────────────────────────────────────────────────────────────┤
│  Game/Games/{Odyssey,Aurora,Eclipse} │  Game/Graphics/*     │
│  Game/Scripting (NCS VM)             │  Andastra.UI         │
├─────────────────────────────────────────────────────────────┤
│  Andastra.Runtime: Core │ Content │ Graphics │ Games/Common  │
├─────────────────────────────────────────────────────────────┤
│  Andastra.Core │ Andastra.Graphics                          │
├─────────────────────────────────────────────────────────────┤
│  BioWare (src/BioWare/) — file format parsers                │
└─────────────────────────────────────────────────────────────┘
```

Logical layers map to folders inside a small set of assemblies — not separate csproj files per layer. See the [knowledgebase runtime layering guide](docs/knowledgebase/10-architecture-runtime/runtime-layering.md) for the full mapping.

### Project Organization

**Runtime and game assemblies** (`src/Andastra/`):

- `Andastra.Runtime` — domain logic, content pipeline, graphics abstractions, and runtime-side common bases (`Runtime/Core/`, `Runtime/Content/`, `Runtime/Graphics/`, `Runtime/Games/Common/`)
- `Andastra.Game` — executable, engine implementations, NCS VM, and graphics backends (`Game/Games/`, `Game/Scripting/`, `Game/Graphics/`)
- `Andastra.Core` — shared core types
- `Andastra.Graphics` — graphics helper assembly
- `Andastra.UI` — Avalonia UI shell
- `Andastra.Game.Scripting.Abstractions` — scripting contracts shared with Runtime
- `Andastra` — aggregator/launcher shell (optional entry point)

**Engine implementations** (folders inside `Andastra.Game`, not separate csprojs):

- `Game/Games/Odyssey/` — Odyssey engine (KOTOR, TSL, Jade Empire)
- `Game/Games/Aurora/` — Aurora engine (NWN, NWN2)
- `Game/Games/Eclipse/` — Eclipse engine (Dragon Age, Mass Effect)
- `Game/Games/Infinity/` — planned; not yet present in the repository

**Supporting projects:**

- `BioWare` (`src/BioWare/`) — file format parsers and resource management
- `Andastra.Tests` (`tests/Andastra.Tests/`) — unit and integration tests
- `BioWare.Tests` (`tests/BioWare.Tests/`) — BioWare library tests

**Development tools** (`src/Tools/`):

- `OdyPatch` — runnable mod installer host (GUI + CLI; launches OdyPatch.UI)
- `OdyPatch.UI` — Avalonia UI library for the installer (build only; not directly runnable)
- `NCSDecomp.CLI` — NWScript bytecode decompiler
- `NSSComp` — NWScript compiler
- `OdyTools` — content creation and editing tools
- `KotorDiff` — file comparison and diff tool

## Features

### Engine Runtime

- **Area System**: Complete area loading with LYT layout, VIS visibility culling, and room mesh rendering
- **Navigation**: Walkmesh-based pathfinding with A* algorithm, surface material support, and dynamic obstacle handling
- **Entity System**: Component-based architecture supporting creatures, doors, placeables, triggers, waypoints, and more
- **Scripting**: Full NCS virtual machine implementation with NWScript engine API surface
- **Dialogue**: DLG conversation system with voice-over playback and lip-sync support
- **Combat**: Round-based combat system with d20 mechanics, damage calculation, and effect application
- **Save/Load**: Complete save game serialization compatible with original game formats
- **Mod Support**: Full resource precedence chain (override → module → save → chitin) matching original behavior

### Development Tools

- **OdyPatch**: Comprehensive mod installation tool with support for 2DA, GFF, TLK, NSS/NCS, and SSF modifications
- **NCSDecomp**: Decompile NWScript bytecode back to source with full instruction analysis
- **Script Compiler**: Compile NWScript source files to bytecode with KOTOR 1/2 compatibility
- **Format Tools**: Read, write, and manipulate all game file formats

## Requirements

- **.NET 9.0 SDK** or later
- **MonoGame 3.8** or later (for runtime)

## Building

### Quick Start

```bash
# Clone the repository
git clone <repository-url>
cd Andastra

# Recommended green path (Linux: pass --framework net9.0)
dotnet build src/BioWare/BioWare.csproj --framework net9.0

# Full solution
dotnet build Andastra.sln --framework net9.0

# Release configuration
dotnet build Andastra.sln --configuration Release --framework net9.0
```

## Running

### Game Runtime

```bash
# Run the game (requires game installation)
dotnet run --project src/Andastra/Game/Andastra.Game.csproj --framework net9.0

# Or specify game path
dotnet run --project src/Andastra/Game/Andastra.Game.csproj --framework net9.0 -- --game-path "C:\Games\KOTOR"
```

### Development Tools

```bash
# Run OdyPatch (GUI by default; CLI with --install / --validate / --uninstall)
dotnet run --project src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0

# Run NCSDecomp CLI
dotnet run --project src/Tools/NCSDecomp.CLI/NCSDecomp.CLI.csproj --framework net9.0

# Run script compiler
dotnet run --project src/Tools/NSSComp/NSSComp.csproj --framework net9.0
```

## Project Structure

```shell
Andastra/
├── src/
│   ├── Andastra/
│   │   ├── Game/              # Andastra.Game — executable, engines, scripting
│   │   │   ├── Games/         # Odyssey, Aurora, Eclipse engine code
│   │   │   ├── Scripting/     # NCS VM
│   │   │   └── Graphics/      # MonoGame / Stride backends
│   │   ├── Runtime/           # Andastra.Runtime — domain, content, abstractions
│   │   │   ├── Core/
│   │   │   ├── Content/
│   │   │   ├── Graphics/
│   │   │   └── Games/Common/
│   │   ├── Core/
│   │   ├── Graphics/
│   │   └── UI/
│   ├── BioWare/               # Format parsers and extract
│   └── Tools/                 # Development tools
│       ├── OdyPatch/
│       ├── OdyPatch.UI/
│       ├── NCSDecomp.CLI/
│       ├── NSSComp/
│       └── OdyTools/
├── tests/
│   ├── Andastra.Tests/
│   └── BioWare.Tests/
├── docs/                      # Documentation and knowledgebase
├── helper_scripts/            # Build and utility scripts
└── vendor/                    # Third-party dependencies
```

## Development

### Code Standards

- **C# Language Version**: Maximum C# 7.3 (for .NET Framework 4.x compatibility)
- **Architecture**: Layered architecture with strict dependency rules
- **Testing**: Comprehensive unit and integration tests
- **Documentation**: XML documentation comments for public APIs

### Key Design Principles

1. **Unified Multi-Engine Architecture**: Common abstractions enable support for multiple BioWare engine families with shared code
2. **Layered Architecture**: Core domain logic is independent of rendering and game-specific code
3. **Component-Based Entities**: Entity system uses composition over inheritance
4. **Resource Precedence**: Matches original game resource loading behavior exactly for each engine
5. **Script Compatibility**: NCS VM maintains bytecode compatibility with original engines
6. **Mod Support**: Full compatibility with existing mod formats and tools across all supported engines

### Adding New Features

When implementing new engine features:

1. **Core Domain First**: Implement pure domain logic in `Runtime/Core/`
2. **Content Pipeline**: Add asset conversion in `Runtime/Content/`
3. **Engine Abstraction**: Add shared runtime interfaces in `Runtime/Games/Common/` or game-loop glue in `Game/Games/Common/` as appropriate
4. **Engine Implementation**: Implement engine-specific behavior in `Game/Games/{Engine}/` (for example `Game/Games/Odyssey/`)
5. **Rendering**: Add backend adapters in `Game/Graphics/MonoGame/` or `Game/Graphics/Stride/`
6. **Tests**: Write comprehensive tests for deterministic logic

## Testing

```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --verbosity detailed

# Run specific test project
dotnet test tests/Andastra.Tests/Andastra.Tests.csproj --framework net9.0

# Run with code coverage
dotnet test --collect:"XPlat Code Coverage"
```

## Documentation

- **[Knowledgebase](docs/knowledgebase/90-meta/README.md)** - Evidence-first project docs for agents and contributors (start here)
- **[Quick Start Guide](docs/QUICKSTART.md)** - Getting started with development
- **[Engine Roadmap](docs/engine_roadmap.md)** - Implementation roadmap and status
- **[Architecture Documentation](docs/)** - Detailed architecture and design documents

## Contributing

When contributing to Andastra:

1. Follow the layered architecture and dependency rules
2. Maintain C# 7.3 compatibility
3. Write tests for new features
4. Document public APIs with XML comments
5. Match original engine behavior where applicable
6. Keep engine-specific logic in `Game/Games/{Engine}/` (for example `Game/Games/Odyssey/`)
7. Use shared abstractions in `Runtime/Games/Common/` and `Game/Games/Common/` for cross-engine functionality

## License

This project is licensed under AGPLv3. See the [LICENSE](LICENSE) file for details.

## Status

Andastra is under active development. The Odyssey engine implementation is the most mature, with core systems functional for KOTOR and TSL. The Aurora, Eclipse, and Infinity engine families have foundational implementations with ongoing development. See the [engine roadmap](docs/engine_roadmap.md) for detailed implementation status across all engine families.

**Status**: Active reverse engineering project - Contributions welcome!
