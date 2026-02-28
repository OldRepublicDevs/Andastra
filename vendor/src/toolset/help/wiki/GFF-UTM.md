# UTM (Merchant)

Part of the [GFF File Format Documentation](GFF-File-Format).

UTM files define [merchant templates](GFF-File-Format#utm-merchant) with inventory lists, pricing, and merchant-specific properties.

**Official Bioware Documentation:** For the authoritative Bioware Aurora Engine Store format specification, see [Bioware Aurora Store Format](Bioware-Aurora-Store).

**Reference**: [`Libraries/PyKotor/src/pykotor/resource/generics/utm.py`](https://github.com/OldRepublicDevs/PyKotor/blob/master/Libraries/PyKotor/src/pykotor/resource/generics/utm.py)

**Engine verification (Reva):** K1 `CSWSStore::LoadStore` @ 0x005c7180; `LoadStores` @ 0x005057a0; `LoadFromTemplate` @ 0x005c7760.

---

## Core Identity fields

| field | type | Description |
|:------|:-----|:------------|
| `ResRef` | [ResRef](GFF-File-Format#gff-data-types) | Template identifier (max 16 chars); used when GIT references this merchant |
| `Tag` | [CExoString](GFF-File-Format#gff-data-types) | Unique tag; ReadFieldCExoString, default "". Scripts use GetObjectByTag |
| `LocName` | [CExoLocString](GFF-File-Format#gff-data-types) | Display name in store UI; ReadFieldCExoLocString |

---

## Pricing & Script

| field | type | Description |
|:------|:-----|:------------|
| `MarkUp` | INT32 | ReadFieldINT, default 0. Percentage multiplier for sell price: 100 = base, 150 = 50% markup |
| `MarkDown` | INT32 | ReadFieldINT, default 0. Percentage of base price when player sells: 100 = full, 50 = half |
| `OnOpenStore` | [ResRef](GFF-File-Format#gff-data-types) | ReadFieldCResRef. NCS script (max 16 chars) fired when store opens |
| `BuySellFlag` | Byte | ReadFieldBYTE. Bit 0 = can buy from player; bit 1 = can sell to player |

---

## Inventory

| field | type | Description |
|:------|:-----|:------------|
| `ItemList` | List | GetList/GetListElement. Each element: ObjectId (DWORD), InventoryRes (CResRef), Infinite (BYTE), Dropable (BYTE) |

---

## Toolset-only

| field | type | Description |
|:------|:-----|:------------|
| `ID` | Byte | Palette/organization; engine does not read. Stored as 0–255 |
| `Comment` | [CExoString](GFF-File-Format#gff-data-types) | Developer notes; engine does not read |
