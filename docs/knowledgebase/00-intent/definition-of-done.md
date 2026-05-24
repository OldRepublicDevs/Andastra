# Definition of Done

Checklist for agents and contributors before considering work complete.

## Code Quality

- [ ] Affected projects compile cleanly. Prefer `dotnet build src/BioWare/BioWare.csproj --framework net9.0` for narrow checks; full solution may fail on KotorCLI or missing stubs. `[REPO]` (`AGENTS.md`)
- [ ] C# language version respects per-project pins (C# 7.3 where configured). No C# 8+ syntax in 7.3-targeted projects. `[REPO]` (`.cursorrules`)
- [ ] Public APIs have XML documentation where README contributing section applies. `[REPO]`
- [ ] Avalonia views use paired `.axaml` + `.axaml.cs` files. `[REPO]` (`.cursorrules`)

## Engine Fidelity (K1/TSL behavior changes)

- [ ] Both `/K1_swkotor` and `/TSL_swkotor2` analyzed via AgentDecompile when RE tooling available. `[REPO]`
- [ ] Unified behavior description with inline K1/TSL differences. `[REPO]`
- [ ] Source comments use format: `// Reference: /K1_swkotor @ 0xADDR, /TSL_swkotor2 @ 0xADDR`. `[REPO]`
- [ ] Agent response includes AgentDecompile status suffix for engine tasks. `[REPO]`
- [ ] Relevant KotOR discoveries reflected in `wiki/` when appropriate. `[REPO]`

## Incomplete Work Policy

Any incomplete logic must use typed TODO markers: `[REPO]` (`.cursorrules`)

- `// TODO: STUB - ...`
- `// TODO: PLACEHOLDER - ...`
- `// TODO: FIXME - ...`
- `// TODO: SIMPLIFIED - ...`
- `// TODO: HACK - ...`

## Git Discipline (multi-agent)

- [ ] Never `git add .` or wildcard adds. `[REPO]`
- [ ] Chain `git add <explicit-files> && git commit -m "type(scope): message"`. `[REPO]`
- [ ] Conventional commit prefixes: `feat:`, `fix:`, `refactor:`, `docs:`, `chore:`, `test:`. `[REPO]`
- [ ] Pre-commit hooks run fully (no `--no-verify` unless user requests). `[REPO]`

## Testing

- [ ] Run relevant tests when behavior changes: `[REPO]` (`AGENTS.md`)
  - `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0`
  - `dotnet test tests/Andastra.Tests/Andastra.Tests.csproj --framework net9.0`
- [ ] Format changes: BioWare roundtrip tests in `tests/BioWare.Tests/Formats/`
- [ ] NCS changes: compile roundtrip in `tests/Andastra.Tests/`

## Documentation

- [ ] Public `.md` in `docs/` (except root README). `[REPO]`
- [ ] KB updates when architecture or operational truth changes. `[SYNTH]`
- [ ] Progress roadmaps in `.cursor/roadmaps/` per rules (directory may not exist yet). `[REPO]`

## Repo Implications

- Claiming "full solution builds" may be false while KotorCLI or missing solution stubs remain red — see [build-health-matrix.md](../40-operational-risk/build-health-matrix.md).
- Engine PRs without dual-binary references are incomplete per project rules even if tests pass.
- Commits are user-requested in Cursor user rules; agents propose chained commands after file changes.
