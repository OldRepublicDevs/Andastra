# Andastra Knowledgebase

Evidence-first documentation for the Andastra .NET game engine and tooling suite. This KB is the preferred onboarding path for agents and contributors.

## Relationship to Other Docs

| Location | Role |
|----------|------|
| `README.md` | Public project overview (architecture aligned with KB as of 2026-05-23) |
| `AGENTS.md` | Cursor Cloud build/test environment |
| `.cursorrules` | Highest-authority agent rules (engine fidelity, git, RE format) |
| `docs/knowledgebase/` (here) | Layered, labeled, auditable project truth |
| `wiki/` | File format byte-level specifications |
| `docs/*.md` | Investigation archives (verify before trusting) |

See [authority-map.md](authority-map.md) for conflict resolution.

## Taxonomy

| Layer | Path | Purpose |
|-------|------|---------|
| Intent | [00-intent/](../00-intent/) | Mission, engine scope, definition of done |
| Architecture | [10-architecture-runtime/](../10-architecture-runtime/) | Solution topology, runtime layers, BioWare boundary, tools |
| Domain | [20-domain-theory/](../20-domain-theory/) | Odyssey engine, formats, NCS/VM, RE methodology, TSLPatcher |
| Operational risk | [40-operational-risk/](../40-operational-risk/) | Build health, doc drift, CI risks, RE gaps, license |
| Execution | [50-execution/](../50-execution/) | Setup, build ladder, run paths, agent workflow |
| Meta | [90-meta/](.) | Evidence contract, authority, caveats (this layer) |

Layer `30-product-ux/` is deferred for content scope; tool chain, full solution net9.0 build, and OdyPatch NuGet pack are green as of 2026-05-23 (plans 033–035).

## CI validation (branch `docs/feat-knowledgebase-initial`)

See [pr-merge-readiness.md](pr-merge-readiness.md) for PR #2 merge checklist.

| Job | Status |
|-----|--------|
| CI — Test | Green (BioWare + Andastra.Tests) |
| CI — Lint | Green |
| CI — Solution Build (net9.0) | Green path; CI uses `-m:1` (plan 031) |
| Test Builds matrix | Green (net48-win, net9.0 linux/osx/win) |
| dotnet-desktop (Windows) | BioWare + OdyTools + OdyPatch + OdyPatch.UI + KotorCLI + ConvertKotorGame |
| OdyPatch NuGet pack | Green on Linux net9.0 via `helper_scripts/build-nuget.sh` (plan 035) |

Runtime game testing and OdyPatch mod-install UX remain manual. `[REPO]`

## Reading Order

### New agent (default)

1. [evidence-contract.md](evidence-contract.md)
2. [00-intent/project-mission.md](../00-intent/project-mission.md)
3. [50-execution/agent-workflow.md](../50-execution/agent-workflow.md)
4. [50-execution/build-and-test-ladder.md](../50-execution/build-and-test-ladder.md)
5. [20-domain-theory/reverse-engineering-methodology.md](../20-domain-theory/reverse-engineering-methodology.md)

### Runtime / engine contributor

1. [10-architecture-runtime/runtime-layering.md](../10-architecture-runtime/runtime-layering.md)
2. [10-architecture-runtime/game-vs-runtime-split.md](../10-architecture-runtime/game-vs-runtime-split.md)
3. [20-domain-theory/odyssey-engine-overview.md](../20-domain-theory/odyssey-engine-overview.md)
4. [40-operational-risk/re-fidelity-gaps.md](../40-operational-risk/re-fidelity-gaps.md)

### Format / BioWare library contributor

1. [10-architecture-runtime/bioware-library-boundary.md](../10-architecture-runtime/bioware-library-boundary.md)
2. [20-domain-theory/file-format-catalog.md](../20-domain-theory/file-format-catalog.md)
3. Link out to `wiki/` for byte layouts

### Modding / tools contributor

1. [10-architecture-runtime/tools-ecosystem.md](../10-architecture-runtime/tools-ecosystem.md)
2. [20-domain-theory/tslpatcher-domain.md](../20-domain-theory/tslpatcher-domain.md)
3. [50-execution/run-tools-reference.md](../50-execution/run-tools-reference.md)

## Document Index

### 00-intent

- [project-mission.md](../00-intent/project-mission.md)
- [engine-family-scope.md](../00-intent/engine-family-scope.md)
- [definition-of-done.md](../00-intent/definition-of-done.md)

### 10-architecture-runtime

- [solution-topology.md](../10-architecture-runtime/solution-topology.md)
- [runtime-layering.md](../10-architecture-runtime/runtime-layering.md)
- [game-vs-runtime-split.md](../10-architecture-runtime/game-vs-runtime-split.md)
- [bioware-library-boundary.md](../10-architecture-runtime/bioware-library-boundary.md)
- [tools-ecosystem.md](../10-architecture-runtime/tools-ecosystem.md)

### 20-domain-theory

- [odyssey-engine-overview.md](../20-domain-theory/odyssey-engine-overview.md)
- [resource-precedence-chain.md](../20-domain-theory/resource-precedence-chain.md)
- [file-format-catalog.md](../20-domain-theory/file-format-catalog.md)
- [ncs-nwscript-vm.md](../20-domain-theory/ncs-nwscript-vm.md)
- [reverse-engineering-methodology.md](../20-domain-theory/reverse-engineering-methodology.md)
- [tslpatcher-domain.md](../20-domain-theory/tslpatcher-domain.md)

### 40-operational-risk

- [build-health-matrix.md](../40-operational-risk/build-health-matrix.md)
- [documentation-drift-register.md](../40-operational-risk/documentation-drift-register.md)
- [ci-release-risks.md](../40-operational-risk/ci-release-risks.md)
- [re-fidelity-gaps.md](../40-operational-risk/re-fidelity-gaps.md)
- [license-and-compliance.md](../40-operational-risk/license-and-compliance.md)

### 50-execution

- [dev-environment-setup.md](../50-execution/dev-environment-setup.md)
- [build-and-test-ladder.md](../50-execution/build-and-test-ladder.md)
- [run-game-runtime.md](../50-execution/run-game-runtime.md)
- [run-tools-reference.md](../50-execution/run-tools-reference.md)
- [agent-workflow.md](../50-execution/agent-workflow.md)
- [contributing-paths.md](../50-execution/contributing-paths.md)

### 90-meta

- [evidence-contract.md](evidence-contract.md)
- [authority-map.md](authority-map.md)
- [caveat-register.md](caveat-register.md)
- [pr-merge-readiness.md](pr-merge-readiness.md)

## Repo Implications

- Update this index when adding or removing KB docs.
- Game-behavior discoveries that affect KotOR I/II may also warrant `wiki/` updates per `.cursorrules`.
- KB changes use `docs:` conventional commits, one file or tight group per commit.
