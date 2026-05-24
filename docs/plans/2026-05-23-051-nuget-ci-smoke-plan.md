---
title: "ci: add odypatch nuget pack smoke job"
type: ci
status: completed
date: 2026-05-24
origin: pr-merge-readiness suggested slice 051+; nuget pack green locally but not in CI
---

# ci: add OdyPatch NuGet pack smoke job

## Summary

Add a lightweight CI job that runs `helper_scripts/build-nuget.sh` on ubuntu net9.0 to catch OdyPatch packaging regressions. Sync KB docs that currently note NuGet pack is local-only.

---

## Requirements

- R1. Add `nuget-pack-smoke` job to `.github/workflows/ci.yml` running `bash helper_scripts/build-nuget.sh` (no publish).
- R2. Assert `.nupkg` artifact exists after pack (script exit 0 + file present).
- R3. Update `ci-release-risks.md` and `build-health-matrix.md` with CI coverage note.
- R4. Update `pr-merge-readiness.md` local validation table and suggested next slices.
- R5. Drift remediation **#42**; plans index row **051**.

---

## Scope Boundaries

- No NuGet.org publish, no API keys in CI.
- No changes to `build-nuget.sh` unless CI requires a flag (prefer none).
- OdyPatch UX validation and bulk evidence-label pass remain deferred (051+).

---

## Test Scenarios

| Scenario | Expected |
|----------|----------|
| Local smoke | `bash helper_scripts/build-nuget.sh` exits 0 and creates `OdyPatch.*.nupkg` |
| CI job | New job passes on PR branch after workflow change |

---

## Repo Implications

- CI runtime increases ~1–2 min on ubuntu for OdyPatch+OdyTools build+pack.
- Failures indicate packaging/SPDX/csproj regressions before merge.
