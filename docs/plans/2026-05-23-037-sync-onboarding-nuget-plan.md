---
title: "docs: Sync onboarding docs with NuGet pack toolchain"
type: docs
status: completed
date: 2026-05-23
origin: plans 033-035 landed NuGet fixes; AGENTS/QUICKSTART omit pack path
---

# docs: Sync onboarding docs with NuGet pack toolchain

## Summary

NuGet pack/push is green (plan 035) but `AGENTS.md`, `QUICKSTART.md`, and `dev-environment-setup.md` do not mention `helper_scripts/build-nuget.sh` or `docs/NUGET.md`. Also fix `pr-merge-readiness.md` to include plan 036.

---

## Requirements

- R1. Add NuGet pack/publish pointers to `AGENTS.md` Running tools section.
- R2. Add optional NuGet pack row to `QUICKSTART.md` Next Steps / tools section.
- R3. Add NuGet subsection to `dev-environment-setup.md`.
- R4. Update `pr-merge-readiness.md` plans table through 036.
- R5. Drift register remediation **#28**.

---

## Scope Boundaries

- No csproj or CI changes.
- No `30-product-ux/` layer.
