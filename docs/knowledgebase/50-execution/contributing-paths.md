# Contributing Paths

Where to add features and how to align with project structure.

## Layering (from README, corrected)

1. **Domain logic** → `src/Andastra/Runtime/Core/` (no graphics deps)
2. **Shared game abstractions** → `Runtime/Games/Common/` and `Game/Games/Common/`
3. **Engine-specific** → `src/Andastra/Game/Games/{Odyssey,Aurora,Eclipse}/`
4. **File formats** → `src/BioWare/Resource/Formats/`
5. **Tools** → `src/Tools/` (OdyPatch, editors, CLIs)

See [runtime-layering.md](../10-architecture-runtime/runtime-layering.md).

## Adding Runtime Features

| Feature type | Target |
|--------------|--------|
| K1/TSL gameplay | `Game/Games/Odyssey/` |
| Cross-engine shared | `Game/Games/Common/` or `Runtime/Games/Common/` |
| Rendering | `Game/Graphics/` or `Runtime/Graphics/` |
| NWScript / VM | `Game/Scripting/` |

## Adding Format Support

1. Parser in BioWare
2. Test in `tests/BioWare.Tests/Formats/`
3. Wiki spec if mod-facing
4. Update [file-format-catalog.md](../20-domain-theory/file-format-catalog.md)

## Adding Tools

- Mod installer: extend **OdyPatch** host and **OdyPatch.UI** library (not HoloPatcher); patch engine in `BioWare.TSLPatcher`. Run/test via `OdyPatch.csproj`. `[REPO]`
- NuGet packaging: `OdyPatch.csproj` + `helper_scripts/build-nuget.{sh,ps1}` — see [NUGET.md](../../NUGET.md)
- Standalone editor: new csproj under `src/Tools/OdyTools/Editors/Standalone/` following existing `OdyTool*.Standalone.csproj` pattern
- Standalone editors inherit `Editors/Directory.Build.props` (per-project `obj/`/`bin/`) and `OdyTools.Standalone.Editor.props` (shared sources, Avalonia `Link` metadata) — do not share a global `Editors/obj/` path
- CLI: follow NSSComp/NCSDecomp.CLI structure

## Documentation Updates

| Change type | Update |
|-------------|--------|
| Architecture / ops | `docs/knowledgebase/` |
| Tool installer/editor UX | `docs/knowledgebase/30-product-ux/` (stub layer) |
| KotOR behavior discovery | `wiki/` |
| Investigation / RE report | `docs/` (dated, may go stale) |
| Agent rules | `.cursorrules` (high bar) |

## Tests

- New behavior: add tests in appropriate test project
- Engine changes: manual game validation + RE citations
- Format changes: roundtrip tests mandatory

## Repo Implications

- Infinity/Aurora/Eclipse contributions need scoping against [engine-family-scope.md](../00-intent/engine-family-scope.md).
- Definition of done: [definition-of-done.md](../00-intent/definition-of-done.md).
