# UTW (Waypoint)

Part of the [GFF File Format Documentation](GFF-File-Format).

UTW files define [waypoint templates](GFF-File-Format#utw-waypoint). Waypoints are invisible markers used for spawn points, navigation targets, map notes, and reference points for scripts.

## Documentation References

**Official Bioware Documentation:** For the authoritative Bioware Aurora Engine Waypoint format specification, see [Bioware Aurora Waypoint Format](Bioware-Aurora-Waypoint).

**Reference**: [`Libraries/PyKotor/src/pykotor/resource/generics/utw.py`](https://github.com/OldRepublicDevs/PyKotor/blob/master/Libraries/PyKotor/src/pykotor/resource/generics/utw.py)

**Engine verification (Reva):** K1 `CSWSWaypoint::LoadWaypoint` @ 0x005c7f30; `CSWSArea::LoadWaypoints` @ 0x00505360 (reads WaypointList from GIT; each element passed to LoadWaypoint).

---

## Core Identity fields

| field | type | Description |
|:------|:-----|:------------|
| `TemplateResRef` | [ResRef](GFF-File-Format#gff-data-types) | Template identifier (max 16 chars); used when GIT references this waypoint. LoadWaypoint does not read this—it is used to find the UTW file when loading GIT instances. |
| `Tag` | [CExoString](GFF-File-Format#gff-data-types) | Unique tag; engine reads via ReadFieldCExoString, default "". Scripts use GetObjectByTag; doors/triggers link to waypoint tags. |
| `LocalizedName` | [CExoLocString](GFF-File-Format#gff-data-types) | Display name on map and travel menu; ReadFieldCExoLocString. |
| `Description` | [CExoLocString](GFF-File-Format#gff-data-types) | Toolset only; engine does not read. |
| `Comment` | [CExoString](GFF-File-Format#gff-data-types) | Developer notes; engine does not read. |

---

## Map Note Functionality

| field | type | Description |
|:------|:-----|:------------|
| `HasMapNote` | Byte | ReadFieldBYTE, default 0. When non-zero, enables map note; MapNoteEnabled and MapNote are only read when this is true. |
| `MapNoteEnabled` | Byte | ReadFieldBYTE, default 0; only read when HasMapNote is true. 0 = pin hidden by default; 1 = visible. Scripts toggle with SetMapPinEnabled. |
| `MapNote` | [CExoLocString](GFF-File-Format#gff-data-types) | ReadFieldCExoLocString; only read when HasMapNote is true. Text displayed on area map. |

### Map Notes

- If HasMapNote is 0, the engine ignores MapNote and MapNoteEnabled
- Can be toggled via script (`SetMapPinEnabled`)
- Used for quest objectives and location labels

---

## Position & Orientation (in UTW or GIT)

LoadWaypoint reads XPosition, YPosition, ZPosition and XOrientation, YOrientation, ZOrientation (ReadFieldFLOAT, default 0.0). Orientation vector is normalized if not unit length. When loading from GIT, position comes from the GIT WaypointList element; the UTW template can store defaults. The Holocron UTW editor does not expose position—it is set when placing the waypoint in the area (GIT).

---

## Linking & Appearance (toolset only)

| field | type | Description |
|:------|:-----|:------------|
| `LinkedTo` | [CExoString](GFF-File-Format#gff-data-types) | Tag of linked object; engine does not read. |
| `Appearance` | Byte | Appearance type; toolset palette. |
| `PaletteID` | Byte | Toolset palette category. |

---

## Usage

- **Spawn Points**: `CreateObject` uses waypoint location
- **Patrols**: AI walks between waypoints
- **Teleport**: `JumpToLocation` / `JumpToObject` targets waypoints
- **Transitions**: Doors/Triggers link to waypoint tags
