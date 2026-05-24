---
title: "fix: Include DialogHelper in OdyTools standalone editors"
type: fix
status: completed
date: 2026-05-23
origin: docs/knowledgebase/40-operational-risk/build-health-matrix.md
---

# fix: Include DialogHelper in OdyTools standalone editors

## Summary

Fix compile failures in slim OdyTools standalone editor projects by adding `Utils/DialogHelper.cs` to the shared standalone props import list, unblocking desktop CI annotations and aligning build-health claims with reality.

---

## Problem Frame

Standalone editor csprojs include `Editor.cs`, `WindowUtils.cs`, and dialog files that call `DialogHelper`, but none include `Utils/DialogHelper.cs`. CI annotations on `.NET Core Desktop / build (Release)` report `CS0103: The name 'DialogHelper' does not exist` across multiple standalone paths. `[REPO]`

---

## Requirements

- R1. `dotnet build src/Tools/OdyTools/Editors/OdyToolARE.Standalone.csproj --framework net9.0` succeeds.
- R2. At least one additional standalone editor (e.g. `OdyToolUTE.Standalone.csproj`) builds on net9.0.
- R3. Fix applies to all standalones via shared props — no per-csproj duplication.
- R4. Update `build-health-matrix.md` to reflect OdyTools/OdyPatch green status from plan 013.

---

## Scope Boundaries

- Do not refactor DialogHelper API or editor UI logic.
- Do not change AIO `OdyTools.csproj` (already green).

---

## Key Technical Decisions

- **Single props change**: Add `DialogHelper.cs` to `Standalone/OdyTools.Standalone.Editor.props` alongside other shared Utils includes — one edit covers 25+ standalones.

---

## Implementation Units

- U1. **Add DialogHelper to shared standalone props**

**Goal:** All standalones compile shared editor code that references DialogHelper.

**Requirements:** R1, R2, R3

**Dependencies:** None

**Files:**
- Modify: `src/Tools/OdyTools/Editors/Standalone/OdyTools.Standalone.Editor.props`

**Approach:**
- Add `<Compile Include="$(_OdyToolsRoot)\Utils\DialogHelper.cs" />` to the shared Utils ItemGroup.

**Test scenarios:**
- Test expectation: none — compile verification only.

**Verification:**
- ARE and UTE standalone csprojs build on net9.0.

---

- U2. **Refresh build health matrix**

**Goal:** KB reflects post-013 OdyTools/OdyPatch green path.

**Requirements:** R4

**Dependencies:** U1

**Files:**
- Modify: `docs/knowledgebase/40-operational-risk/build-health-matrix.md`

**Test scenarios:**
- Test expectation: none — documentation.

**Verification:**
- Matrix lists OdyTools AIO and OdyPatch as green; standalone editors note DialogHelper fix.

---

## Sources & References

- CI annotations: `.NET Core Desktop / build (Release)` job 77565654800
- Shared props: `src/Tools/OdyTools/Editors/Standalone/OdyTools.Standalone.Editor.props`
- Prior fix: `docs/plans/2026-05-23-013-fix-odytools-build-errors-plan.md`
