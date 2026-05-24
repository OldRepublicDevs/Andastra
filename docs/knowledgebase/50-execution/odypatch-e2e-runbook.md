# OdyPatch E2E Manual Runbook

Manual end-to-end verification path for OdyPatch mod install against a real K1 or TSL installation. CI cannot run this — standard agents use the validate fixture instead (plans 055/057).

## Prerequisites

| Requirement | Notes |
|-------------|-------|
| K1 or TSL install | Steam/GOG/CD path with `swkotor.exe` or `swkotor2.exe` `[REPO]` |
| Mod with TSLPatcher layout | Folder containing `tslpatchdata/changes.ini` (or equivalent) `[REPO]` |
| OdyPatch built | Release recommended: `dotnet build src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0 -c Release` `[REPO]` |
| Backup | Copy game directory or use a disposable test install before `--install` `[SYNTH]` |

## Validation ladder (no game install)

Run before attempting install. Mirrors CI `nuget-pack-smoke` and [build-and-test-ladder.md](build-and-test-ladder.md) Step 5. `[REPO]`

```bash
dotnet run --project src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0 -c Release -- --help
dotnet run --project src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0 -c Release -- \
  --validate --game-dir tests/fixtures/odypatch-fake-game \
  --tslpatchdata tests/fixtures/odypatch-minimal-mod/tslpatchdata
```

Expected: exit 0; validate prints completion message. `[REPO]`

## CLI install (real game)

Reference: `src/Tools/OdyPatch/Program.cs` — `--install`, `--validate`, and `--uninstall` are mutually exclusive. `[REPO]`

```bash
dotnet run --project src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0 -c Release -- \
  --install \
  --game-dir /path/to/KOTOR_or_TSL \
  --tslpatchdata /path/to/mod/tslpatchdata
```

Optional flags from `Program.cs`: `[REPO]`

| Flag | Purpose |
|------|---------|
| `--namespace-option-index <n>` | Select namespace option when mod defines multiple |
| `--console` | Console logging during CLI operation |
| `--uninstall` | Remove mod (same `--game-dir` / `--tslpatchdata` pair) |

## GUI install (real game)

```bash
dotnet run --project src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0
```

Launches Avalonia installer (OdyPatch.UI). Requires display server; headless environments should use CLI flags. See [odypatch-installer-ux.md](../30-product-ux/odypatch-installer-ux.md). `[REPO]`

## Manual verification checklist

| Step | Action | Pass criteria |
|------|--------|---------------|
| 1 | `--validate` on target mod + game dir | Exit 0, no config errors `[OPEN]` |
| 2 | `--install` on backup/test install | Exit 0, patches applied `[OPEN]` |
| 3 | Launch game, load affected content | Expected mod behavior in-game `[OPEN]` |
| 4 | `--uninstall` (if mod supports) | Game restored or documented partial state `[OPEN]` |

Claims marked `[OPEN]` until verified on a real install and recorded in [odypatch-installer-ux.md](../30-product-ux/odypatch-installer-ux.md).

## CI boundary

| Check | CI | Local manual |
|-------|-----|--------------|
| Pack + `--help` + `--validate` fixture | `nuget-pack-smoke` job | build-and-test-ladder Step 5 |
| `--install` against real game | **Not run** | This runbook |

## Related docs

- [run-tools-reference.md](run-tools-reference.md) — CLI entry points
- [build-and-test-ladder.md](build-and-test-ladder.md) — agent validation path
- [odypatch-installer-ux.md](../30-product-ux/odypatch-installer-ux.md) — product UX status table
- [tslpatcher-domain.md](../20-domain-theory/tslpatcher-domain.md) — patch semantics

## Repo implications

- Update verification status in `odypatch-installer-ux.md` when manual E2E is completed.
- Do not claim install parity with legacy TSLPatcher until checklist steps pass on real mods.
