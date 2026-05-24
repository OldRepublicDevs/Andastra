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

1. Run narrowest build/test ladder
2. Fix compiler/analyzer errors in touched projects
3. Mark implementation status in response

## Tooling Failures

If AgentDecompile or Roslyn MCP unavailable — **stop and report** per `.cursorrules` §9–§10.

## Repo Implications

- Multiple agents editing same files require per-file commits to reduce merge pain.
- KB updates for architecture/ops changes keep institutional memory out of chat-only context.
- OdyPatch-only policy — never add HoloPatcher to solution or docs.
