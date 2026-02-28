/**
 * KotOR GIT/IFO Save/Load — Executable C++
 *
 * Same logic as the engine (Reva decompilation) but in runnable C++.
 * Implement IGffWriter/IGffReader for your GFF library, or use GffMemoryBackend.
 *
 * List names, struct IDs, and field names match K1 (swkotor.exe) exactly.
 */

#ifndef KOTOR_SAVE_LOAD_H
#define KOTOR_SAVE_LOAD_H

#include <cstdint>
#include <cstddef>
#include <string>
#include <vector>

// ---- Constants (K1 GIT) ----

namespace KotOR {

// GIT list labels (max 16 chars in GFF)
inline constexpr const char* LIST_CREATURE    = "Creature List";
inline constexpr const char* LIST_ITEM       = "List";
inline constexpr const char* LIST_DOOR       = "Door List";
inline constexpr const char* LIST_TRIGGER    = "TriggerList";
inline constexpr const char* LIST_ENCOUNTER  = "Encounter List";
inline constexpr const char* LIST_WAYPOINT   = "WaypointList";
inline constexpr const char* LIST_SOUND      = "SoundList";
inline constexpr const char* LIST_PLACEABLE  = "Placeable List";
inline constexpr const char* LIST_STORE      = "StoreList";
inline constexpr const char* LIST_AREA_EFFECT= "AreaEffectList";
inline constexpr const char* LIST_CAMERA     = "CameraList";

// Struct IDs for list elements (AddListElement / GetElementType)
enum StructId : uint32_t {
  GIT_STRUCT_ITEM        = 0x00,
  GIT_STRUCT_TRIGGER     = 0x01,
  GIT_STRUCT_CREATURE    = 0x04,
  GIT_STRUCT_WAYPOINT    = 0x05,
  GIT_STRUCT_SOUND      = 0x06,
  GIT_STRUCT_ENCOUNTER  = 0x07,
  GIT_STRUCT_DOOR       = 0x08,
  GIT_STRUCT_PLACEABLE  = 0x09,
  GIT_STRUCT_STORE     = 0x0B,
  GIT_STRUCT_AREA_EFFECT= 0x0D,
  GIT_STRUCT_CAMERA    = 0x0E,
  GIT_STRUCT_AREA_PROPERTIES = 100,
  GIT_STRUCT_AREA_MAP   = 0x65,
};

// GIT file type / version
inline constexpr const char* GIT_FILE_TYPE    = "GIT ";
inline constexpr const char* GIT_FILE_VERSION = "V2.0";

// ERF resource type for GIT in save
inline constexpr uint16_t ERF_TYPE_GIT = 0x7E7;
inline constexpr uint16_t ERF_TYPE_IFO = 0x7DE;
inline constexpr uint16_t ERF_TYPE_ARE = 0x7DC;

// ---- Data types (what you pass in / get out) ----

struct Vector3 {
  float x = 0.f, y = 0.f, z = 0.f;
};

struct AreaState {
  uint8_t  current_weather = 0;
  uint8_t  weather_started  = 0;
  uint8_t  trans_pending    = 0;
  uint8_t  trans_pend_next_id = 0;
  uint8_t  trans_pend_curr_id  = 0;
  uint32_t sun_fog_color    = 0;
  uint8_t  unescapable      = 0;
  uint8_t  restrict_mode    = 0;
  uint32_t stealth_xp_max   = 0;
  uint32_t stealth_xp_current = 0;
  uint32_t stealth_xp_loss  = 0;
  uint8_t  stealth_xp_enabled = 0;
  // Script vars / ambient sound: engine-specific; leave as placeholder or extend.
};

struct AreaMapState {
  bool     has_map = false;
  int32_t  res_x   = 0;
  int32_t  res_y   = 0;
  std::vector<uint32_t> data;  // DWORDs (AreaMapDataSize is in bytes = data.size() * 4)
};

struct CreatureRef {
  uint32_t object_id = 0;
  // Full creature state is written by SerializeCreature_K2; here we only need ID for iteration.
};

struct ObjectRef {
  uint32_t object_id = 0;
};

struct PlaceableCameraEntry {
  int32_t  id = 0;
  Vector3  position{};
  float    orientation[4] = {0,0,0,1};  // quaternion
  float    pitch = 0.f, height = 0.f, fov = 0.f, mic_range = 0.f;
};

// ---- GFF writer interface (implement this or use GffMemoryBackend) ----

struct GffStructHandle {
  void* impl = nullptr;  // backend-specific
};

struct GffListHandle {
  void* impl = nullptr;
};

class IGffWriter {
public:
  virtual ~IGffWriter() = default;

  // Create a new GFF in memory. Root struct is returned. file_type e.g. "GIT ", version "V2.0".
  virtual bool CreateGff(const char* file_type, const char* version, GffStructHandle* out_root) = 0;

  // Add a list to a struct (e.g. root). label e.g. "Creature List".
  virtual bool AddList(GffStructHandle parent, const char* label, GffListHandle* out_list) = 0;
  // Add an element to the list with the given struct ID. Returns handle to the new element struct.
  virtual bool AddListElement(GffListHandle list, uint32_t struct_id, GffStructHandle* out_element) = 0;

  // Add a child struct to parent (e.g. "AreaProperties", 100).
  virtual bool AddStructToStruct(GffStructHandle parent, const char* label, uint32_t struct_id, GffStructHandle* out_child) = 0;

  // Write fields into a struct
  virtual bool WriteFieldDWORD (GffStructHandle s, const char* label, uint32_t value) = 0;
  virtual bool WriteFieldINT  (GffStructHandle s, const char* label, int32_t value) = 0;
  virtual bool WriteFieldBYTE (GffStructHandle s, const char* label, uint8_t value) = 0;
  virtual bool WriteFieldWORD (GffStructHandle s, const char* label, uint16_t value) = 0;
  virtual bool WriteFieldFLOAT(GffStructHandle s, const char* label, float value) = 0;
  virtual bool WriteFieldVOID (GffStructHandle s, const char* label, const void* data, uint32_t size_bytes) = 0;
  virtual bool WriteFieldCExoString(GffStructHandle s, const char* label, const char* str) = 0;
  virtual bool WriteFieldCResRef (GffStructHandle s, const char* label, const char* resref) = 0;
  virtual bool WriteFieldVector  (GffStructHandle s, const char* label, const Vector3* v) = 0;
  virtual bool WriteFieldQuaternion(GffStructHandle s, const char* label, const float xyzw[4]) = 0;

  // Optional: get raw bytes for the GFF (so you can write to ERF or file).
  virtual bool GetGffBytes(std::vector<uint8_t>* out) = 0;
};

// ---- GFF reader interface ----

class IGffReader {
public:
  virtual ~IGffReader() = default;

  // Open a GFF from bytes. Returns root struct.
  virtual bool OpenGff(const uint8_t* data, size_t size, GffStructHandle* out_root) = 0;

  virtual bool GetList(GffStructHandle parent, const char* label, GffListHandle* out_list) = 0;
  virtual uint32_t GetListCount(GffListHandle list) = 0;
  virtual bool GetListElement(GffListHandle list, uint32_t index, GffStructHandle* out_element) = 0;
  virtual uint32_t GetElementType(GffStructHandle element) = 0;

  virtual bool GetStructFromStruct(GffStructHandle parent, const char* label, GffStructHandle* out_child) = 0;

  virtual bool ReadFieldDWORD (GffStructHandle s, const char* label, uint32_t default_val, uint32_t* out) = 0;
  virtual bool ReadFieldINT   (GffStructHandle s, const char* label, int32_t default_val, int32_t* out) = 0;
  virtual bool ReadFieldBYTE  (GffStructHandle s, const char* label, uint8_t default_val, uint8_t* out) = 0;
  virtual bool ReadFieldFLOAT (GffStructHandle s, const char* label, float default_val, float* out) = 0;
  virtual bool ReadFieldCExoString(GffStructHandle s, const char* label, std::string* out) = 0;
  virtual bool ReadFieldCResRef (GffStructHandle s, const char* label, char resref[32]) = 0;
  virtual bool ReadFieldVector  (GffStructHandle s, const char* label, Vector3* out) = 0;
  virtual bool ReadFieldVOID   (GffStructHandle s, const char* label, std::vector<uint8_t>* out) = 0;
};

// ---- Executable save/load (same logic as engine) ----

// Save area state into the GIT root: vars, weather, then entity lists, then Properties, Maps, Cameras.
// creature_ids: list of object IDs for creatures in this area (non-PC). Same for items, doors, etc.
// You can pass empty vectors; the writer will still create empty lists so the GIT is valid.
bool SaveGIT(
  IGffWriter* w,
  const char* area_resref,
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
  const std::vector<PlaceableCameraEntry>* cameras
);

// Load area state from GIT bytes. is_saved_game: if true, restore vars/weather and full object state.
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
  std::vector<PlaceableCameraEntry>* out_cameras
);

} // namespace KotOR

#endif
