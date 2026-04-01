# NWScript TSL-Exclusive Commands

TSL adds 11 NWScript commands that have no K1 equivalent. All are handled by `CSWVirtualMachineCommands` in `swkotor2.exe`.

Reference: `/TSL_GOG_swkotor2` (reverse-engineered via agdec-mcp).

---

## Influence System (opcodes 0x31B, 0x31C, 0x31D)

All three influence commands are implemented in a single function at `/TSL_GOG_swkotor2 @ 0x0079C800`.

### GetInfluence (opcode 0x31B)

```nss
int GetInfluence(int nNPC)
```

Reads the influence value (0–100) for the specified party NPC index from `CSWPartyTable`. Returns the raw stored value; uninitialized slots return −1.

### SetInfluence (opcode 0x31C)

```nss
void SetInfluence(int nNPC, int nInfluence)
```

Sets influence for party NPC `nNPC` to `nInfluence`. Clamped to 0–100. Side effects:

- Calls `CGuiInGame::UpdateStatus(this, 9)` when influence increases (GUI gain feedback).
- Calls `CGuiInGame::UpdateStatus(this, 10)` when influence decreases (GUI loss feedback).
- Calls `CSWSCreature::HandleAlignmentInfluence()` if the NPC is currently in the active party roster.

### AdjustInfluence (opcode 0x31D)

```nss
void AdjustInfluence(int nNPC, int nAmount, object oTarget)
```

Adjusts influence by `nAmount`. If the stored value is −1 (uninitialized), it is first initialized to 50 before applying the delta.

---

## HandleAlignmentInfluence — Kreia Mechanic

`CSWSCreature::HandleAlignmentInfluence` at `/TSL_GOG_swkotor2 @ 0x00683A70` is called by `SetInfluence` when the influenced creature is active in the party.

### Non-Kreia path

For all NPCs other than Kreia: reads the alignment delta from the creature structure at `creature+0x1198+0x18C`, clamps to 0–100, and writes directly to `creature+0x1198+0x18A`.

### Kreia path — gray Jedi alignment mirror

When the influenced NPC has the tag `"kreia"`:

1. Looks up the NPC's entry in the influences 2DA (`rules+0x130+0x138`).
2. Reads `G_PC_Align_Val` — the global variable tracking the player character's alignment.
3. Checks whether the PC has feat `0x95` (Jedi Master) or `0xA7` (Sith Lord) and computes a class-level alignment multiplier:

   | Class level | Multiplier |
   |-------------|-----------|
   | 1–6         | ×1.2      |
   | 7–10        | ×1.4      |
   | 11–14       | ×1.6      |
   | 15–18       | ×1.8      |
   | 19+         | ×2.0      |

4. Resolves the NPC tag to a party-table index (hard-coded):

   | Index | Tag         |
   |-------|-------------|
   | 0     | atton       |
   | 1     | baodur      |
   | 4     | handmaiden  |
   | 6     | kreia       |
   | 9     | visasmarr   |
   | 10    | hanharr     |
   | 11    | disciple    |

5. Reads current influence for `nNPC`; if −1, reads `BaseInfluence` from the 2DA.

6. Applies the alignment formula:

```
new_influence = clamp(current_influence + delta, 0, 100)
factor        = ((new_influence − 50) × class_multiplier) / 50.0   // range −M..+M
pc_factor     = (PC_alignment − 50) / 50.0                          // range −1..+1
combined      = pc_factor × factor

if combined < 0:
    new_alignment = (1 + combined) × PC_alignment       // pushed toward opposite
else:
    new_alignment = (100 − PC_alignment) × combined + PC_alignment  // reinforced
```

7. Writes clamped alignment back to `creature+0x1198+0x18A` and calls `UpdatePureGoodEvilPowers()`.

**Design intent:** The formula implements Kreia's philosophy of balance. Positive influence on a light-side PC reinforces light-side alignment, but negative influence or misalignment pushes the PC toward the opposite extreme. The class-level multiplier means high-level Jedi/Sith Masters are affected more strongly.

---

## Other TSL-Exclusive Commands

| Function | TSL Address | Description |
|----------|------------|-------------|
| `ExecuteCommandForceHeartbeat` | `0x0078EDF0` | `ForceHeartbeat(object)` — triggers immediate heartbeat on target |
| `ExecuteCommandAdjustCreatureAttributes` | `0x0079C250` | Directly adjusts ability scores (STR/DEX/CON/INT/WIS/CHA) via NWScript |
| `ExecuteCommandAdjustCreatureSkills` | `0x0079C4B0` | Directly adjusts skill ranks (Computer/Demolitions/Stealth/etc.) via NWScript |
| `ExecuteCommandModifyBaseSavingThrow` | `0x0079C5C0` | Modifies base Fortitude/Reflex/Will saving throws |
| `ExecuteCommandInfluence` | `0x0079C800` | Influence opcodes 0x31B/0x31C/0x31D (see above) |
| `ExecuteCommandGrantAbility` | `0x0079DB00` | Grants a feat or force power to a creature at runtime |
| `ExecuteCommandIsStealthed` | `0x007A1340` | `GetIsStealthed(object)` — returns TRUE if creature is in stealth mode |
| `ExecuteCommandShowPartySelection` | `0x007A8330` | `ShowPartySelectionGUI(string, int, int)` — shows party swap overlay |
| `ExecuteCommandGetScriptParameter` | `0x007AF2F0` | `GetScriptParameter(int nParam)` — reads one of 5 integer script params (1–5) |
| `ExecuteCommandSetBonusForcePoints` | `0x007AF710` | Sets bonus Force Points added on top of creature's base max FP |
| `ExecuteCommandIsRunning` | `0x007B06B0` | `GetIsRunning(object)` — returns TRUE if creature is running |

---

## GetScriptParameter Detail

`ExecuteCommandGetScriptParameter` at `/TSL_GOG_swkotor2 @ 0x007AF2F0`:

```c
int GetScriptParameter(int nParam)  // nParam: 1–5
{
    // Script parameters stored at VirtualMachine[index-1]
    // VM base: 0x00A11C08; offsets +0x00..+0x10 (5 × DWORD)
    if (nParam < 1 || nParam > 5) return 0;
    return VirtualMachine[nParam - 1];
}
```

TSL allows scripts to receive up to 5 integer parameters. These are placed at `VM @ 0x00A11C08` before the script runs and are readable with `GetScriptParameter(1)` through `GetScriptParameter(5)`.

---

## NWScript VM Function Coverage

| Metric | Value |
|--------|-------|
| K1 `CSWVirtualMachineCommands` total | 577 |
| TSL `CSWVirtualMachineCommands` total | 34 named + ~540 unnamed (FUN_xxx) |
| K1↔TSL confirmed pairs | 3,625 (includes cascade-matched) |
| TSL-exclusive commands | 11 |
| K1-only commands (no TSL match found) | 27 (mostly minigame/platform-specific) |

K1-only commands with no TSL counterpart include: `ExecuteCommandYavinHackCloseDoor`, `ExecuteCommandShipBuild`, `ExecuteCommandGetButtonMashCheck`, `ExecuteCommandDoSinglePlayerAutoSave`, `ExecuteCommandGetIsPlayableRacialType`, and swoop minigame commands that differ between games.
