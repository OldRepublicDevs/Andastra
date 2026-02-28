/**
 * Runnable test: SaveGIT -> round-trip in memory -> LoadGIT. Verifies the C++ logic runs.
 * Build and run in a C++ project (e.g. add all .cpp and include this directory).
 */

#include "GffMemoryBackend.h"
#include "KotORSaveLoad.h"
#include <cstdio>
#include <cstdlib>

using namespace KotOR;

int main() {
  GffMemoryBackend backend;

  // ---- Save ----
  AreaState area_state{};
  area_state.current_weather = 1;
  area_state.weather_started = 1;
  area_state.stealth_xp_max = 100;
  area_state.stealth_xp_current = 50;

  AreaMapState map_state{};
  map_state.has_map = true;
  map_state.res_x = 64;
  map_state.res_y = 64;
  map_state.data.resize(64 * 64, 0xFFu);

  std::vector<uint32_t> creature_ids = { 100, 101, 102 };
  std::vector<uint32_t> item_ids = { 200, 201 };
  std::vector<uint32_t> door_ids = { 300 };

  std::vector<PlaceableCameraEntry> cameras;
  cameras.push_back({ 1, {1.f, 2.f, 3.f}, {0,0,0,1}, 0.5f, 1.5f, 90.f, 10.f });

  bool ok = SaveGIT(
    &backend,
    "end_m01aa",
    &area_state,
    &map_state,
    creature_ids,
    item_ids,
    door_ids,
    {}, {}, {}, {}, {}, {}, {},  // trigger, encounter, waypoint, sound, placeable, store, area_effect
    &cameras
  );
  if (!ok) {
    std::fprintf(stderr, "SaveGIT failed\n");
    return 1;
  }

  // ---- Load (round-trip: OpenGff(nullptr, 0) uses in-memory root) ----
  AreaState loaded_state{};
  AreaMapState loaded_map{};
  std::vector<uint32_t> loaded_creatures, loaded_items, loaded_doors;
  std::vector<PlaceableCameraEntry> loaded_cameras;

  ok = LoadGIT(
    &backend,
    nullptr,
    0,
    true,
    &loaded_state,
    &loaded_map,
    &loaded_creatures,
    &loaded_items,
    &loaded_doors,
    nullptr, nullptr, nullptr, nullptr, nullptr, nullptr, nullptr,  // trigger..area_effect
    &loaded_cameras
  );
  if (!ok) {
    std::fprintf(stderr, "LoadGIT failed\n");
    return 1;
  }

  // ---- Verify ----
  if (loaded_state.current_weather != area_state.current_weather ||
      loaded_state.stealth_xp_max != area_state.stealth_xp_max) {
    std::fprintf(stderr, "Area state mismatch\n");
    return 1;
  }
  if (loaded_creatures != creature_ids || loaded_items != item_ids || loaded_doors != door_ids) {
    std::fprintf(stderr, "Entity ID list mismatch\n");
    return 1;
  }
  if (!loaded_map.has_map || loaded_map.res_x != map_state.res_x || loaded_map.res_y != map_state.res_y ||
      loaded_map.data.size() != map_state.data.size()) {
    std::fprintf(stderr, "Map state mismatch\n");
    return 1;
  }
  if (loaded_cameras.size() != 1 || loaded_cameras[0].id != 1) {
    std::fprintf(stderr, "Camera list mismatch\n");
    return 1;
  }

  std::printf("OK: SaveGIT -> LoadGIT round-trip passed.\n");
  return 0;
}
