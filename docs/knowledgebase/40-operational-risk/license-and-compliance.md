# License and Compliance

AGPLv3 implications for Andastra development and distribution.

## License

Andastra is licensed under **GNU Affero General Public License v3.0**. `[REPO]` (`LICENSE`, `README.md`)

## Key Considerations

| Topic | Guidance | Label |
|-------|----------|-------|
| Source distribution | AGPL requires offering corresponding source when distributing binaries | [REPO] |
| Network use | AGPL network clause may apply to SaaS offering modified engine | [OFFICIAL] AGPLv3 intent |
| Game assets | KOTOR/TSL assets are copyrighted separately — users must own installs | [SYNTH] |
| Tool + engine bundles | Combining OdyPatch with engine runtime affects derivative work packaging | [OPEN] |

**`[OPEN]`** This KB does not provide legal advice. Commercial or store distribution needs proper legal review.

## Third-Party Dependencies

MonoGame, Stride, Avalonia, Newtonsoft.Json, etc. — each carries its own license. Check NuGet metadata and `THIRD-PARTY-NOTICES` if present. `[REPO]`

## Repo Implications

- Public API surface expansions may affect downstream AGPL obligations.
- Do not redistribute game BIFs/ERFs/chitin contents — tools operate on user-owned installs.
- Release workflows should include license file in artifacts per `docs/WORKFLOWS.md` practices.
