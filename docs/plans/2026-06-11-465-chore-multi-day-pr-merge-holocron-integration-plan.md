---
title: "chore: multi-day PR merge + Holocron integration"
type: chore
status: in_progress
date: 2026-06-11
origin: docs/plans/2026-06-10-464-chore-stack-simulation-arc-tracker-sync-v20-plan.md
branch: work/day1-stack-simulation-land
---

# chore: multi-day PR merge + Holocron integration (plan 465)

## Summary

Consolidate the NCS CONSTI stack-simulation arc (plans **421**–**463**, PRs **#89**–**#133**) onto `master` in one bulk land, close superseded open PRs, sync KB tracker rows, and prepare Holocron integration follow-ups.

## Day 1 — stack-simulation arc land (completed)

| Item | Status |
|------|--------|
| Merge plan-463 scanner + tests onto `master` | **Done** @ merge `3b060001a` |
| NcsConsti test count | **163** passing (`--filter FullyQualifiedName~NcsConsti`) |
| Preserve master-only four-hop mixed relay tests | **Done** (no duplicate removal) |
| KB union: field-value arc + stack-simulation merged note | **Done** |
| Close superseded PRs **#89**–**#133** | Pending post-merge PR close |
| Open PR to `master` + CI green + merge | In progress |

## Day 2+ (deferred)

- Holocron browser integration gate
- Field-value arc **#81**–**#86** sequential merge
- Relay arc **#77**–**#88** merge and rebase cleanup
- Plan **464** tracker sync v20 full KB refresh (build ladder Step 3b)

## Verification

- `dotnet build src/BioWare/BioWare.csproj --framework net9.0`
- `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti` — **≥163** pass
- `gh pr checks` green before merge
