---
title: "docs: Sync onboarding tool chain post-027"
type: docs
status: completed
date: 2026-05-23
origin: PR #2 post-027 onboarding gap
---

# docs: Sync onboarding tool chain post-027

## Summary

Plan 027 added ConvertKotorGame to dotnet-desktop CI, but `QUICKSTART.md`, build ladder, and CI risk docs still omit it from the documented green tool chain.

---

## Requirements

- R1. `QUICKSTART.md` lists ConvertKotorGame in tool-chain summary and Run Tools commands.
- R2. `build-and-test-ladder.md` Step 5 / tool chain includes KotorCLI and ConvertKotorGame.
- R3. `ci-release-risks.md` documents dotnet-desktop Windows tool coverage (incl. ConvertKotorGame).
- R4. Drift register remediation **#19**.

---

## Scope Boundaries

- No code or workflow changes.
