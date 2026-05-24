# Product UX Layer

User-facing tool workflows — installer UX, editor UX, and mod-author surfaces. **Stub layer** seeded 2026-05-23 (plan 038); deep product research deferred.

## Purpose

Bridge between architecture/tools docs (`10-architecture-runtime/`) and operational runbooks (`50-execution/`). Captures what users and mod authors experience when running OdyPatch and OdyTools, with explicit `[OPEN]` boundaries where runtime verification is missing.

## Documents

| Doc | Surface |
|-----|---------|
| [odypatch-installer-ux.md](odypatch-installer-ux.md) | Mod install / patch application |
| [odytools-editor-ux.md](odytools-editor-ux.md) | Holocron-style content editors |

## Authority

- Compile/run paths: [run-tools-reference.md](../50-execution/run-tools-reference.md) `[REPO]`
- Patch semantics: [tslpatcher-domain.md](../20-domain-theory/tslpatcher-domain.md) `[REPO]`
- Tool matrix: [tools-ecosystem.md](../10-architecture-runtime/tools-ecosystem.md) `[REPO]`
- OdyPatch host README: `src/Tools/OdyPatch/README.md` `[REPO]`
- OdyPatch.UI library README: `src/Tools/OdyPatch.UI/README.md` `[REPO]`

## Repo implications

- Expand this layer after manual K1/TSL install validation or dedicated UX research pass.
- Do not cite vendor PyKotor/HoloPatcher wiki as Andastra UX truth.
