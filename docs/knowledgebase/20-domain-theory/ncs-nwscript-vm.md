# NCS and NWScript VM

Script compilation, bytecode, and in-game execution.

## Runtime VM

- NCS virtual machine and NWScript engine API surface live under **`src/Andastra/Game/Scripting/`**. `[REPO]`
- README describes "Runtime.Scripting" — actual code is in Game project (see [game-vs-runtime-split.md](../10-architecture-runtime/game-vs-runtime-split.md)).

## Toolchain

| Component | Path | Role |
|-----------|------|------|
| **NSSComp** | `src/Tools/NSSComp/` | Compile `.nss` → `.ncs` (K1/TSL via BioWare) |
| **NCSDecomp.CLI** | `src/Tools/NCSDecomp.CLI/` | Decompile `.ncs` → `.nss` |
| **NcsTool.ps1** | `helper_scripts/NcsTool.ps1` | Agent-mandated compile/decompile/compare/roundtrip/generate-defs |

Agent rules reference `scripts/NcsTool.ps1` but file is under `helper_scripts/` — path drift C11 in caveat register. `[REPO]`

## Deep References

- `docs/NSS_GRAMMAR_REFERENCE.md` — grammar and language surface
- `wiki/NCS-File-Format.md` — bytecode format
- `wiki/NWScript-TSL-Exclusive-Commands.md` — TSL-only commands

## Tests

- `tests/Andastra.Tests/` — NCS compile roundtrip covering K1 and TSL. `[REPO]`
- BioWare NCS parser tests under `tests/BioWare.Tests/` as applicable

## K1 vs TSL

NWScript differs between games (constants, TSL-only functions). Compiler and VM must branch on `BioWareGame` / game type. `[REPO]`/`[SYNTH]`

Use unified documentation with inline difference notes per `.cursorrules`.

## Repo Implications

- Bytecode or opcode changes require roundtrip tests in Andastra.Tests.
- Decompiler port status docs (`docs/NCSDecomp_*.md`) reference legacy `CSharpKOTOR` paths — verify against `src/BioWare/Resource/Formats/NCS/`.
- Script defs generation flows through NcsTool.ps1 per agent policy, not ad-hoc scripts.
