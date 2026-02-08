# Reva (Ghidra) project – binaries and confirmation

Documentation in the codebase that cites **Reva** refers to reverse‑engineered behavior from the Ghidra project used by the Andastra-Reva MCP server.

## Binaries currently in the Reva project

- **k1_win_gog_swkotor.exe** – KotOR 1 (Odyssey)
- **k2_win_gog_aspyr_swkotor2.exe** – KotOR 2 Aspyr (Odyssey)
- **k2_win_gog_legacypc_swkotor2.exe** – KotOR 2 Legacy PC (Odyssey)

All Reva-confirmed comments that mention addresses (e.g. `CSWGuiMainMenu @ 0x0067c4c0`) are from **k1_win_gog_swkotor.exe** unless stated otherwise. K2 equivalents can be confirmed in the K2 binaries.

## Binaries to add for Aurora / Eclipse confirmation

To confirm behavior for **Neverwinter Nights**, **Dragon Age Origins**, and **Dragon Age II**, add these executables to the Reva (Ghidra) project. Import may need a longer timeout for large binaries.

| Engine  | Path |
|--------|------|
| Aurora (NWN) | `G:\SteamLibrary\steamapps\common\Neverwinter Nights\bin\win32\nwmain.exe` |
| Eclipse (DAO) | `G:\Games\Dragon Age\bin_ship\daorigins.exe` |
| Eclipse (DA2) | `G:\Games\Dragon Age II\bin_ship\DragonAge2.exe` |

After they are imported and analyzed, replace any remaining “vendor/reone”, “vendor/xoreos”, or “vendor/KotOR.js” references with Reva-confirmed function/symbol names and addresses from the corresponding binary.

## Reva references in code

- **Main menu / GUI**: `DisplayMainMenu` @ 0x005fca30, `CSWGuiMainMenu` @ 0x0067c4c0, `LoadFromLayout` @ 0x0067ace0, `CSWGuiBorder::Draw` @ 0x004168c0, `GetFullScreenBG` @ 0x0040a900, `GetScreenResolutionString` @ 0x0040a3e0, `CSWGuiManager` @ 0x0040bad0.
- **TXI**: `Material::Init` @ 0x0047b110, `CAurTextureBasic::Init` @ 0x00422af0 (AurResGet ResRef + ".txi", ParseField).
- **Area**: `CSWSArea::LoadAreaHeader` @ 0x00508c50 (`DynAmbientColor` → `sw_area.dynamic_ambient_color`).
- **VBO/IBO**: `GLRender::SetVertexBuffer` @ 0x00425900 (glBindBufferARB 0x8892, glVertexPointer), `AurVertexBufferObjectARBAvailable`.
