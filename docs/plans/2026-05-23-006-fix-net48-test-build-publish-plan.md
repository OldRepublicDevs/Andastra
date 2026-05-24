---
title: "fix: net48 test-build publish flags"
type: fix
status: completed
date: 2026-05-23
origin: PR #2 Test Build net48-win-x64 NETSDK1125
---

# fix: net48 test-build publish flags

## Summary

Fix `test-builds.yml` net48 matrix job failing with NETSDK1125 — single-file publish is not supported for net48.

---

## Problem Frame

`Test Build - net48-win-x64` fails: `Publishing to a single-file is only supported for netcoreapp target`. net9.0 matrix jobs pass.

---

## Requirements

- R1. Use single-file publish properties only for net9.0+ in test-builds
- R2. net48 uses standard `dotnet publish` without PublishSingleFile
- R3. Verify step unchanged (nsscomp.exe)

---

## Implementation Units

- U1. Conditional publish flags in `test-builds.yml`

**Verification:** net48-win-x64 Test Build passes on next CI run.

---

## Sources & References

- GitHub run 26349393351 job 77564870915
