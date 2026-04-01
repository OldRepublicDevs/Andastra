# CSWSCreature and CSWSCreatureStats Struct Layout

Reverse-engineered offsets for `CSWSCreature` and `CSWSCreatureStats` objects in KotOR I and TSL.

Key entry point: `CSWSCreature::HandleAlignmentInfluence`  
Reference: `/K1_GOG_full @ TODO`, `/TSL_GOG_swkotor2 @ 0x00683A70`

---

## CSWSCreature

`CSWSCreature` is the server-side creature object. The `param_1` pointer passed to most `CSWSCreature::` methods points to this structure.

| Offset | Size | Type     | Field                  | Notes |
|--------|------|----------|------------------------|-------|
| +0x1198 | 4   | ptr      | `pStats`               | Pointer to `CSWSCreatureStats`. Accessed in `HandleAlignmentInfluence @ 0x00683A70`. |

<!-- TODO: Add full CSWSCreature header offsets once additional functions are decompiled. -->
<!-- Reference: /TSL_GOG_swkotor2 @ 0x00683A70 -->

---

## CSWSCreatureStats

`CSWSCreatureStats` holds all statistical properties for a creature (attributes, skills, saving throws, classes, alignment, etc.). The pointer lives at `CSWSCreature + 0x1198`.

| Offset | Size | Type     | Field                  | Notes |
|--------|------|----------|------------------------|-------|
| +0xAB  | 1    | byte     | `nClassType[0]`        | Base class slot 0 type (DnD class index). Part of class array `+0xAB + idx * 0x28`. |
| +0xD3  | 1    | byte     | `nClassType[1]`        | Class slot 1 type. |
| +0xFB  | 1    | byte     | `nClassType[2]`        | Class slot 2 type. |
| +0xF8  | ?    | ?        | TODO: class multiplier field | Used in alignment multiplier calculation — exact type TBD. |
| +0xFD  | ?    | byte/short | `nClassCount`        | Number of active class entries. Derived from `0x18A - 0x8D`. TODO: Verify. |
| +0x12E | 2    | short    | `nForcePoints`         | Current force points. GFF field "ForcePoints" (short). Confirmed via TSL `SaveStats @ 0x006F0190`. K1 address TODO. |
| +0x130 | 2    | short    | `nForceBaseline`       | Baseline/max force points. Confirmed adjacent to ForcePoints in TSL `SaveStats`. K1 address TODO. |
| +0x18A | 2    | short    | `nGoodEvil`            | Alignment: 0 = pure evil, 50 = neutral, 100 = pure good. GFF field "GoodEvil" (BYTE). Confirmed via `SaveStats`/`ReadStatsFromGff` in both games. |
| +0x18C | 1    | byte (signed) | `nGoodEvilDelta`  | Pending alignment change applied during `HandleAlignmentInfluence`. |
| +0x???  | 1    | char (signed) | `nBaseNPCAlignment` | **TSL-only.** GFF field "BaseCNPCAlignment" (CHAR). Defaults to `good_evil` when absent. Clamped to [-1, 100]. Confirmed via TSL `ReadStatsFromGff @ 0x006EC350` and `SaveStats @ 0x006F0190`. Offset within struct TBD — TODO: determine via analyze-data-flow. |

### Class Array Layout

Classes are stored as an array starting at `CSWSCreatureStats + 0xAB`, stride `0x28` bytes per entry:

```
base = pStats + 0xAB
class_entry[i] = base + i * 0x28

class_entry fields (offsets within entry):
  +0x00  byte   nClassType    (D&D/SW class ID)
  +0x01  byte   nClassLevel   (levels in this class)
  // remainder TBD
```

### Class Type Constants (K1 / TSL)

| ID | Class                |
|----|----------------------|
| 0  | Soldier              |
| 1  | Scout                |
| 2  | Scoundrel            |
| 3  | Jedi Guardian        |
| 4  | Jedi Consular        |
| 5  | Jedi Sentinel        |
| 6  | Combat Droid         |
| 7  | Expert Droid         |
| 8  | Minion               |
| 9  | TSL: Jedi Weapon Master |
| 10 | TSL: Jedi Master     |
| 11 | TSL: Sith Marauder   |
| 12 | TSL: Sith Lord       |
| 13 | TSL: Jedi Watchman   |
| 14 | TSL: Sith Assassin   |

Feat IDs referenced in `HandleAlignmentInfluence`:
- Feat `0x95` = Jedi Master (TSL, triggers ×2.0 alignment multiplier at lvl ≥ 19)
- Feat `0xA7` = Sith Lord (TSL, same)

---

## Alignment Multiplier Logic

From `CSWSCreature::HandleAlignmentInfluence @ TSL 0x00683A70`:

```c
// Read class level from CSWSCreatureStats
int class_level = pStats[0xAB + class_idx * 0x28 + 1];  // nClassLevel at +1

float class_multiplier;
if      (class_level >= 19) class_multiplier = 2.0f;
else if (class_level >= 15) class_multiplier = 1.8f;
else if (class_level >= 11) class_multiplier = 1.6f;
else if (class_level >= 7)  class_multiplier = 1.4f;
else                        class_multiplier = 1.2f;

// Full formula:
float factor    = ((new_influence - 50) * class_multiplier) / 50.0f;
float pc_factor = (PC_alignment - 50) / 50.0f;
float combined  = pc_factor * factor;

short new_align;
if (combined < 0.0f)
    new_align = (short)((1.0f + combined) * (float)PC_alignment);
else
    new_align = (short)((100.0f - (float)PC_alignment) * combined + (float)PC_alignment);

new_align = clamp(new_align, 0, 100);
pStats[0x18A] = new_align;
```

TSL-exclusive — no K1 equivalent.

---

## Hard-Coded NPC Party Indices

Used by `CSWSCreature::HandleAlignmentInfluence` to resolve NPC tags → party table slots:

| Index | Tag         |
|-------|-------------|
| 0     | `atton`     |
| 1     | `baodur`    |
| 4     | `handmaiden`|
| 6     | `kreia`     |
| 9     | `visasmarr` |
| 10    | `hanharr`   |
| 11    | `disciple`  |

---

## To-Do

- [ ] `TODO: Find CSWSCreature::GetGoodEvil @ K1` and `@ TSL` — verify alignment read path
- [ ] `TODO: Find CSWSCreature::SetAlignment` in both games — verify write path confirms `+0x1198+0x18A`
- [ ] `TODO: Find CSWSCreature::GetGoodEvil @ K1` and `@ TSL` — verify alignment read path
- [ ] `TODO: Find CSWSCreature::SetAlignment` in both games — verify write path confirms `+0x1198+0x18A`
- [ ] `TODO: Determine nBaseNPCAlignment offset` in CSWSCreatureStats — run analyze-data-flow on `TSL ReadStatsFromGff @ 0x006EC350`
- [ ] `TODO: Map remaining CSWSCreatureStats fields` (HP, base abilities, skills, feats bitfield)
- [ ] `TODO: Verify nClassCount offset (+0xFD)` with CSWSCreature::GetNumClasses decompilation
- [ ] `TODO: Map CSWSCreature vtable @ K1 + TSL` — many virtual methods untested
- [ ] `TODO: Confirm nForceBaseline field name` — verify +0x130 is max/base force, not some other force-related short
- [ ] `TODO: Find CSWSCreatureStats::SaveStats @ K1` — verify K1 also writes ForcePoints from pStats+0x12E

## Confirmed Serializer Paths

| Function | K1 | TSL | Notes |
|----------|----|-----|---------|
| `CSWSCreatureStats::SaveStats`      | `0x005B1B90` | `0x006F0190` | Writes GoodEvil, ForcePoints; TSL also writes BaseCNPCAlignment |
| `CSWSCreatureStats::ReadStatsFromGff` | `0x005AFCE0` | `0x006EC350` | Reads GoodEvil (clamp 0–100); TSL also reads BaseCNPCAlignment (default=good_evil, clamp -1–100) |

TSL `ReadStatsFromGff @ 0x006EC350` key excerpt:
```c
this->good_evil = clamp(ReadFieldBYTE("GoodEvil", prev=this->good_evil), 0, 100);
char bna = ReadFieldCHAR("BaseCNPCAlignment", default=-1);
if (bna == -1) bna = (char)this->good_evil;
this->base_npc_alignment = clamp(bna, -1, 100);  // -1 = no override
```

TSL `SaveStats @ 0x006F0190` key excerpt:
```c
WriteFieldBYTE("GoodEvil", this->good_evil);
WriteFieldCHAR("BaseCNPCAlignment", this->base_npc_alignment);
// pStats = *(CSWSCreature + 0x1198)
WriteFieldSHORT("ForcePoints", *(short*)(pStats + 0x12E));
```

AgentDecompile status: Partially completed - Missing K1 address for `CSWSCreature::HandleAlignmentInfluence` and `nBaseNPCAlignment` struct offset, all CSWSCreature vtable offsets. TODO find them :(

<!-- Reference: /TSL_GOG_swkotor2 @ 0x00683A70, /K1_GOG_full @ TODO -->
