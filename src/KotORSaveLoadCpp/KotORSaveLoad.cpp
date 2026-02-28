/**
 * KotOR GIT Save/Load — Executable implementation.
 * Same logic as engine (K1 SaveGIT @ 0x0050ba00, LoadGIT @ 0x0050dd80) in runnable C++.
 */

#include "KotORSaveLoad.h"
#include <cstring>

namespace KotOR {

// ---- Helpers: write one entity list (Creatures, Items, Doors, ...) ----

static bool SaveEntityList(
  IGffWriter* w,
  GffStructHandle root,
  const char* list_label,
  uint32_t struct_id,
  const std::vector<uint32_t>& object_ids)
{
  GffListHandle list;
  if (!w->AddList(root, list_label, &list))
    return false;
  for (uint32_t obj_id : object_ids) {
    GffStructHandle elem;
    if (!w->AddListElement(list, struct_id, &elem))
      return false;
    if (!w->WriteFieldDWORD(elem, "ObjectId", obj_id))
      return false;
    // Full per-type state (SaveItem, SaveDoor, SaveObjectState, etc.) would go here.
  }
  return true;
}

// ---- SaveProperties: AreaProperties child (struct ID 100) ----

static bool SaveProperties(
  IGffWriter* w,
  GffStructHandle root,
  const AreaState* area_state)
{
  GffStructHandle child;
  if (!w->AddStructToStruct(root, "AreaProperties", GIT_STRUCT_AREA_PROPERTIES, &child))
    return false;
  if (!w->WriteFieldBYTE(child, "Unescapable", area_state->unescapable))
    return false;
  if (!w->WriteFieldBYTE(child, "RestrictMode", area_state->restrict_mode))
    return false;
  if (!w->WriteFieldDWORD(child, "StealthXPMax", area_state->stealth_xp_max))
    return false;
  if (!w->WriteFieldDWORD(child, "StealthXPCurrent", area_state->stealth_xp_current))
    return false;
  if (!w->WriteFieldDWORD(child, "StealthXPLoss", area_state->stealth_xp_loss))
    return false;
  if (!w->WriteFieldBYTE(child, "StealthXPEnabled", area_state->stealth_xp_enabled))
    return false;
  if (!w->WriteFieldBYTE(child, "TransPending", area_state->trans_pending))
    return false;
  if (!w->WriteFieldBYTE(child, "TransPendNextID", area_state->trans_pend_next_id))
    return false;
  if (!w->WriteFieldBYTE(child, "TransPendCurrID", area_state->trans_pend_curr_id))
    return false;
  if (!w->WriteFieldDWORD(child, "SunFogColor", area_state->sun_fog_color))
    return false;
  return true;
}

// ---- SaveMaps: AreaMap child (struct ID 0x65) ----

static bool SaveMaps(
  IGffWriter* w,
  GffStructHandle root,
  const AreaMapState* map_state)
{
  if (!map_state || !map_state->has_map || map_state->data.empty())
    return true;
  GffStructHandle child;
  if (!w->AddStructToStruct(root, "AreaMap", GIT_STRUCT_AREA_MAP, &child))
    return false;
  if (!w->WriteFieldINT(child, "AreaMapResX", map_state->res_x))
    return false;
  if (!w->WriteFieldINT(child, "AreaMapResY", map_state->res_y))
    return false;
  uint32_t size_bytes = static_cast<uint32_t>(map_state->data.size() * sizeof(uint32_t));
  if (!w->WriteFieldDWORD(child, "AreaMapDataSize", size_bytes))
    return false;
  if (!w->WriteFieldVOID(child, "AreaMapData", map_state->data.data(), size_bytes))
    return false;
  return true;
}

// ---- SavePlaceableCameras: CameraList, struct ID 14 ----

static bool SavePlaceableCameras(
  IGffWriter* w,
  GffStructHandle root,
  const std::vector<PlaceableCameraEntry>* cameras)
{
  if (!cameras)
    return true;
  GffListHandle list;
  if (!w->AddList(root, LIST_CAMERA, &list))
    return false;
  for (const auto& cam : *cameras) {
    GffStructHandle elem;
    if (!w->AddListElement(list, GIT_STRUCT_CAMERA, &elem))
      return false;
    if (!w->WriteFieldINT(elem, "CameraID", cam.id))
      return false;
    if (!w->WriteFieldVector(elem, "Position", &cam.position))
      return false;
    if (!w->WriteFieldQuaternion(elem, "Orientation", cam.orientation))
      return false;
    if (!w->WriteFieldFLOAT(elem, "Pitch", cam.pitch))
      return false;
    if (!w->WriteFieldFLOAT(elem, "Height", cam.height))
      return false;
    if (!w->WriteFieldFLOAT(elem, "FieldOfView", cam.fov))
      return false;
    if (!w->WriteFieldFLOAT(elem, "MicRange", cam.mic_range))
      return false;
  }
  return true;
}

// ---- SaveGIT (full engine order) ----

bool SaveGIT(
  IGffWriter* w,
  const char* /* area_resref */,
  const AreaState* area_state,
  const AreaMapState* map_state,
  const std::vector<uint32_t>& creature_ids,
  const std::vector<uint32_t>& item_ids,
  const std::vector<uint32_t>& door_ids,
  const std::vector<uint32_t>& trigger_ids,
  const std::vector<uint32_t>& encounter_ids,
  const std::vector<uint32_t>& waypoint_ids,
  const std::vector<uint32_t>& sound_ids,
  const std::vector<uint32_t>& placeable_ids,
  const std::vector<uint32_t>& store_ids,
  const std::vector<uint32_t>& area_effect_ids,
  const std::vector<PlaceableCameraEntry>* cameras)
{
  GffStructHandle root;
  if (!w->CreateGff(GIT_FILE_TYPE, GIT_FILE_VERSION, &root))
    return false;

  // 1) Before any list: weather/transition (SaveVarTable x2 omitted; implement via backend if needed)
  const AreaState* state = area_state ? area_state : static_cast<const AreaState*>(nullptr);
  if (state) {
    if (!w->WriteFieldBYTE(root, "CurrentWeather", state->current_weather))
      return false;
    if (!w->WriteFieldBYTE(root, "WeatherStarted", state->weather_started))
      return false;
    if (!w->WriteFieldBYTE(root, "TransPending", state->trans_pending))
      return false;
    if (!w->WriteFieldBYTE(root, "TransPendNextID", state->trans_pend_next_id))
      return false;
    if (!w->WriteFieldBYTE(root, "TransPendCurrID", state->trans_pend_curr_id))
      return false;
  }

  // 2) Entity lists in engine order
  if (!SaveEntityList(w, root, LIST_CREATURE,    GIT_STRUCT_CREATURE,    creature_ids))   return false;
  if (!SaveEntityList(w, root, LIST_ITEM,       GIT_STRUCT_ITEM,         item_ids))      return false;
  if (!SaveEntityList(w, root, LIST_DOOR,       GIT_STRUCT_DOOR,         door_ids))      return false;
  if (!SaveEntityList(w, root, LIST_TRIGGER,    GIT_STRUCT_TRIGGER,      trigger_ids))   return false;
  if (!SaveEntityList(w, root, LIST_ENCOUNTER, GIT_STRUCT_ENCOUNTER,    encounter_ids)) return false;
  if (!SaveEntityList(w, root, LIST_WAYPOINT,   GIT_STRUCT_WAYPOINT,     waypoint_ids))  return false;
  if (!SaveEntityList(w, root, LIST_SOUND,      GIT_STRUCT_SOUND,        sound_ids))     return false;
  if (!SaveEntityList(w, root, LIST_PLACEABLE,  GIT_STRUCT_PLACEABLE,    placeable_ids)) return false;
  if (!SaveEntityList(w, root, LIST_STORE,      GIT_STRUCT_STORE,        store_ids))     return false;
  if (!SaveEntityList(w, root, LIST_AREA_EFFECT, GIT_STRUCT_AREA_EFFECT, area_effect_ids)) return false;

  // 3) After entity lists: Properties, Maps, PlaceableCameras
  if (state && !SaveProperties(w, root, state))
    return false;
  if (!SaveMaps(w, root, map_state))
    return false;
  if (!SavePlaceableCameras(w, root, cameras))
    return false;

  return true;
}

// ---- Load: read one entity list (check struct ID, push ObjectId) ----

static bool LoadEntityList(
  IGffReader* r,
  GffStructHandle root,
  const char* list_label,
  uint32_t expected_struct_id,
  std::vector<uint32_t>* out_ids)
{
  if (!out_ids)
    return true;
  out_ids->clear();
  GffListHandle list;
  if (!r->GetList(root, list_label, &list))
    return true;  // missing list = empty
  uint32_t n = r->GetListCount(list);
  for (uint32_t i = 0; i < n; i++) {
    GffStructHandle elem;
    if (!r->GetListElement(list, i, &elem))
      continue;
    if (r->GetElementType(elem) != expected_struct_id)
      continue;
    uint32_t obj_id = 0;
    if (r->ReadFieldDWORD(elem, "ObjectId", 0, &obj_id))
      out_ids->push_back(obj_id);
  }
  return true;
}

// ---- LoadProperties ----

static bool LoadProperties(
  IGffReader* r,
  GffStructHandle root,
  AreaState* out_area_state)
{
  if (!out_area_state)
    return true;
  GffStructHandle child;
  if (!r->GetStructFromStruct(root, "AreaProperties", &child))
    return true;
  r->ReadFieldBYTE(child, "Unescapable", 0, &out_area_state->unescapable);
  r->ReadFieldBYTE(child, "RestrictMode", 0, &out_area_state->restrict_mode);
  uint32_t u32 = 0;
  r->ReadFieldDWORD(child, "StealthXPMax", 0, &u32); out_area_state->stealth_xp_max = u32;
  r->ReadFieldDWORD(child, "StealthXPCurrent", 0, &u32); out_area_state->stealth_xp_current = u32;
  r->ReadFieldDWORD(child, "StealthXPLoss", 0, &u32); out_area_state->stealth_xp_loss = u32;
  r->ReadFieldBYTE(child, "StealthXPEnabled", 0, &out_area_state->stealth_xp_enabled);
  r->ReadFieldBYTE(child, "TransPending", 0, &out_area_state->trans_pending);
  r->ReadFieldBYTE(child, "TransPendNextID", 0, &out_area_state->trans_pend_next_id);
  r->ReadFieldBYTE(child, "TransPendCurrID", 0, &out_area_state->trans_pend_curr_id);
  r->ReadFieldDWORD(child, "SunFogColor", 0, &out_area_state->sun_fog_color);
  return true;
}

// ---- LoadMaps ----

static bool LoadMaps(
  IGffReader* r,
  GffStructHandle root,
  AreaMapState* out_map_state)
{
  if (!out_map_state)
    return true;
  GffStructHandle child;
  if (!r->GetStructFromStruct(root, "AreaMap", &child))
    return true;
  int32_t rx = 0, ry = 0;
  r->ReadFieldINT(child, "AreaMapResX", 0, &rx);
  r->ReadFieldINT(child, "AreaMapResY", 0, &ry);
  uint32_t size_bytes = 0;
  r->ReadFieldDWORD(child, "AreaMapDataSize", 0, &size_bytes);
  std::vector<uint8_t> raw;
  if (r->ReadFieldVOID(child, "AreaMapData", &raw) && !raw.empty() && size_bytes > 0) {
    out_map_state->has_map = true;
    out_map_state->res_x = rx;
    out_map_state->res_y = ry;
    size_t num_dwords = (size_bytes + 3) / 4;
    out_map_state->data.resize(num_dwords);
    size_t copy = (raw.size() < size_bytes) ? raw.size() : size_bytes;
    std::memcpy(out_map_state->data.data(), raw.data(), copy);
  }
  return true;
}

// ---- LoadPlaceableCameras ----

static bool LoadPlaceableCameras(
  IGffReader* r,
  GffStructHandle root,
  std::vector<PlaceableCameraEntry>* out_cameras)
{
  if (!out_cameras)
    return true;
  out_cameras->clear();
  GffListHandle list;
  if (!r->GetList(root, LIST_CAMERA, &list))
    return true;
  uint32_t n = r->GetListCount(list);
  for (uint32_t i = 0; i < n; i++) {
    GffStructHandle elem;
    if (!r->GetListElement(list, i, &elem))
      continue;
    if (r->GetElementType(elem) != GIT_STRUCT_CAMERA)
      continue;
    PlaceableCameraEntry cam;
    int32_t id = 0;
    if (r->ReadFieldINT(elem, "CameraID", 0, &id))
      cam.id = id;
    r->ReadFieldVector(elem, "Position", &cam.position);
    // Orientation: GFF often stores as 4 floats; if your reader has ReadFieldQuaternion use it
    float ox = 0.f, oy = 0.f, oz = 0.f, ow = 1.f;
    r->ReadFieldFLOAT(elem, "Pitch", 0.f, &cam.pitch);
    r->ReadFieldFLOAT(elem, "Height", 0.f, &cam.height);
    r->ReadFieldFLOAT(elem, "FieldOfView", 0.f, &cam.fov);
    r->ReadFieldFLOAT(elem, "MicRange", 0.f, &cam.mic_range);
    cam.orientation[0] = ox;
    cam.orientation[1] = oy;
    cam.orientation[2] = oz;
    cam.orientation[3] = ow;
    out_cameras->push_back(cam);
  }
  return true;
}

// ---- LoadGIT (full engine order) ----

bool LoadGIT(
  IGffReader* r,
  const uint8_t* gff_bytes,
  size_t gff_size,
  bool is_saved_game,
  AreaState* out_area_state,
  AreaMapState* out_map_state,
  std::vector<uint32_t>* out_creature_ids,
  std::vector<uint32_t>* out_item_ids,
  std::vector<uint32_t>* out_door_ids,
  std::vector<uint32_t>* out_trigger_ids,
  std::vector<uint32_t>* out_encounter_ids,
  std::vector<uint32_t>* out_waypoint_ids,
  std::vector<uint32_t>* out_sound_ids,
  std::vector<uint32_t>* out_placeable_ids,
  std::vector<uint32_t>* out_store_ids,
  std::vector<uint32_t>* out_area_effect_ids,
  std::vector<PlaceableCameraEntry>* out_cameras)
{
  GffStructHandle root;
  if (!r->OpenGff(gff_bytes, gff_size, &root))
    return false;

  if (is_saved_game && out_area_state) {
    // LoadVarTable x2 would go here (script/var tables); backend-dependent
    uint8_t b = 0;
    if (r->ReadFieldBYTE(root, "CurrentWeather", 0, &b))
      out_area_state->current_weather = b;
    if (r->ReadFieldBYTE(root, "WeatherStarted", 0, &b))
      out_area_state->weather_started = b;
  }

  uint8_t use_templates = 0;
  r->ReadFieldBYTE(root, "UseTemplates", 0, &use_templates);
  (void)use_templates;  // pass to entity loaders if implementing full spawn-from-template

  if (!LoadEntityList(r, root, LIST_CREATURE,     GIT_STRUCT_CREATURE,     out_creature_ids))   return false;
  if (!LoadEntityList(r, root, LIST_ITEM,        GIT_STRUCT_ITEM,         out_item_ids))      return false;
  if (!LoadEntityList(r, root, LIST_DOOR,         GIT_STRUCT_DOOR,         out_door_ids))      return false;
  if (!LoadEntityList(r, root, LIST_TRIGGER,      GIT_STRUCT_TRIGGER,      out_trigger_ids))   return false;
  if (!LoadEntityList(r, root, LIST_ENCOUNTER,    GIT_STRUCT_ENCOUNTER,    out_encounter_ids)) return false;
  if (!LoadEntityList(r, root, LIST_WAYPOINT,     GIT_STRUCT_WAYPOINT,     out_waypoint_ids))  return false;
  if (!LoadEntityList(r, root, LIST_SOUND,       GIT_STRUCT_SOUND,         out_sound_ids))     return false;
  if (!LoadEntityList(r, root, LIST_PLACEABLE,    GIT_STRUCT_PLACEABLE,    out_placeable_ids)) return false;
  if (!LoadEntityList(r, root, LIST_STORE,       GIT_STRUCT_STORE,         out_store_ids))     return false;
  if (!LoadEntityList(r, root, LIST_AREA_EFFECT,  GIT_STRUCT_AREA_EFFECT,   out_area_effect_ids)) return false;

  if (!LoadProperties(r, root, out_area_state))
    return false;
  if (!LoadMaps(r, root, out_map_state))
    return false;
  if (!LoadPlaceableCameras(r, root, out_cameras))
    return false;

  return true;
}

} // namespace KotOR
