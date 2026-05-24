---
title: "docs: Fix wiki OdyPatch links and sync AGENTS tool coverage"
type: docs
status: completed
date: 2026-05-23
origin: PR #2 drift remediation continuation
---

# docs: Fix wiki OdyPatch links and sync AGENTS tool coverage

## Summary

Top-level `wiki/` format pages still link to HoloPatcher mod-dev README stubs that do not exist in-repo. `AGENTS.md` omits KotorCLI, ConvertKotorGame, and full-solution build commands added in plans 018–021.

---

## Requirements

- R1. Replace HoloPatcher mod-dev links in `wiki/GFF-File-Format.md`, `wiki/2DA-File-Format.md`, `wiki/SSF-File-Format.md`, `wiki/TLK-File-Format.md` with OdyPatch README path (match `wiki/Home.md` pattern).
- R2. Add KotorCLI, ConvertKotorGame, and full solution build notes to `AGENTS.md` Running tools / Build sections.
- R3. Drift register remediation **#16**.

---

## Scope Boundaries

- Do not edit `vendor/` wiki corpus.
- No code or workflow changes.

## Test Scenarios

- Grep `wiki/*.md` (excluding vendor) — no HoloPatcher links in the four format files.
- AGENTS.md mentions KotorCLI `--help` and `dotnet build Andastra.sln --framework net9.0`.
