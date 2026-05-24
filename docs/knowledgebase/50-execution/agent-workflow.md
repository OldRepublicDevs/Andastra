# Agent Workflow

Mandatory practices for AI agents working in Andastra.

## Priority Order

1. **Engine fidelity** (K1 + TSL dual-binary RE)
2. **Repository integrity** (git discipline, no wildcard adds)
3. **Type safety** (C# 7.3 where pinned, clean builds)
4. **Conflict avoidance** in multi-agent workflows

`[REPO]` (`.cursorrules`)

## Engine Tasks

1. Confirm both binaries in AgentDecompile
2. Decompile K1 and TSL; compare inline
3. Use unified reference comment format
4. End response with AgentDecompile status suffix

See [reverse-engineering-methodology.md](../20-domain-theory/reverse-engineering-methodology.md).

## Git Discipline

```bash
# Unix — one file or tight group per commit
git add path/to/file1 path/to/file2 && git commit -m "type(scope): message"
```

- Never `git add .` or `git add -A`
- Conventional commits only
- Let pre-commit hooks complete

User rules: **only commit when user explicitly asks** — otherwise propose commands in `Proposed Git Commands` block.

## Code Standards

- Typed TODOs for incomplete work (`STUB`, `PLACEHOLDER`, `FIXME`, `SIMPLIFIED`, `HACK`)
- Prefer `if (GameType == K1) … else if (K2)` over new inheritance
- Avalonia: paired `.axaml` + `.axaml.cs`
- NSS/NCS: `helper_scripts/NcsTool.ps1` only

## Validation Before Done

1. Run narrowest build/test ladder — [build-and-test-ladder.md](build-and-test-ladder.md)
2. Fix compiler/analyzer errors in touched projects
3. Mark implementation status in response
4. Tooling/doc slices: check [pr-merge-readiness.md](../90-meta/pr-merge-readiness.md) for PR #2 scope

Optional when touching OdyPatch packaging: `./helper_scripts/build-nuget.sh` (Linux net9.0). `[REPO]`

## OdyPatch tooling

- **Run GUI/CLI:** `dotnet run --project src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0` — do **not** `dotnet run` `OdyPatch.UI` (library csproj). `[REPO]` (plans 044–045)
- **Patch semantics:** `BioWare.TSLPatcher` in `src/BioWare/TSLPatcher/`
- **UX boundaries:** [odypatch-installer-ux.md](../30-product-ux/odypatch-installer-ux.md)
- **READMEs:** `src/Tools/OdyPatch/README.md`, `src/Tools/OdyPatch.UI/README.md`

## KB Entry Points

1. [90-meta/README.md](../90-meta/README.md) — taxonomy and reading order
2. [evidence-contract.md](../90-meta/evidence-contract.md) — label rules
3. [build-and-test-ladder.md](build-and-test-ladder.md) — validation ladder
4. Mod/tools UX stubs: [30-product-ux/](../30-product-ux/README.md)

## Tooling Failures

If AgentDecompile or Roslyn MCP unavailable — **stop and report** per `.cursorrules` §9–§10.

## Repo Implications

- Multiple agents editing same files require per-file commits to reduce merge pain.
- KB updates for architecture/ops changes keep institutional memory out of chat-only context.
- OdyPatch-only policy — never add HoloPatcher to solution or docs.
