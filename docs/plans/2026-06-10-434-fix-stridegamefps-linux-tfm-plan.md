---
title: "fix: StrideGameFPS Linux TargetFramework for NuGet CI restore"
type: fix
status: completed
date: 2026-06-10
origin: NETSDK1013 on Linux when NuGet/submit workflow restores all csproj files
---

# fix: StrideGameFPS Linux TargetFramework for NuGet CI restore

## Summary

`src/StrideGameFPS/StrideGameFPS.csproj` sets `TargetFramework` only on Windows (`net9.0-windows`). On Linux the non-Windows `PropertyGroup` omits `TargetFramework`, causing **NETSDK1013** when CI workflows run `dotnet restore` against discovered csproj files. Add `net9.0` for non-Windows with a stub entry point (existing `Net48Stub.cs` pattern) and exclude Stride-dependent sources so restore and build succeed without Stride packages.

---

## Problem Frame

- StrideGameFPS is on disk but **not** in `Andastra.sln`. `[REPO]`
- NuGet/submit workflows use `find . -name "*.csproj"` and restore the first N projects — StrideGameFPS is hit on Linux. `[REPO]`
- Stride 4.2 packages target **net9.0-windows** only; full game build remains Windows-only. `[REPO]`

---

## Requirements

- R1. Non-Windows `PropertyGroup` sets `<TargetFramework>net9.0</TargetFramework>`.
- R2. `dotnet restore src/StrideGameFPS/StrideGameFPS.csproj` succeeds on Linux.
- R3. `dotnet build src/StrideGameFPS/StrideGameFPS.csproj --framework net9.0` succeeds on Linux (stub only).
- R4. Windows `net9.0-windows` build unchanged (full Stride game, `Net48Stub.cs` excluded).
- R5. Plan index row **434**; optional KB note in build-health-matrix.

---

## Scope Boundaries

- Do not add StrideGameFPS to `Andastra.sln`.
- Do not change NuGet workflow find logic unless csproj fix is insufficient.
- No engine/AgentDecompile work.

---

## Implementation Units

- U1. **StrideGameFPS.csproj** — add `net9.0` TFM; conditional compile excludes for stub vs full game.
- U2. **Net48Stub.cs** — clarify stub message for non-Windows builds.
- U3. **docs/plans/README.md** — plan 434 index row.

**Verification:**

```bash
dotnet restore src/StrideGameFPS/StrideGameFPS.csproj
dotnet build src/StrideGameFPS/StrideGameFPS.csproj --framework net9.0 -c Release
```

---

## Repo implications

- Linux CI restore of orphan utility csproj no longer fails NETSDK1013.
- Full Stride demo remains Windows-only (`net9.0-windows`).
