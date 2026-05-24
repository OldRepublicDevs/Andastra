# Andastra AI Agent Rules (2026 Edition)

You are an AI agent contributing to Andastra, a .NET project for reverse-engineering and modding Knights of the Old Republic I (K1) and II (TSL). Your highest priority is **game engine fidelity, repository integrity, conflict avoidance in multi-agent workflows, type safety, and strict git discipline**. Follow every rule exactly.

## 1. MANDATORY: Core Game Engine Fidelity (Highest Priority Rule)

Treat K1 (`swkotor.exe`) and TSL (`swkotor2.exe`) as one engine with minor differences. For any change involving engine behavior, mechanics, file formats, resources, or reverse-engineered logic: analyze both binaries via agentdecompile and provide one unified description with inline differences.

Use these program paths whenever available:
- `/K1_swkotor`
- `/TSL_swkotor2`

### Prohibited
- Do not write K1-only or TSL-only sections for engine behavior unless explicitly requested.
- Do not provide only one address for shared behavior.
- Do not use single-game reference format.

### Mandatory workflow (every engine task)
1. Open and confirm both binaries in agentdecompile.
2. Locate and decompile in K1; capture address and core logic.
3. Locate and decompile in TSL; capture address and core logic.
4. Compare and document differences inline.
5. Use required address formatting or explicit TODO when unknown.
6. Write one unified behavior description.

### Address and reference format
- Function references: `FunctionName @ (/K1_swkotor @ 0xADDRESS, /TSL_swkotor2 @ 0xADDRESS)`
- If unknown: `TODO: <task>`
- Source comments: `// Reference: /K1_swkotor @ 0xADDRESS, /TSL_swkotor2 @ 0xADDRESS`

If you see incorrectly formatted agentdecompile comments, add a TODO marker so they can be grepped and fixed:
- `// TODO: Fix agentdecompile reference format - <details>`

### Mandatory response suffix for engine-related responses
Add exactly one line:
- `AgentDecompile status: Completed - Analyzed both K1 and TSL :)`
- `AgentDecompile status: Partially completed - Missing TSL address for <function>, TODO find it :(`
- `AgentDecompile status: Skipped - <exact reason> :(`

## 2. MANDATORY: Git Commit Discipline (High Priority - Non-Negotiable)

In multi-agent environments, conflicts arise quickly. To prevent them:
- **NEVER** use `git add .`, `git add -A`, or wildcard/all-files commands.
- **ALWAYS** add and commit one file (or a tightly related small group) at a time.
- **ALWAYS** chain `git add` and `git commit` on the same line.

### Exact command formats (must use one)
- Windows: `git add <file1> <file2>; git commit -m "type(scope): message"`
- Unix/Mac: `git add <file1> <file2> && git commit -m "type(scope): message"`

### Commit requirements
- List only explicit modified/created/deleted/moved files.
- Use conventional commits only: `feat:`, `fix:`, `refactor:`, `docs:`, `chore:`, `test:`.
- Keep commit messages concise, lowercase, and descriptive.
- Let pre-commit hooks run fully.
- Prefer 2-3 commands per clean commit.

### General git safety
- Append `--no-pager` to paging commands.
- Preserve working-tree changes.
- Snapshot before cleanup actions: `git stash push --include-untracked`.
- Get explicit user approval before destructive commands, quoting the exact command.

### Mandatory response behavior
- After any file change, end with a fenced `Proposed Git Commands` block containing exact chained command(s), then:
`Git commits: Issued per rules ✅`
- If no changes were made:
`Git commits: No changes made ✅`

## 3. Static Type and Build Safety

- Run project-appropriate checks for changed C#/.NET code before completion.
- Prefer these validations:
  - `dotnet build Andastra.sln`
  - `dotnet test Andastra.sln --verbosity normal` (when tests are relevant)
- Fix all compiler and analyzer errors before proposing completion.

Suppressions are allowed only when justified:
- `#pragma warning disable` or suppressions for external/third-party constraints with clear rationale.
- Temporary suppressions require a TODO and follow-up plan.
- Avoid broad or blanket suppressions.

Favor explicit types and safe runtime checks:
- Prefer `is`/pattern checks over brittle reflection checks for normal control flow.

## 4. File Organization

Keep the root clean and follow repository structure conventions:
- Keep public docs in `docs/` (except root `README.md`).
- Keep scripts in `scripts/` or existing tooling folders already used by the repo.
- Keep runtime/game code under `src/Andastra/` and tools under `src/Tools/`.

Do not move files solely to satisfy generic rules when this repository already has established locations.

## 5. .NET and C# Conventions

- Follow each project file's configured target frameworks and language version.
- Most core projects pin C# to `7.3`; do not introduce C# 8+ features where unsupported.
- Keep nullable/implicit usings behavior aligned with each project.

Rewrite prohibited constructs in C# 7.3-targeted projects:
- Nullable reference syntax (`string?`) -> `string` plus nullability annotations where used.
- Using declarations -> explicit `using (...) { }` blocks.
- Switch expressions -> traditional switch statements.

## 6. Incomplete Code Policy (High Priority - Non-Negotiable)

Prefer fully complete, production-ready implementations. If any part is incomplete, mark it explicitly with `// TODO:` and type.

Required TODO types:
- `// TODO: STUB - ...`
- `// TODO: PLACEHOLDER - ...`
- `// TODO: FIXME - ...`
- `// TODO: SIMPLIFIED - ...`
- `// TODO: HACK - ...`

Never leave placeholder logic, empty bodies, or vague comments without TODO markers.

### Mandatory response behavior
- After any code change, end with one of:
  - `Implementation status: Complete - No incomplete code or TODOs added ✅`
  - List introduced TODOs, then:
    `Implementation status: Incomplete sections marked per rules ⚠️`

## 7. Architecture Preferences

- For new logic, prefer clear conditional branching over introducing new inheritance hierarchies.
- Preserve existing inheritance structures unless explicitly asked to refactor.

Example:
```csharp
if (GameType == GameType.K1) { /* K1 behavior */ }
else if (GameType == GameType.K2) { /* K2 behavior */ }
```

## 8. Avalonia UI Workflow

Use paired files for Avalonia views/dialogs/windows:
- `<name>.axaml`
- `<name>.axaml.cs`

Do not create standalone `<name>.cs` code-behind for an `.axaml` view. If encountered, fix it and ensure build success.

## 9. Reverse Engineering Workflow

Follow this sequence:
1. Analyze in Ghidra or agentdecompile first.
2. Document symbols (function/variable names, comments, prototypes).
3. In C#, add source reference comments using both games when applicable.
4. Verify parity with MCP tooling output.

If REVA/agentdecompile tooling is unavailable or fails, stop and report immediately.

## 10. Specialized Tooling Rules

- For NSS/NCS operations, use only `helper_scripts/NcsTool.ps1` for compile/decompile/compare/roundtrip/generate-defs.
- Use Roslyn/Navicode MCP capabilities whenever they can improve correctness or bulk-safe refactors.
- If Roslyn/Navicode MCP setup fails, stop and report immediately.

## 11. Documentation and Definition of Done

Documentation:
- Public-facing `.md` belongs in `docs/`.
- Progress roadmaps belong in `.cursor/roadmaps/`.
- Update `wiki/` when new KotOR I/II game behavior discoveries are relevant.

Definition of done checklist:
- Code compiles cleanly for affected projects.
- Changes respect project language/version constraints.
- Engine behavior changes are verified against K1 and TSL.
- Files are in repository-appropriate locations.
- Incomplete parts are explicitly marked with typed TODOs.
- Git commands are proposed in chained per-file commit format.
