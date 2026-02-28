# UTC Editor: Field Types and Ranges

This document records GFF field types and valid ranges for the Creature (UTC) format for use when setting spinbox min/max in the Holocron Toolset UTC editor and when validating mod files.

**Source:** Engine behavior and GFF type definitions (BYTE, SHORT, WORD, INT32, FLOAT, ResRef). Verified against reva (CSWSCreatureStats::ReadStatsFromGff: K1 @ 0x005afce0, TSL @ 0x006ec350). The UTC editor tooltips describe how each field is used in-game and how modders can change it.

## Hit Points and Force Points (SHORT)

All of the following are stored as signed 16-bit (SHORT). The engine does not use negative values for HP/FP in practice.

| GFF Field          | UI Control    | Type  | Min  | Max   | Default |
|--------------------|---------------|-------|------|------|--------|
| HitPoints          | Base HP       | SHORT | 0    | 32767| 0      |
| CurrentHitPoints   | Current HP    | SHORT | 0    | 32767| 0      |
| MaxHitPoints       | Max HP        | SHORT | 0    | 32767| 0      |
| ForcePoints        | Max FP        | SHORT | 0    | 32767| 0      |
| CurrentForce       | Current FP    | SHORT | 0    | 32767| 0      |

**In-game:** For NPCs, the engine may recompute current HP/FP from level and ability modifiers after loading. Current HP can temporarily exceed Max HP from buffs.

## Ability Scores (BYTE)

Str, Dex, Con, Int, Wis, Cha are stored as BYTE (0–255). Modifiers are derived as (ability - 10) / 2.

| GFF Field | UI Control  | Type | Min | Max | Default |
|-----------|-------------|------|-----|-----|--------|
| Str       | Strength    | BYTE | 0   | 255 | 0      |
| Dex       | Dexterity   | BYTE | 0   | 255 | 0      |
| Con       | Constitution| BYTE | 0   | 255 | 0      |
| Int       | Intelligence| BYTE | 0   | 255 | 0      |
| Wis       | Wisdom      | BYTE | 0   | 255 | 0      |
| Cha       | Charisma    | BYTE | 0   | 255 | 0      |

**Typical modder range:** 8–20 for normal creatures.

## Save bonuses (SHORT, effective -128–127)

Written by the engine as SHORT with a signed-byte value; effective range -128–127.

| GFF Field   | UI Control   | Type  | Min  | Max   |
|------------|--------------|-------|-----|-------|
| fortbonus  | Fortitude    | SHORT | -128 | 127   |
| refbonus   | Reflex       | SHORT | -128 | 127   |
| willbonus  | Will         | SHORT | -128 | 127   |

## Natural AC and Challenge Rating

| GFF Field        | UI Control       | Type  | Min | Max  |
|------------------|------------------|-------|-----|------|
| NaturalAC        | Armor Class      | BYTE  | 0   | 255  |
| ChallengeRating  | Challenge Rating | FLOAT | 0   | 100  |

## Skill ranks (SkillList Rank)

Each skill in the SkillList list has a `Rank` field (BYTE). Range 0–255.

| UI Control       | GFF              |
|------------------|------------------|
| Computer Use     | SkillList Rank   |
| Demolitions      | SkillList Rank   |
| Stealth          | SkillList Rank   |
| Awareness        | SkillList Rank   |
| Persuade         | SkillList Rank   |
| Repair           | SkillList Rank   |
| Security         | SkillList Rank   |
| Treat Injury     | SkillList Rank   |

## Class levels (SHORT)

| GFF Field   | UI Control  | Type  | Min | Max   |
|-------------|-------------|-------|-----|-------|
| ClassLevel  | Class 1 / 2 | SHORT | 0   | 32767 |

Typical level 1–20.

## KotOR 2 only

| GFF Field      | UI Control    | Type  | Notes                    |
|----------------|---------------|-------|--------------------------|
| BlindSpot      | Blindspot     | FLOAT | 0–360 degrees            |
| MultiplierSet  | Multiplier Set| BYTE  | 0–255; 0 = typical default; used for encounter scaling |

## ResRefs and strings

- **TemplateResRef, Conversation, all script hooks:** ResRef, max 16 characters.
- **Tag:** CExoString; no engine max length; keep unique per module for GetObjectByTag.
- **Comment:** CExoString; developer notes only; not read by the game.
- **FirstName, LastName:** CExoLocString; localized; no fixed max.
