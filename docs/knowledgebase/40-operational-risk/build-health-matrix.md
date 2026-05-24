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
| **OdyTools** (AIO) | Green | Delegate wiring fixed 2026-05-23 |
| **OdyPatch** / **OdyPatch.UI** | Green | OdyTools reference restored 2026-05-23 |
| **OdyPatch NuGet pack** | Green | SPDX `LGPL-3.0-only`; `helper_scripts/build-nuget.sh` on net9.0 Linux (plan 035) |
| Standalone OdyTool editors | Green | Shared props include DialogHelper 2026-05-23 |
| OdyTools.Tests | Green when OdyTools builds | Depends on parent |
| **KotorCLI** | Green | System.CommandLine 2.0 Option API fixed 2026-05-23 |
| **ConvertKotorGame** | Green | Builds on net9.0 |
| **Andastra.sln** (full) | Green | Linux net9.0; CI `solution-build` uses `-m:1` to avoid parallel deps locks (plan 031) |

## Known Build Caveats

| Item | Notes |
|------|-------|
| Isolated game/runtime projects | Stride assembly processor may fail on isolated Linux builds of single game csproj `[REPO]` |
| OdyPatch runtime UX | Compile green; mod-install flow unverified without K1/TSL install `[REPO]` |
| Game runtime in CI | No K1/TSL install exercised in standard CI `[REPO]` |

## Full Solution

```bash
dotnet build Andastra.sln --framework net9.0
```

Green on Linux net9.0 after standalone obj isolation (plan 020). CI validates via `solution-build` job with `-m:1` (plan 031). Local agents may omit `-m:1` unless hitting parallel output locks. `[REPO]`

## Missing / Orphan Artifacts

| Item | Issue |
|------|-------|
| `src/MonoGameFPS/` | Referenced in older docs; not in solution; restore unaffected `[REPO]` |
| `src/StrideGameFPS/` | On disk; not in `Andastra.sln`; non-blocking `[REPO]` |
| `tests/Runtime/Parsing/MDL/MDLAsciiTests.cs` | Orphan — not in test csproj |
| `tests/Runtime/Graphics/.../VulkanDeviceScratchBufferTests.cs` | Orphan |

`[REPO]`

## Repo Implications

- Agents default to BioWare + tests ladder — see [build-and-test-ladder.md](../50-execution/build-and-test-ladder.md).
- Tooling PRs should state which tool csproj was built.
- OdyPatch mod-installer UX validation is unblocked at compile time; runtime UX still unverified.
