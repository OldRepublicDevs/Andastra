---
title: "docs: Fix OdyPatch README naming and layout drift"
type: docs
status: completed
date: 2026-05-23
origin: plan 038 flagged TSLPatcher.Core and wrong paths in OdyPatch README
---

# docs: Fix OdyPatch README naming and layout drift

## Summary

`src/Tools/OdyPatch/README.md` is a stale Python-port template (Andastra title, `TSLPatcher.Core`, wrong build paths, missing `TESTING.md`). Replace with a concise repo-accurate README aligned with KB.

---

## Requirements

- R1. Rename framing to **OdyPatch**; map engine to `BioWare.TSLPatcher` under `src/BioWare/TSLPatcher/`.
- R2. Correct build/run commands (`net9.0`, `src/Tools/OdyPatch/`).
- R3. Link KB docs (`odypatch-installer-ux`, `tslpatcher-domain`, `NUGET.md`).
- R4. Remove broken references (`TESTING.md`, `cd Andastra`, `TSLPatcher.Core` sections).
- R5. Clear known drift note in `odypatch-installer-ux.md`; drift register **#30**.

---

## Scope Boundaries

- No code changes outside README.
- No full feature parity audit vs Python port.
