---
title: "docs: pr36 arc closure plan 063 sync"
type: docs
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-341-docs-kb-ncs-consti-cf-verification-plan.md
branch: feat/plan-324-ncs-consti-conditional-strref
---

# docs: PR #36 arc closure — plan 063 sync (plan 342)

## Summary

Plans **324**–**341** complete the NCS CONSTI control-flow arc on [PR #36](https://github.com/th3w1zard1/Andastra/pull/36). Close the parent plan **063** deferred note (CONSTI disambiguation) and finalize merge-readiness tracker before merge.

## Requirements

- R1. `docs/plans/2026-05-24-063-...`: update CONSTI deferred row — control-flow arc **324**–**341** landed on PR #36; **74** NcsConsti tests; full stack simulation remains deferred.
- R2. Mark plan **341** frontmatter `status: complete`.
- R3. `pr-merge-readiness.md`: scope includes KB **341**; add arc-complete note; local validation row for ref-search test counts.
- R4. Run and record: NcsConsti **74**, FindStrRef **18**, CLI ref-search **12**.
- R5. Plan index row **342**.

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FindStrRefCommandTests
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~InstallationRefSearchCommandCliTests
```

## Scope Boundaries

- Doc + verification only; do not merge PR #36 unless user explicitly requests.
