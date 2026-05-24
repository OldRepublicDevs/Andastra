---
title: "fix: Isolate OdyTools standalone obj/bin paths"
type: fix
status: completed
date: 2026-05-23
origin: dotnet build Andastra.sln parallel Avalonia file-lock failures
---

# fix: Isolate OdyTools standalone obj/bin paths

## Summary

All `*Standalone.csproj` files under `Editors/` share `Editors/obj/` and `Editors/bin/`, causing Avalonia resource file locks and corrupted axaml paths during parallel `Andastra.sln` builds on Linux.

---

## Problem Frame

```
error MSB4018: The process cannot access the file '.../Editors/obj/Debug/net9.0/Avalonia/resources' because it is being used by another process.
Editors/home/brunner56/.../LocalizedStringDialog.axaml: AVLN2000 Unable to find type
```

Individual standalone builds succeed; full solution parallel build fails. `[REPO]`

---

## Requirements

- R1. Each standalone csproj uses unique `BaseIntermediateOutputPath` / `BaseOutputPath` via shared props.
- R2. Shared `$(_OdyToolsRoot)` AvaloniaXaml includes use `Link` metadata to avoid path concatenation bugs.
- R3. `dotnet build Andastra.sln --framework net9.0` completes without Avalonia obj collisions (may still warn on unrelated projects).
- R4. Update build-health-matrix / build-and-test-ladder with solution-build caveat resolution.
- R5. Drift register remediation 11.

---

## Implementation Units

- U1. **OdyTools.Standalone.Editor.props** — per-project obj/bin + Link on shared axaml
- U2. **Standalone csproj axaml Link pass** — all `$(_OdyToolsRoot)\*.axaml` includes
- U3. **Verify** — full solution build
- U4. **KB docs** — drift register + build ladder

---

## Scope Boundaries

- Do not remove standalone projects from solution.
- Stride assembly processor intermittent failures are secondary; verify after obj isolation.
