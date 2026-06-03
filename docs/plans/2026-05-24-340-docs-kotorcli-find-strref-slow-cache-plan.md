---
title: "docs: KotorCLI find-strref slow vs cache NCS behavior"
type: docs
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-339-test-bioware-find-strref-cf-slow-cache-plan.md
branch: feat/plan-324-ncs-consti-conditional-strref
---

# docs: KotorCLI find-strref slow vs cache NCS behavior (plan 340)

## Summary

Plans **337**–**339** validated that `find-strref` **cache-path** indexing respects NCS CONSTI control-flow gating while the **slow path** (no cache) matches raw CONSTI operands. Document this intentional split in `src/Tools/KotorCLI/README.md` so mod authors and agents know when hits differ.

## Requirements

- R1. README `find-strref` section explains cache vs slow path selection (`--cache-file`, default no cache).
- R2. README documents NCS behavior: slow path = any matching CONSTI; cache path = `ShouldIndexAsStrRefCandidate` (control-flow + usage context + `--ncs-strref-min`).
- R3. README notes dead early-return locals: slow path may report; cache path excludes (plans **337**–**339**).
- R4. Plan index row **340**; PR #36 tracker sync for plan **339** complete and **340** landed.
- R5. Existing tests still pass (doc-only; no code change).

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FindStrRefCommandTests
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Doc-only; no scanner or API changes.
- Browser tests skipped (backend/doc-only).
