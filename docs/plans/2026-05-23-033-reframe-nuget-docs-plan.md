---
title: "docs: Reframe NUGET.md for BioWare TSLPatcher + OdyPatch"
type: docs
status: completed
date: 2026-05-23
origin: PR #2 deferred NUGET TSLPatcher.Core drift from plan 032
---

# docs: Reframe NUGET.md for BioWare TSLPatcher + OdyPatch

## Summary

`docs/NUGET.md` still describes a non-existent `TSLPatcher.Core` NuGet project. TSLPatcher logic lives in `src/BioWare/TSLPatcher/` (`BioWare.TSLPatcher` assembly); only `OdyPatch` is `IsPackable`.

---

## Requirements

- R1. Replace `TSLPatcher.Core` references with accurate BioWare/OdyPatch distribution model.
- R2. Keep OdyPatch pack/push commands using `src/Tools/OdyPatch/` (from plan 032).
- R3. Update programmatic example to `BioWare.TSLPatcher` namespaces.
- R4. Cross-link `tslpatcher-domain.md`; drift register remediation **#24**.

---

## Scope Boundaries

- Do not enable `IsPackable` on BioWare.NET.TSLPatcher csproj.
- No workflow changes.
