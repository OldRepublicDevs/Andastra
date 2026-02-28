/**
 * In-memory GFF backend implementation.
 */

#include "GffMemoryBackend.h"
#include <cstring>

namespace KotOR {

GffStructData* GffMemoryBackend::Ptr(GffStructHandle h) {
  return static_cast<GffStructData*>(h.impl);
}
GffStructData* GffMemoryBackend::Ptr(GffListHandle) {
  return nullptr;  // we use last_list_* instead
}

// ---- Writer ----

bool GffMemoryBackend::CreateGff(const char* /* file_type */, const char* /* version */, GffStructHandle* out_root) {
  root_ = std::make_shared<GffStructData>();
  root_->struct_id = 0;
  if (out_root)
    out_root->impl = root_.get();
  return true;
}

bool GffMemoryBackend::AddList(GffStructHandle parent, const char* label, GffListHandle* out_list) {
  GffStructData* p = Ptr(parent);
  if (!p || !label || !out_list)
    return false;
  last_list_parent_ = p;
  last_list_label_ = label;
  last_list_vector_ = &p->lists[label];
  out_list->impl = p;  // dummy; AddListElement uses last_list_*
  return true;
}

bool GffMemoryBackend::AddListElement(GffListHandle /* list */, uint32_t struct_id, GffStructHandle* out_element) {
  if (!last_list_vector_ || !out_element)
    return false;
  auto elem = std::make_shared<GffStructData>();
  elem->struct_id = struct_id;
  last_list_vector_->push_back(elem);
  out_element->impl = elem.get();
  return true;
}

bool GffMemoryBackend::AddStructToStruct(GffStructHandle parent, const char* label, uint32_t struct_id, GffStructHandle* out_child) {
  GffStructData* p = Ptr(parent);
  if (!p || !label || !out_child)
    return false;
  auto child = std::make_shared<GffStructData>();
  child->struct_id = struct_id;
  p->child_structs[label] = child;
  out_child->impl = child.get();
  return true;
}

bool GffMemoryBackend::WriteFieldDWORD(GffStructHandle s, const char* label, uint32_t value) {
  GffStructData* p = Ptr(s);
  if (!p || !label) return false;
  GffFieldValue f;
  f.type = GffFieldValue::Type::DWORD;
  f.u32 = value;
  p->fields[label] = std::move(f);
  return true;
}
bool GffMemoryBackend::WriteFieldINT(GffStructHandle s, const char* label, int32_t value) {
  GffStructData* p = Ptr(s);
  if (!p || !label) return false;
  GffFieldValue f;
  f.type = GffFieldValue::Type::INT;
  f.i32 = value;
  p->fields[label] = std::move(f);
  return true;
}
bool GffMemoryBackend::WriteFieldBYTE(GffStructHandle s, const char* label, uint8_t value) {
  GffStructData* p = Ptr(s);
  if (!p || !label) return false;
  GffFieldValue f;
  f.type = GffFieldValue::Type::BYTE;
  f.u32 = value;
  p->fields[label] = std::move(f);
  return true;
}
bool GffMemoryBackend::WriteFieldWORD(GffStructHandle s, const char* label, uint16_t value) {
  GffStructData* p = Ptr(s);
  if (!p || !label) return false;
  GffFieldValue f;
  f.type = GffFieldValue::Type::WORD;
  f.u32 = value;
  p->fields[label] = std::move(f);
  return true;
}
bool GffMemoryBackend::WriteFieldFLOAT(GffStructHandle s, const char* label, float value) {
  GffStructData* p = Ptr(s);
  if (!p || !label) return false;
  GffFieldValue f;
  f.type = GffFieldValue::Type::FLOAT;
  f.f32 = value;
  p->fields[label] = std::move(f);
  return true;
}
bool GffMemoryBackend::WriteFieldVOID(GffStructHandle s, const char* label, const void* data, uint32_t size_bytes) {
  GffStructData* p = Ptr(s);
  if (!p || !label) return false;
  GffFieldValue f;
  f.type = GffFieldValue::Type::VOID;
  f.blob.assign(static_cast<const uint8_t*>(data), static_cast<const uint8_t*>(data) + size_bytes);
  p->fields[label] = std::move(f);
  return true;
}
bool GffMemoryBackend::WriteFieldCExoString(GffStructHandle s, const char* label, const char* str) {
  GffStructData* p = Ptr(s);
  if (!p || !label) return false;
  GffFieldValue f;
  f.type = GffFieldValue::Type::CEXOSTRING;
  f.str = str ? str : "";
  p->fields[label] = std::move(f);
  return true;
}
bool GffMemoryBackend::WriteFieldCResRef(GffStructHandle s, const char* label, const char* resref) {
  GffStructData* p = Ptr(s);
  if (!p || !label) return false;
  GffFieldValue f;
  f.type = GffFieldValue::Type::CRESREF;
  f.str = resref ? resref : "";
  p->fields[label] = std::move(f);
  return true;
}
bool GffMemoryBackend::WriteFieldVector(GffStructHandle s, const char* label, const Vector3* v) {
  GffStructData* p = Ptr(s);
  if (!p || !label || !v) return false;
  GffFieldValue f;
  f.type = GffFieldValue::Type::VECTOR;
  f.v3 = *v;
  p->fields[label] = std::move(f);
  return true;
}
bool GffMemoryBackend::WriteFieldQuaternion(GffStructHandle s, const char* label, const float xyzw[4]) {
  GffStructData* p = Ptr(s);
  if (!p || !label || !xyzw) return false;
  GffFieldValue f;
  f.type = GffFieldValue::Type::QUATERNION;
  std::memcpy(f.quat, xyzw, sizeof(f.quat));
  p->fields[label] = std::move(f);
  return true;
}

bool GffMemoryBackend::GetGffBytes(std::vector<uint8_t>* out) {
  if (out)
    out->clear();
  return true;  // not implemented; use OpenGff(nullptr, 0) for round-trip
}

// ---- Reader ----

bool GffMemoryBackend::OpenGff(const uint8_t* data, size_t size, GffStructHandle* out_root) {
  if (!out_root)
    return false;
  if (data == nullptr && size == 0 && root_) {
    out_root->impl = root_.get();
    return true;
  }
  return false;  // binary GFF parse not implemented
}

bool GffMemoryBackend::GetList(GffStructHandle parent, const char* label, GffListHandle* out_list) {
  GffStructData* p = Ptr(parent);
  if (!p || !label || !out_list)
    return false;
  auto it = p->lists.find(label);
  if (it == p->lists.end()) {
    last_list_vector_ = nullptr;
    return false;
  }
  last_list_parent_ = p;
  last_list_label_ = label;
  last_list_vector_ = &it->second;
  out_list->impl = p;
  return true;
}

uint32_t GffMemoryBackend::GetListCount(GffListHandle /* list */) {
  return last_list_vector_ ? static_cast<uint32_t>(last_list_vector_->size()) : 0;
}

bool GffMemoryBackend::GetListElement(GffListHandle /* list */, uint32_t index, GffStructHandle* out_element) {
  if (!last_list_vector_ || !out_element || index >= last_list_vector_->size())
    return false;
  out_element->impl = (*last_list_vector_)[index].get();
  return true;
}

uint32_t GffMemoryBackend::GetElementType(GffStructHandle element) {
  GffStructData* p = Ptr(element);
  return p ? p->struct_id : 0xFFFFFFFF;
}

bool GffMemoryBackend::GetStructFromStruct(GffStructHandle parent, const char* label, GffStructHandle* out_child) {
  GffStructData* p = Ptr(parent);
  if (!p || !label || !out_child)
    return false;
  auto it = p->child_structs.find(label);
  if (it == p->child_structs.end())
    return false;
  out_child->impl = it->second.get();
  return true;
}

static bool ReadFieldImpl(GffStructData* p, const char* label, GffFieldValue** out) {
  if (!p || !label)
    return false;
  auto it = p->fields.find(label);
  if (it == p->fields.end())
    return false;
  *out = &it->second;
  return true;
}

bool GffMemoryBackend::ReadFieldDWORD(GffStructHandle s, const char* label, uint32_t default_val, uint32_t* out) {
  GffFieldValue* f = nullptr;
  if (!out || !ReadFieldImpl(Ptr(s), label, &f)) { if (out) *out = default_val; return false; }
  if (f->type != GffFieldValue::Type::DWORD) { *out = default_val; return false; }
  *out = f->u32;
  return true;
}
bool GffMemoryBackend::ReadFieldINT(GffStructHandle s, const char* label, int32_t default_val, int32_t* out) {
  GffFieldValue* f = nullptr;
  if (!out || !ReadFieldImpl(Ptr(s), label, &f)) { if (out) *out = default_val; return false; }
  if (f->type != GffFieldValue::Type::INT) { *out = default_val; return false; }
  *out = f->i32;
  return true;
}
bool GffMemoryBackend::ReadFieldBYTE(GffStructHandle s, const char* label, uint8_t default_val, uint8_t* out) {
  GffFieldValue* f = nullptr;
  if (!out || !ReadFieldImpl(Ptr(s), label, &f)) { if (out) *out = default_val; return false; }
  if (f->type != GffFieldValue::Type::BYTE) { *out = default_val; return false; }
  *out = static_cast<uint8_t>(f->u32);
  return true;
}
bool GffMemoryBackend::ReadFieldFLOAT(GffStructHandle s, const char* label, float default_val, float* out) {
  GffFieldValue* f = nullptr;
  if (!out || !ReadFieldImpl(Ptr(s), label, &f)) { if (out) *out = default_val; return false; }
  if (f->type != GffFieldValue::Type::FLOAT) { *out = default_val; return false; }
  *out = f->f32;
  return true;
}
bool GffMemoryBackend::ReadFieldCExoString(GffStructHandle s, const char* label, std::string* out) {
  GffFieldValue* f = nullptr;
  if (!out || !ReadFieldImpl(Ptr(s), label, &f)) return false;
  if (f->type != GffFieldValue::Type::CEXOSTRING) return false;
  *out = f->str;
  return true;
}
bool GffMemoryBackend::ReadFieldCResRef(GffStructHandle s, const char* label, char resref[32]) {
  GffFieldValue* f = nullptr;
  if (!resref || !ReadFieldImpl(Ptr(s), label, &f)) { if (resref) resref[0] = '\0'; return false; }
  if (f->type != GffFieldValue::Type::CRESREF) { resref[0] = '\0'; return false; }
  size_t len = f->str.size() < 31 ? f->str.size() : 31;
  std::memcpy(resref, f->str.c_str(), len);
  resref[len] = '\0';
  return true;
}
bool GffMemoryBackend::ReadFieldVector(GffStructHandle s, const char* label, Vector3* out) {
  GffFieldValue* f = nullptr;
  if (!out || !ReadFieldImpl(Ptr(s), label, &f)) return false;
  if (f->type != GffFieldValue::Type::VECTOR) return false;
  *out = f->v3;
  return true;
}
bool GffMemoryBackend::ReadFieldVOID(GffStructHandle s, const char* label, std::vector<uint8_t>* out) {
  GffFieldValue* f = nullptr;
  if (!out || !ReadFieldImpl(Ptr(s), label, &f)) return false;
  if (f->type != GffFieldValue::Type::VOID) return false;
  *out = f->blob;
  return true;
}

} // namespace KotOR
