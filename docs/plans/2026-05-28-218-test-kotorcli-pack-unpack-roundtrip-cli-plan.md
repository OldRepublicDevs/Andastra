---
title: "test: kotorcli pack unpack roundtrip cli subprocess"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-217-test-kotorcli-build-pipeline-full-chain-cli-plan.md
branch: feat/holocron-port-phase-b
---

# test: pack→unpack roundtrip CLI subprocess (plan 218)

## Summary

Mirror the two remaining `BuildPipelineIntegrationTests` pack/unpack orchestration cases through CLI subprocess.

## Requirements

- R1. `CliPack_Unpack_RemoveDeleted_RemovesStaleJson` — mirrors `Pack_Unpack_RemoveDeleted_RemovesStaleJsonNotInArchive`.
- R2. `CliPack_Unpack_Roundtrip_RestoresJsonUnderRules` — mirrors `Pack_Unpack_Roundtrip_WritesJsonUnderRulesPath`.
- R3. ResRefs ≤16 chars (`rm_creature`, `rt_creature`).
- R4. Extend `BuildPipelineCommandCliTests.cs` with `PackUnpackPipelineConfig`.
- R5. README **344** tests.

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0
```
