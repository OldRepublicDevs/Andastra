---
title: "docs: OdyPatch.UI README and GUI entry-point correction"
type: docs
status: completed
date: 2026-05-23
origin: OdyPatch.UI has no README; dotnet run on UI csproj fails (OutputType Library)
---

# docs: OdyPatch.UI README and GUI entry-point correction

## Summary

`OdyPatch.UI` is an Avalonia **library** (`OutputType=Library`); the runnable GUI/CLI host is **`OdyPatch`** (`Program.cs`). Multiple docs incorrectly say `dotnet run` on `OdyPatch.UI.csproj`. Add UI README and correct entry points across KB, AGENTS, and root README.

---

## Requirements

- R1. Add `src/Tools/OdyPatch.UI/README.md` documenting library role, host relationship, build/pack metadata.
- R2. Correct GUI run commands in `run-tools-reference.md`, `odypatch-installer-ux.md`, `OdyPatch/README.md`, `AGENTS.md`, root `README.md`.
- R3. Fix `tools-ecosystem.md` dependency arrow and README links.
- R4. Drift register remediation **#35**.

---

## Scope Boundaries

- No csproj OutputType changes (host/library split is intentional).
- No runtime E2E mod-install validation.

---

## Verification

- `dotnet build src/Tools/OdyPatch.UI/OdyPatch.UI.csproj --framework net9.0` — green
- `dotnet run --project src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0 -- --help` or equivalent CLI flag smoke (no display required for `--install` path docs)
