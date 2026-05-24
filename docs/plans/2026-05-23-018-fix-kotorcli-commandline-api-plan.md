---
title: "fix: KotorCLI System.CommandLine 2.0 Option API"
type: fix
status: completed
date: 2026-05-23
origin: docs/knowledgebase/40-operational-risk/build-health-matrix.md
---

# fix: KotorCLI System.CommandLine 2.0 Option API

## Summary

KotorCLI crashes on startup because `Option<T>` two-argument constructors pass description strings as aliases (System.CommandLine 2.0.2). Add a small factory helper and migrate all option declarations.

---

## Problem Frame

```
Unhandled exception. System.ArgumentException: Names and aliases cannot contain whitespace: "Show version information"
   at System.CommandLine.Option..ctor(String name, String[] aliases)
```

All `new Option<T>(name, description)` call sites in `src/Tools/KotorCLI/` are affected. `[REPO]`

---

## Requirements

- R1. `dotnet run --project src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0 -- --help` exits 0.
- R2. Centralize option creation via `Cli.Opt<T>(name, description)` helper.
- R3. Update build-health-matrix, run-tools-reference, tools-ecosystem, solution-topology, AGENTS.md.
- R4. Drift register remediation 9.

---

## Implementation Units

- U1. **Add Cli.cs helper** — `src/Tools/KotorCLI/Cli.cs`
- U2. **Migrate Option declarations** — all Commands/*.cs + Program.cs
- U3. **Verify CLI** — build + `--help`
- U4. **Update KB + AGENTS.md**

---

## Scope Boundaries

- Do not refactor command handlers or downgrade System.CommandLine package.
- Full solution Stride assembly-processor Linux failure is out of scope.
