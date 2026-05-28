---
title: "feat: ReferenceCache NCS StrRef scan enablement"
type: feat
status: complete
date: 2026-05-28
completed: 2026-05-28
origin: docs/plans/2026-05-24-063-feat-pykotor-holocron-port-continuation-plan.md (deferred NCS ReferenceCache)
branch: feat/holocron-port-phase-b
---

# feat: ReferenceCache NCS StrRef scan enablement (plan 286)

## Summary

Close the remaining **ReferenceCache NCS StrRef** gap from plan **063** / **068**: `IncludeNcsStrRefScan` gates NCS indexing in `BuildStrRefReferenceCache` and batch `FindAllStrRefReferences` conversion (parity with `FindStrRefReferences` slow/cache paths). BioWare + KotorCLI tests prove override NCS hits are omitted when the flag is false.

## Requirements

- R1. `BuildStrRefReferenceCache` skips `.ncs` resources when `ReferenceSearchOptions.IncludeNcsStrRefScan` is false.
- R2. `FindAllStrRefReferences` omits NCS offset locations and NCS resources when `IncludeNcsStrRefScan` is false; passes `options` into resource enumeration for scope parity.
- R3. BioWare tests: NCS StrRef found by default; excluded with `IncludeNcsStrRefScan = false`.
- R4. KotorCLI subprocess tests: `find-strref` NCS hit without `--no-ncs`; miss with `--no-ncs` on NCS-only fixture.
- R5. Update plan **063** / **068** deferred notes — NCS StrRef via `ReferenceCache` is landed.

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~Cli_FindStrRef_Ncs"
```
