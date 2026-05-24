# Build Health Matrix

Per-project build expectations for agents and CI.

## Green Path (recommended baseline)

Verified on Linux with .NET 9: `[REPO]` (session 2026-05-23)

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet build tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0
dotnet test tests/Andastra.Tests/Andastra.Tests.csproj --framework net9.0
```

| Project | Status | Notes |
|---------|--------|-------|
| BioWare | Green | net9.0 + net48 |
| BioWare.Tests | Green | Format roundtrips |
| Andastra.Tests | Green | NCS roundtrip |
| NSSComp | Green | CLI `--help` works |
| NCSDecomp.CLI | Green | CLI `--help` works |
| Standalone OdyTool editors | Mostly green | Individual csprojs |
| OdyTools.Tests | Green when OdyTools builds | Depends on parent |

## Red / Broken

| Project | Status | Error class |
|---------|--------|-------------|
| **OdyTools** (AIO) | Red | Method group → `System.Action` conversion `[REPO]` (`AGENTS.md`) |
| **OdyPatch** | Red | Depends on OdyTools |
| **OdyPatch.UI** | Red | Transitive |
| **KotorCLI** | Red | System.CommandLine API bug — crashes on startup `[REPO]` |

## Full Solution

```bash
dotnet build Andastra.sln
```

May fail on OdyTools/OdyPatch even when core runtime path is healthy. `[REPO]`

Do **not** claim full-solution green without explicit verification after OdyTools fix.

## Missing / Orphan Artifacts

| Item | Issue |
|------|-------|
| `src/MonoGameFPS/` | Referenced in docs; gitignored / absent |
| `src/StrideGameFPS/` | On disk; not in Andastra.sln |
| `tests/Runtime/Parsing/MDL/MDLAsciiTests.cs` | Orphan — not in test csproj |
| `tests/Runtime/Graphics/.../VulkanDeviceScratchBufferTests.cs` | Orphan |

`[REPO]`

## Repo Implications

- Agents default to BioWare + tests ladder — see [build-and-test-ladder.md](../50-execution/build-and-test-ladder.md).
- Tooling PRs should state which tool csproj was built.
- Fixing OdyTools unblocks mod installer UX validation.
