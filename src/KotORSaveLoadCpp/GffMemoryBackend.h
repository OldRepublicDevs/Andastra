/**
 * In-memory GFF backend: implements IGffWriter and IGffReader so SaveGIT/LoadGIT
 * run without a real GFF file. Use for tests or plug in a real GFF library later.
 *
 * OpenGff(nullptr, 0, &root) binds to the root created by CreateGff (round-trip in memory).
 */

#ifndef GFF_MEMORY_BACKEND_H
#define GFF_MEMORY_BACKEND_H

#include "KotORSaveLoad.h"
#include <map>
#include <memory>
#include <string>

namespace KotOR {

struct GffFieldValue {
  enum class Type { DWORD, INT, BYTE, WORD, FLOAT, VOID, CEXOSTRING, CRESREF, VECTOR, QUATERNION };
  Type type;
  std::vector<uint8_t> blob;  // for VOID; for others we use union-like storage
  uint32_t u32 = 0;
  int32_t  i32 = 0;
  float    f32 = 0.f;
  std::string str;
  Vector3  v3{};
  float    quat[4] = {0,0,0,1};
};

struct GffStructData {
  uint32_t struct_id = 0;
  std::map<std::string, GffFieldValue> fields;
  std::map<std::string, std::shared_ptr<GffStructData>> child_structs;
  std::map<std::string, std::vector<std::shared_ptr<GffStructData>>> lists;
};

class GffMemoryBackend : public IGffWriter, public IGffReader {
public:
  GffMemoryBackend() = default;
  ~GffMemoryBackend() override = default;

  // ---- IGffWriter ----
  bool CreateGff(const char* file_type, const char* version, GffStructHandle* out_root) override;
  bool AddList(GffStructHandle parent, const char* label, GffListHandle* out_list) override;
  bool AddListElement(GffListHandle list, uint32_t struct_id, GffStructHandle* out_element) override;
  bool AddStructToStruct(GffStructHandle parent, const char* label, uint32_t struct_id, GffStructHandle* out_child) override;
  bool WriteFieldDWORD (GffStructHandle s, const char* label, uint32_t value) override;
  bool WriteFieldINT  (GffStructHandle s, const char* label, int32_t value) override;
  bool WriteFieldBYTE (GffStructHandle s, const char* label, uint8_t value) override;
  bool WriteFieldWORD (GffStructHandle s, const char* label, uint16_t value) override;
  bool WriteFieldFLOAT(GffStructHandle s, const char* label, float value) override;
  bool WriteFieldVOID (GffStructHandle s, const char* label, const void* data, uint32_t size_bytes) override;
  bool WriteFieldCExoString(GffStructHandle s, const char* label, const char* str) override;
  bool WriteFieldCResRef (GffStructHandle s, const char* label, const char* resref) override;
  bool WriteFieldVector  (GffStructHandle s, const char* label, const Vector3* v) override;
  bool WriteFieldQuaternion(GffStructHandle s, const char* label, const float xyzw[4]) override;
  bool GetGffBytes(std::vector<uint8_t>* out) override;

  // ---- IGffReader ----
  bool OpenGff(const uint8_t* data, size_t size, GffStructHandle* out_root) override;
  bool GetList(GffStructHandle parent, const char* label, GffListHandle* out_list) override;
  uint32_t GetListCount(GffListHandle list) override;
  bool GetListElement(GffListHandle list, uint32_t index, GffStructHandle* out_element) override;
  uint32_t GetElementType(GffStructHandle element) override;
  bool GetStructFromStruct(GffStructHandle parent, const char* label, GffStructHandle* out_child) override;
  bool ReadFieldDWORD (GffStructHandle s, const char* label, uint32_t default_val, uint32_t* out) override;
  bool ReadFieldINT   (GffStructHandle s, const char* label, int32_t default_val, int32_t* out) override;
  bool ReadFieldBYTE  (GffStructHandle s, const char* label, uint8_t default_val, uint8_t* out) override;
  bool ReadFieldFLOAT (GffStructHandle s, const char* label, float default_val, float* out) override;
  bool ReadFieldCExoString(GffStructHandle s, const char* label, std::string* out) override;
  bool ReadFieldCResRef (GffStructHandle s, const char* label, char resref[32]) override;
  bool ReadFieldVector  (GffStructHandle s, const char* label, Vector3* out) override;
  bool ReadFieldVOID   (GffStructHandle s, const char* label, std::vector<uint8_t>* out) override;

private:
  std::shared_ptr<GffStructData> root_;
  // List operations use "current list" (last AddList / GetList) so we don't need to store list handles.
  GffStructData* last_list_parent_ = nullptr;
  std::string last_list_label_;
  std::vector<std::shared_ptr<GffStructData>>* last_list_vector_ = nullptr;

  static GffStructData* Ptr(GffStructHandle h);
  static GffStructData* Ptr(GffListHandle h);
};

} // namespace KotOR

#endif
