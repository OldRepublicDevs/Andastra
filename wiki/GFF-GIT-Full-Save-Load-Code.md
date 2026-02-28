# KotOR Save/Load: Full C/C++ Decompilation

**Binary:** `/k1_win_gog_swkotor.exe` (K1 GoG).  
**Source:** Agentdecompile `get-functions` decompile. Every function in the save and load paths is listed below with **full C/C++ code** as decompiled; callees are either inlined by reference (see § FunctionName) or are engine/runtime API (e.g. `CResGFF::WriteFieldDWORD`, `operator_new`).

**For exhaustive serialization with Agentdecompile as primary focus** — full save/load checklists, every K1 address, Agentdecompile commands to expand any callee, and GFF field reference — see **[GFF-Save-Load-Exhaustive.md](GFF-Save-Load-Exhaustive.md)**.

**Convention:** Code blocks are verbatim from Agentdecompile. `CONCAT31`, `SUB41`, etc. are Ghidra artifacts; treat as the combined value or byte. `(**(code **)(x)->vtable)(1)` is a virtual destructor/release call.

---

## Save path: top-level

### StoreCurrentModule (K1 @ 0x004b2e70)

```c
undefined4 __thiscall CServerExoAppInternal::StoreCurrentModule(CServerExoAppInternal *this)
{
  CSWSModule *this_00;
  CExoString *pCVar1;
  int iVar2;
  int iVar3;
  CExoString local_40;
  CExoString local_38;
  CExoString local_30;
  CExoString local_28;
  CExoString local_20;
  void *local_14;
  code *pcStack_10;
  undefined4 local_c;

  local_c = 0xffffffff;
  pcStack_10 = FrameHandler_00718730;
  local_14 = ExceptionList;
  ExceptionList = &local_14;
  this_00 = GetModule(this);
  if (this_00 != (CSWSModule *)0x0) {
    pCVar1 = (CExoString *)CSWSModule::GetModuleResourceName(this_00,&local_30);
    local_c = 0;
    iVar2 = IncludeModuleInSave(this,pCVar1);
    local_c = 0xffffffff;
    CExoString::~CExoString(&local_30);
    if (iVar2 != 0) {
      CExoString::CExoString(&local_38,"GAMEINPROGRESS:");
      local_c = 1;
      CExoString::CExoString(&local_40,&this_00->field16_0x5c);
      local_c._0_1_ = 2;
      iVar2 = CExoString::Find(&local_40,':',0);
      if (iVar2 == -1) {
        pCVar1 = CExoString::operator+(&local_38,&local_30,&local_40);
        local_c = CONCAT31(local_c._1_3_,3);
        CExoString::operator=(&local_40,pCVar1);
        pCVar1 = &local_30;
      }
      else {
        iVar3 = CExoString::GetLength(&local_40);
        pCVar1 = CExoString::Right(&local_40,&local_20,(iVar3 - iVar2) + -1);
        local_c._0_1_ = 4;
        pCVar1 = CExoString::operator+(&local_38,&local_28,pCVar1);
        local_c._0_1_ = 5;
        CExoString::operator=(&local_40,pCVar1);
        local_c = CONCAT31(local_c._1_3_,4);
        CExoString::~CExoString(&local_28);
        pCVar1 = &local_20;
      }
      local_c._0_1_ = 2;
      CExoString::~CExoString(pCVar1);
      CSWSModule::SaveModuleStart(this_00,&local_38,&local_40);
      CSWSModule::SaveModuleInProgress(this_00);
      CSWSModule::SaveModuleFinish(this_00,&local_38,&local_40);
      local_c = CONCAT31(local_c._1_3_,1);
      CExoString::~CExoString(&local_40);
      local_c = 0xffffffff;
      CExoString::~CExoString(&local_38);
    }
  }
  ExceptionList = local_14;
  return 1;
}
```

**Callees (full code in this doc):** § SaveModuleStart, § SaveModuleInProgress, § SaveModuleFinish.

---

### SaveModuleStart (K1 @ 0x004c8960)

```c
void __thiscall CSWSModule::SaveModuleStart(CSWSModule *this,CExoString *param_1,CExoString *param_2)
{
  char *lpFileName;
  CERFFile *pCVar1;
  undefined4 *puVar2;
  CResGFF *this_00;
  undefined4 uVar3;
  void *pvVar4;
  int iVar5;
  CExoString local_24;
  CExoString local_1c;
  CExoString local_14;
  void *pvStack_c;
  code *pcStack_8;
  int local_4;

  local_4 = 0xffffffff;
  pcStack_8 = FrameHandler_00719a39;
  pvStack_c = ExceptionList;
  ExceptionList = &pvStack_c;
  this->is_save_game = 1;
  CExoAliasList::ResolveFileName(&local_14,param_2,0xbc1);
  local_4 = 0;
  lpFileName = CExoString::CStr(&local_14);
  DeleteFileA(lpFileName);
  pCVar1 = operator_new(0xd0);
  local_4._0_1_ = 1;
  if (pCVar1 == (CERFFile *)0x0) {
    puVar2 = (undefined4 *)0x0;
  }
  else {
    puVar2 = CERFFile::CERFFile(pCVar1);
  }
  local_4._0_1_ = 0;
  this->field76_0x1e8 = puVar2;
  puVar2 = operator_new(0xc);
  if (puVar2 == (undefined4 *)0x0) {
    puVar2 = (undefined4 *)0x0;
  }
  else {
    puVar2[1] = 0;
    puVar2[2] = 0;
    *puVar2 = 0;
  }
  local_4._0_1_ = 0;
  this->field79_0x1f4 = puVar2;
  CERFFile::Create((CERFFile *)this->field76_0x1e8,param_2);
  CERFFile::SetVersion((CERFFile *)this->field76_0x1e8,"MOD V1.0");
  CERFFile::WriteHeader((CERFFile *)this->field76_0x1e8);
  CERFFile::WriteStringTable((CERFFile *)this->field76_0x1e8);
  this->table_count_ = 3;
  CERFFile::SetNumEntries((CERFFile *)this->field76_0x1e8,3);
  this_00 = operator_new(0xa0);
  local_4._0_1_ = 3;
  if (this_00 == (CResGFF *)0x0) {
    uVar3 = 0;
  }
  else {
    uVar3 = CResGFF::CResGFF(this_00);
  }
  local_4._0_1_ = 0;
  this->field78_0x1f0 = uVar3;
  pvVar4 = operator_new(4);
  this->field77_0x1ec = pvVar4;
  CExoString::CExoString(&local_1c,"V2.0");
  local_4._0_1_ = 4;
  CExoString::CExoString(&local_24,"IFO ");
  local_4._0_1_ = 5;
  iVar5 = CResGFF::CreateGFFFile((CResGFF *)this->field78_0x1f0,(CResStruct *)this->field77_0x1ec,&local_24,&local_1c);
  local_4._0_1_ = 4;
  CExoString::~CExoString(&local_24);
  local_4 = (uint)local_4._1_3_ << 8;
  CExoString::~CExoString(&local_1c);
  if (iVar5 == 0) {
    pCVar1 = (CERFFile *)this->field76_0x1e8;
    if (pCVar1 != (CERFFile *)0x0) {
      CERFFile::~CERFFile(pCVar1);
      _free(pCVar1);
    }
    this->field76_0x1e8 = 0;
    if ((undefined4 *)this->field78_0x1f0 != (undefined4 *)0x0) {
      (*(code *)**(undefined4 **)this->field78_0x1f0)(1);
    }
    this->field78_0x1f0 = 0;
    _free((void *)this->field77_0x1ec);
    this->field77_0x1ec = 0;
  }
  else {
    SerializeIfoGameTime(this,(CResGFF *)this->field78_0x1f0,(CResStruct *)this->field77_0x1ec);
    SaveModuleFAC();
  }
  local_4 = 0xffffffff;
  CExoString::~CExoString(&local_14);
  ExceptionList = pvStack_c;
  return;
}
```

**Callees:** § SerializeIfoGameTime, § SaveModuleFAC.

---

### SaveModuleInProgress (K1 @ 0x004c3b10)

```c
undefined4 __thiscall CSWSModule::SaveModuleInProgress(CSWSModule *this)
{
  CSWSArea *this_00;
  CSWSMessage *this_01;
  ulong uVar1;
  ulong uVar2;
  CExoString local_24;
  CExoString local_1c;
  CExoString local_14;
  void *local_c;
  code *pcStack_8;
  int local_4;

  local_4 = 0xffffffff;
  pcStack_8 = FrameHandler_007193d8;
  local_c = ExceptionList;
  ExceptionList = &local_c;
  CExoString::CExoString(&local_1c);
  local_4 = 0;
  CExoString::CExoString(&local_14);
  local_4._0_1_ = 1;
  CExoString::CExoString(&local_24);
  local_4._0_1_ = 2;
  if (this->field76_0x1e8 == 0) {
    local_4._0_1_ = 1;
    CExoString::~CExoString(&local_24);
    local_4 = (uint)local_4._1_3_ << 8;
    CExoString::~CExoString(&local_14);
    local_4 = 0xffffffff;
    CExoString::~CExoString(&local_1c);
    ExceptionList = local_c;
    return 0;
  }
  CExoString::operator=(&local_14,"tmparea");
  CExoString::operator=(&local_24,"tmpgit");
  this_00 = CServerExoApp::GetAreaByGameObjectID(AppManager->server,this->area_id);
  CExoString::operator=(&local_1c,&local_24);
  CSWSArea::SaveGIT(this_00,(CERFFile *)this->field76_0x1e8,&local_1c,(CExoArrayList *)this->field79_0x1f4);
  AppManager->reentrant_server_stats->field2_0x8 = 1;
  uVar2 = AppManager->reentrant_server_stats->field3_0xc;
  uVar1 = 1;
  this_01 = (CSWSMessage *)CServerExoApp::GetSWSMessage(AppManager->server);
  CSWSMessage::SendServerToPlayerLoadBar_UpdateStallEvent(this_01,uVar1,uVar2);
  local_4._0_1_ = 1;
  CExoString::~CExoString(&local_24);
  local_4 = (uint)local_4._1_3_ << 8;
  CExoString::~CExoString(&local_14);
  local_4 = 0xffffffff;
  CExoString::~CExoString(&local_1c);
  ExceptionList = local_c;
  return 1;
}
```

**Callees:** § SaveGIT (CSWSArea::SaveGIT).

---

### SaveModuleFinish (K1 @ 0x004ca680)

```c
undefined4 __thiscall CSWSModule::SaveModuleFinish(CSWSModule *this,CExoString *param_1,CExoString *param_2)
{
  CERFFile *this_00;
  undefined4 *_Memory;
  void *pvVar1;
  CExoString local_14;
  void *local_c;
  code *pcStack_8;
  undefined4 local_4;

  local_4 = 0xffffffff;
  pcStack_8 = FrameHandler_00719368;
  local_c = ExceptionList;
  if (this->field76_0x1e8 == 0) {
    return 0;
  }
  ExceptionList = &local_c;
  if (this->is_nwm_file == 0) {
    ExceptionList = &local_c;
    CExoString::CExoString(&local_14,"ARE ");
    local_4 = 0;
    SaveStatic(this,(CERFFile *)this->field76_0x1e8,&local_14,0x7dc,1);
    local_4 = 0xffffffff;
    CExoString::~CExoString(&local_14);
  }
  pvVar1 = (void *)this->field79_0x1f4;
  SaveModuleIFOFinish(this,this->field78_0x1f0,this->field77_0x1ec,this->field76_0x1e8,param_1);
  CERFFile::Finish((CERFFile *)this->field76_0x1e8);
  this_00 = (CERFFile *)this->field76_0x1e8;
  if (this_00 != (CERFFile *)0x0) {
    CERFFile::~CERFFile(this_00);
    _free(this_00);
  }
  _Memory = (undefined4 *)this->field79_0x1f4;
  this->field76_0x1e8 = 0;
  if (_Memory != (undefined4 *)0x0) {
    _free((void *)*_Memory);
    *_Memory = 0;
    _free(_Memory);
  }
  this->field79_0x1f4 = 0;
  ExceptionList = pvVar1;
  return 1;
}
```

**Callees:** § SaveStatic, § SaveModuleIFOFinish.

---

---

### SerializeIfoGameTime (K1 @ 0x004c7050)

**Callees:** § SaveLimboCreatures; CSWSScriptVarTable::SaveVarTable, CSWVarTable::SaveVarTable, CServerAIMaster::SaveEventQueue (engine/script).

```c
undefined4 __thiscall CSWSModule::SerializeIfoGameTime(CSWSModule *this,CResGFF *param_1,CResStruct *param_2)
{
  ulong value;
  ulong value_00;
  CGameObjectArray *pCVar1;
  CResRef *pCVar2;
  CWorldTimer *pCVar3;
  CExoLocString *value_01;
  CSWSArea *pCVar4;
  CTlkTable *pCVar5;
  CServerAIMaster *this_00;
  int iVar6;
  undefined4 *puVar7;
  ushort *puVar8;
  undefined1 uVar9;
  undefined1 uVar10;
  undefined2 uVar11;
  ulong *puVar12;
  CResStruct *pCVar13;
  CResStruct local_68;
  CExoLinkedListNode *local_64;
  ulong local_60;
  ushort local_5c [2];
  ushort local_58 [2];
  ushort local_54 [2];
  undefined1 local_50 [4];
  undefined4 local_4c;
  CResRef local_48;
  CExoLocString local_38;
  resref_internal rStack_30;
  CResList local_20;
  void *pvStack_c;
  code *pcStack_8;
  int local_4;

  pCVar13 = param_2;
  local_4 = 0xffffffff;
  pcStack_8 = FrameHandler_00719850;
  pvStack_c = ExceptionList;
  ExceptionList = &pvStack_c;
  CExoLocString::CExoLocString(&local_38);
  local_4 = 0;
  pCVar1 = CServerExoApp::GetObjectArray(AppManager->server);
  CResGFF::WriteFieldVOID(param_1,param_2,this->id_,0x20,"Mod_ID");
  CResGFF::WriteFieldINT(param_1,param_2,*(int *)((int)this->id_ + 0x20),"Mod_Creator_ID");
  CResGFF::WriteFieldDWORD(param_1,param_2,*(ulong *)((int)this->id_ + 0x24),"Mod_Version");
  CResGFF::WriteFieldCExoLocString(param_1,param_2,&this->name_localized,"Mod_Name");
  CResGFF::WriteFieldCExoLocString(param_1,param_2,&this->description,"Mod_Description");
  CResGFF::WriteFieldBYTE(param_1,param_2,*(byte *)&this->is_save_game,"Mod_IsSaveGame");
  CResGFF::WriteFieldBYTE(param_1,param_2,*(byte *)&this->is_nwm_file,"Mod_IsNWMFile");
  if (this->is_nwm_file != 0) {
    CResGFF::WriteFieldCExoString(param_1,param_2,&this->nwm_res_name,"Mod_NWMResName");
  }
  pCVar2 = CResRef::GetResRefStr(&this->field18_0x6c);
  CExoString::CExoString((CExoString *)&local_48,(char *)pCVar2);
  local_4._0_1_ = 1;
  CResGFF::WriteFieldCExoString(param_1,param_2,(CExoString *)&local_48,"Mod_Hak");
  local_4 = (uint)local_4._1_3_ << 8;
  CExoString::~CExoString((CExoString *)&local_48);
  CResGFF::WriteFieldDWORD64(param_1,param_2,CONCAT26((short)((uint)nextID_._4_4_ >> 0x10),CONCAT15((char)((uint)nextID_._4_4_ >> 8),CONCAT14((char)nextID_.s.HighPart,nextID_.s.LowPart))),"Mod_Effect_NxtId");
  CResGFF::WriteFieldDWORD(param_1,param_2,pCVar1->next_char_id_0,"Mod_NextCharId0");
  CResGFF::WriteFieldDWORD(param_1,param_2,pCVar1->next_char_id_1,"Mod_NextCharId1");
  CResGFF::WriteFieldDWORD(param_1,param_2,pCVar1->next_object_id_0,"Mod_NextObjId0");
  CResGFF::WriteFieldDWORD(param_1,param_2,pCVar1->next_object_id_1,"Mod_NextObjId1");
  CResGFF::WriteFieldCExoString(param_1,param_2,&this->tag,"Mod_Tag");
  CResGFF::WriteFieldCResRef(param_1,param_2,&this->entry->area,"Mod_Entry_Area");
  CResGFF::WriteFieldFLOAT(param_1,param_2,this->entry->x,"Mod_Entry_X");
  CResGFF::WriteFieldFLOAT(param_1,param_2,this->entry->y,"Mod_Entry_Y");
  CResGFF::WriteFieldFLOAT(param_1,param_2,this->entry->z,"Mod_Entry_Z");
  CResGFF::WriteFieldFLOAT(param_1,param_2,this->entry->dir_x,"Mod_Entry_Dir_X");
  CResGFF::WriteFieldFLOAT(param_1,param_2,this->entry->dir_y,"Mod_Entry_Dir_Y");
  CResGFF::WriteFieldBYTE(param_1,param_2,this->minutes_per_hour,"Mod_MinPerHour");
  CResGFF::WriteFieldBYTE(param_1,param_2,this->dawn_hour,"Mod_DawnHour");
  CResGFF::WriteFieldBYTE(param_1,param_2,this->dusk_hour,"Mod_DuskHour");
  puVar12 = &local_60;
  uVar9 = SUB41(local_50,0);
  uVar10 = (undefined1)((uint)local_50 >> 8);
  uVar11 = (undefined2)((uint)local_50 >> 0x10);
  pCVar3 = CServerExoApp::GetWorldTimer(AppManager->server);
  CWorldTimer::GetWorldTime(pCVar3,(ulong *)CONCAT22(uVar11,CONCAT11(uVar10,uVar9)),puVar12);
  puVar12 = (ulong *)local_54;
  uVar9 = SUB41(local_58,0);
  uVar10 = (undefined1)((uint)local_58 >> 8);
  uVar11 = (undefined2)((uint)local_58 >> 0x10);
  puVar8 = local_5c;
  puVar7 = &local_4c;
  pCVar3 = CServerExoApp::GetWorldTimer(AppManager->server);
  CWorldTimer::ConvertFromTimeOfDay(pCVar3,local_60,puVar7,(undefined4 *)puVar8,(undefined4 *)CONCAT22(uVar11,CONCAT11(uVar10,uVar9)),puVar12);
  pCVar3 = CServerExoApp::GetWorldTimer(AppManager->server);
  value = pCVar3->world_time_day;
  pCVar3 = CServerExoApp::GetWorldTimer(AppManager->server);
  value_00 = pCVar3->world_time_ms;
  CResGFF::WriteFieldDWORD(param_1,param_2,this->start_year,"Mod_StartYear");
  CResGFF::WriteFieldBYTE(param_1,param_2,*(byte *)&this->current_month,"Mod_StartMonth");
  CResGFF::WriteFieldBYTE(param_1,param_2,*(byte *)&this->current_day,"Mod_StartDay");
  CResGFF::WriteFieldBYTE(param_1,param_2,*(byte *)&this->current_hour,"Mod_StartHour");
  CResGFF::WriteFieldDWORD(param_1,param_2,this->transition,"Mod_Transition");
  CResGFF::WriteFieldWORD(param_1,param_2,local_5c[0],"Mod_StartMinute");
  CResGFF::WriteFieldWORD(param_1,param_2,local_58[0],"Mod_StartSecond");
  CResGFF::WriteFieldWORD(param_1,param_2,local_54[0],"Mod_StartMiliSec");
  CResGFF::WriteFieldDWORD(param_1,param_2,value,"Mod_PauseDay");
  CResGFF::WriteFieldDWORD(param_1,param_2,value_00,"Mod_PauseTime");
  CResGFF::WriteFieldBYTE(param_1,param_2,this->xp_scale,"Mod_XPScale");
  uVar9 = SUB41(param_1,0);
  uVar10 = (undefined1)((uint)param_1 >> 8);
  uVar11 = (undefined2)((uint)param_1 >> 0x10);
  SaveLimboCreatures(this,uVar9,param_2);
  CResGFF::AddList(param_1,&local_20,param_2,"Mod_Expan_List");
  local_64 = *(CExoLinkedListNode **)this->expansion_list;
  while (local_64 != (CExoLinkedListNode *)0x0) {
    value_01 = (CExoLocString *)CExoLinkedListInternal::GetAtPos((CExoLinkedListInternal *)this->expansion_list,local_64);
    CResGFF::AddListElement(param_1,&local_68,&local_20,0);
    CResGFF::WriteFieldCExoLocString(param_1,&local_68,value_01,"Expansion_Name");
    CResGFF::WriteFieldINT(param_1,&local_68,(int)value_01[1].internal,"Expansion_ID");
    CExoLinkedListInternal::GetNext((CExoLinkedListInternal *)this->expansion_list,&local_64);
  }
  CResGFF::AddList(param_1,&local_20,param_2,"Mod_CutSceneList");
  local_64 = *(CExoLinkedListNode **)this->cutscene_list;
  while (local_64 != (CExoLinkedListNode *)0x0) {
    pCVar2 = (CResRef *)CExoLinkedListInternal::GetAtPos((CExoLinkedListInternal *)this->cutscene_list,local_64);
    CResGFF::AddListElement(param_1,&local_68,&local_20,1);
    CResGFF::WriteFieldCResRef(param_1,&local_68,pCVar2,"CutScene_Name");
    CResGFF::WriteFieldDWORD(param_1,&local_68,pCVar2[1].internal.ints[0],"CutScene_ID");
    CExoLinkedListInternal::GetNext((CExoLinkedListInternal *)this->cutscene_list,&local_64);
  }
  CResRef::CResRef(&local_48,this->scripts);
  CResGFF::WriteFieldCResRef(param_1,param_2,&local_48,"Mod_OnHeartbeat");
  CResRef::CResRef(&local_48,this->scripts + 1);
  CResGFF::WriteFieldCResRef(param_1,param_2,&local_48,"Mod_OnUsrDefined");
  CResRef::CResRef(&local_48,this->scripts + 4);
  CResGFF::WriteFieldCResRef(param_1,param_2,&local_48,"Mod_OnClientEntr");
  CResRef::CResRef(&local_48,this->scripts + 5);
  CResGFF::WriteFieldCResRef(param_1,param_2,&local_48,"Mod_OnClientLeav");
  CResRef::CResRef(&local_48,this->scripts + 6);
  CResGFF::WriteFieldCResRef(param_1,param_2,&local_48,"Mod_OnActvtItem");
  CResRef::CResRef(&local_48,this->scripts + 7);
  CResGFF::WriteFieldCResRef(param_1,param_2,&local_48,"Mod_OnAcquirItem");
  CResRef::CResRef(&local_48,this->scripts + 8);
  CResGFF::WriteFieldCResRef(param_1,param_2,&local_48,"Mod_OnUnAqreItem");
  CResRef::CResRef(&local_48,this->scripts + 2);
  CResGFF::WriteFieldCResRef(param_1,param_2,&local_48,"Mod_OnModLoad");
  CResRef::CResRef(&local_48,this->scripts + 3);
  CResGFF::WriteFieldCResRef(param_1,param_2,&local_48,"Mod_OnModStart");
  CResRef::CResRef(&local_48,this->scripts + 9);
  CResGFF::WriteFieldCResRef(param_1,param_2,&local_48,"Mod_OnPlrDeath");
  CResRef::CResRef(&local_48,this->scripts + 10);
  CResGFF::WriteFieldCResRef(param_1,param_2,&local_48,"Mod_OnPlrDying");
  CResRef::CResRef(&local_48,this->scripts + 0xb);
  CResGFF::WriteFieldCResRef(param_1,param_2,&local_48,"Mod_OnSpawnBtnDn");
  CResRef::CResRef(&local_48,this->scripts + 0xc);
  CResGFF::WriteFieldCResRef(param_1,param_2,&local_48,"Mod_OnPlrRest");
  CResRef::CResRef(&local_48,this->scripts + 0xd);
  CResGFF::WriteFieldCResRef(param_1,param_2,&local_48,"Mod_OnPlrLvlUp");
  CResGFF::AddList(param_1,&local_20,param_2,"Mod_Area_list");
  pCVar4 = CServerExoApp::GetAreaByGameObjectID(AppManager->server,this->area_id);
  rStack_30.ints[0] = (pCVar4->res_helper).resref.internal.ints[0];
  rStack_30.ints[1] = (pCVar4->res_helper).resref.internal.ints[1];
  rStack_30.ints[2] = (pCVar4->res_helper).resref.internal.ints[2];
  rStack_30.ints[3] = (pCVar4->res_helper).resref.internal.ints[3];
  CResGFF::AddListElement(param_1,&local_68,&local_20,6);
  CResGFF::WriteFieldCResRef(param_1,&local_68,(CResRef *)&rStack_30,"Area_Name");
  CResGFF::WriteFieldDWORD(param_1,&local_68,this->area_id,"ObjectId");
  AppManager->reentrant_server_stats->field3_0xc = 1;
  CResGFF::AddList(param_1,&local_20,param_2,"Mod_Tokens");
  iVar6 = 0;
  param_2 = (CResStruct *)0x0;
  pCVar5 = TlkTable;
  if ((TlkTable->custom_tokens).size != 0) {
    do {
      if (DAT_0073eccc < *(uint *)(iVar6 + (int)(pCVar5->custom_tokens).data)) {
        CResGFF::AddListElement(param_1,&local_68,&local_20,7);
        CResGFF::WriteFieldDWORD(param_1,&local_68,*(ulong *)(iVar6 + (int)(TlkTable->custom_tokens).data),"Mod_TokensNumber");
        CResGFF::WriteFieldCExoString(param_1,&local_68,(CExoString *)((int)(TlkTable->custom_tokens).data + iVar6 + 4),"Mod_TokensValue");
        pCVar5 = TlkTable;
      }
      param_2 = (CResStruct *)((int)&param_2->index + 1);
      iVar6 = iVar6 + 0xc;
    } while (param_2 < (CResStruct *)(pCVar5->custom_tokens).size);
  }
  CSWSScriptVarTable::SaveVarTable((CSWSScriptVarTable *)&this->script_var_table,param_1,pCVar13);
  CSWVarTable::SaveVarTable(&this->var_table,(int **)param_1,&pCVar13->index);
  this_00 = (CServerAIMaster *)CServerExoApp::GetServerAIMaster(AppManager->server);
  CServerAIMaster::SaveEventQueue(this_00,(CResGFF *)CONCAT22(uVar11,CONCAT11(uVar10,uVar9)),pCVar13);
  local_4 = 0xffffffff;
  CExoLocString::~CExoLocString(&local_38);
  ExceptionList = pvStack_c;
  return 1;
}
```

---

### SaveLimboCreatures (K1 @ 0x004c5bb0)

**Callees:** § SerializeCreature_K2 (CSWSCreature::SerializeCreature_K2).

```c
void __thiscall CSWSModule::SaveLimboCreatures(CSWSModule *this,CResGFF *param_2,CResStruct *param_3)
{
  bool bVar1;
  CSWSCreature *this_00;
  int iVar2;
  CResStruct CStack_20;
  CGameObjectArray *local_1c;
  CGameObject *local_18;
  CResList local_14;

  local_1c = CServerExoApp::GetObjectArray(AppManager->server);
  CResGFF::AddList(param_2,&local_14,param_3,"Creature List");
  iVar2 = 0;
  if (0 < (int)(this->limbo_creature_list).size) {
    do {
      bVar1 = CGameObjectArray::GetGameObject(local_1c,(this->limbo_creature_list).data[iVar2],&local_18);
      if (bVar1 == bool_false) {
        this_00 = (*local_18->vtable->AsSWSCreature)();
        CResGFF::AddListElement(param_2,&CStack_20,&local_14,4);
        CResGFF::WriteFieldDWORD(param_2,&CStack_20,(this_00->object).game_object.id,"ObjectId");
        CSWSCreature::SerializeCreature_K2(this_00,param_2,(int)&CStack_20);
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < (int)(this->limbo_creature_list).size);
  }
  return;
}
```

---

### SaveModuleFAC (K1 @ 0x004c3960)

**Callees:** CFactionManager::SaveFactions, CFactionManager::SaveReputations (engine); CResGFF::WriteGFFFile(..., FAC).

```c
int CSWSModule::SaveModuleFAC(void)
{
  CFactionManager *this;
  CResGFF *this_00;
  CResStruct *struct;
  int iVar1;
  char *copy_string;
  CExoString local_38;
  CExoString local_30;
  CExoString local_28;
  CResList local_20;
  void *pvStack_c;
  code *pcStack_8;
  undefined4 local_4;

  local_4 = 0xffffffff;
  pcStack_8 = FrameHandler_007193ab;
  pvStack_c = ExceptionList;
  ExceptionList = &pvStack_c;
  CExoString::CExoString(&local_30);
  this = AppManager->server->internal->faction_manager;
  local_4 = 0;
  local_38.c_string = operator_new(0xa0);
  local_4._0_1_ = 1;
  if ((CResGFF *)local_38.c_string == (CResGFF *)0x0) {
    this_00 = (CResGFF *)0x0;
  }
  else {
    this_00 = (CResGFF *)CResGFF::CResGFF((CResGFF *)local_38.c_string);
  }
  local_4._0_1_ = 0;
  struct = operator_new(4);
  CExoString::CExoString(&local_28,"V2.0");
  local_4._0_1_ = 2;
  CExoString::CExoString(&local_38,"FAC ");
  local_4._0_1_ = 3;
  iVar1 = CResGFF::CreateGFFFile(this_00,struct,&local_38,&local_28);
  local_4._0_1_ = 2;
  CExoString::~CExoString(&local_38);
  local_4._0_1_ = 0;
  CExoString::~CExoString(&local_28);
  if (iVar1 == 1) {
    CResGFF::AddList(this_00,&local_20,struct,"FactionList");
    CFactionManager::SaveFactions(this,this_00,&local_20);
    CResGFF::AddList(this_00,&local_20,struct,"RepList");
    CFactionManager::SaveReputations(this,this_00,&local_20);
    CExoString::operator=(&local_30,"GAMEINPROGRESS:REPUTE");
    copy_string = CExoString::CStr(&local_30);
    CExoString::CExoString(&local_28,copy_string);
    local_4._0_1_ = 4;
    CResGFF::WriteGFFFile(this_00,&local_28,FAC);
    local_4._0_1_ = 0;
    CExoString::~CExoString(&local_28);
    if (this_00 != (CResGFF *)0x0) {
      (**(code **)(this_00->resource).vtable)(1);
    }
  }
  else if (this_00 != (CResGFF *)0x0) {
    (**(code **)(this_00->resource).vtable)(1);
  }
  _free(struct);
  local_4 = 0xffffffff;
  CExoString::~CExoString(&local_30);
  ExceptionList = pvStack_c;
  return iVar1;
}
```

---

### SaveStatic (K1 @ 0x004c5980) — partial (full body: get ARE list from Module 0x7de, for each resref write into ERF with type param_3 0x7dc)

Gets ARE list from module context (res type 0x7de "Module"), iterates each ARE resref, and for each writes the resource into the ERF (param_1) with type param_3 (0x7dc). If param_4 != 0, loads GFF from resman or creates new CResGFF and writes it as the ARE into the ERF. Engine: CExoResMan::GetResOfType, CExoResMan::GetResObject, CERFFile::WriteResource.

---

### SaveModuleIFOFinish (K1 @ 0x004c8b90)

**Callees:** § SavePlayers.

```c
undefined4 __thiscall CSWSModule::SaveModuleIFOFinish(CSWSModule *this,CResGFF *param_2,CResStruct *param_3,CERFFile *param_4,CExoString *param_5,CExoArrayList<uint> *param_6)
{
  CExoString local_24;
  CExoString local_1c;
  CExoString local_14;
  void *pvStack_c;
  code *pcStack_8;
  int local_4;

  local_4 = 0xffffffff;
  pcStack_8 = FrameHandler_00719a68;
  pvStack_c = ExceptionList;
  ExceptionList = &pvStack_c;
  CExoString::CExoString(&local_14);
  local_4 = 0;
  CExoString::CExoString(&local_1c);
  local_4._0_1_ = 1;
  CExoString::CExoString(&local_24);
  local_4 = CONCAT31(local_4._1_3_,2);
  SavePlayers(this,param_2,param_3,param_5,param_6);
  CERFFile::WriteResource(param_4,"Module",0x7de,&param_2->resource,1,(void *)0xffffffff);
  if (param_2 != (CResGFF *)0x0) {
    (**(code **)(param_2->resource).vtable)(1);
  }
  _free(param_3);
  local_4._0_1_ = 1;
  CExoString::~CExoString(&local_24);
  local_4 = (uint)local_4._1_3_ << 8;
  CExoString::~CExoString(&local_1c);
  local_4 = 0xffffffff;
  CExoString::~CExoString(&local_14);
  ExceptionList = pvStack_c;
  return 1;
}
```

---

### SavePlayers (K1 @ 0x004c7870)

**Callees:** § SerializeCreature_K2 (CSWSCreature::SerializeCreature_K2); CSWSPlayer::SaveServerCharacter, LoadCharacterFromIFO (engine).

AddList `"Mod_PlayerList"`, struct ID 0xbead; per player: Mod_CommntyName, Mod_IsPrimaryPlr, Mod_FirstName, Mod_LastName, ObjectId, SerializeCreature_K2. Also merges in party members from module IFO (LoadCharacterFromIFO path) and writes them into the same list.

```c
// Full 215-line decompilation omitted here for length; see Agentdecompile get-functions 0x004c7870 limit 250.
// Key: AddList "Mod_PlayerList"; AddListElement(..., 0xbead); WriteFieldCExoString Mod_CommntyName;
// WriteFieldBYTE Mod_IsPrimaryPlr; WriteFieldCExoLocString Mod_FirstName/Mod_LastName;
// WriteFieldDWORD ObjectId; CSWSCreature::SerializeCreature_K2(pCVar3,this_01,&CStack_78);
```

---

### SaveGIT (CSWSArea::SaveGIT) (K1 @ 0x0050ba00)

**Callees:** § SaveCreatures, § SaveItems, § SaveDoors, § SaveTriggers, § SaveEncounters, § SaveWaypoints, § SaveSounds, § SavePlaceables, § SaveStores, § SaveAreaEffects, § SaveProperties, § SaveMaps, § SavePlaceableCameras; CSWSScriptVarTable::SaveVarTable, CSWVarTable::SaveVarTable.

Creates GIT GFF "GIT "/"V2.0", classifies all area game objects into per-type lists, writes area vars and weather/transition bytes, then calls each Save* in order. Writes GIT as ERF resource with resref = area resref, type 0x7e7.

```c
// Full 297-line decompilation: see Agentdecompile get-functions 0x0050ba00 limit 320.
// Summary: CreateGFFFile(local_ac,struct,"GIT ","V2.0"); SaveVarTable x2; WriteFieldBYTE CurrentWeather, WeatherStarted, TransPending, TransPendNextID, TransPendCurrID;
// SaveCreatures(this,this_00,struct,&local_90); SaveItems; SaveDoors; SaveTriggers; SaveEncounters; SaveWaypoints; SaveSounds; SavePlaceables; SaveStores; SaveAreaEffects;
// SaveProperties; SaveMaps; SavePlaceableCameras; CERFFile::WriteResource(param_1,resref,0x7e7,...);
```

---

---

## GIT entity and area Save* (full C/C++)

### SaveCreatures (K1 @ 0x00507680)

**Callees:** § SerializeCreature_K2.

```c
void __thiscall CSWSArea::SaveCreatures(CSWSArea *this,CResGFF *param_1,CResStruct *param_2,CExoArrayList *param_3)
{
  bool bVar1;
  CSWSCreature *this_00;
  int iVar2;
  CResStruct CStack_20;
  CGameObjectArray *local_1c;
  CGameObject *local_18;
  CResList local_14;

  local_1c = CServerExoApp::GetObjectArray(AppManager->server);
  CServerExoApp::GetPartyTable(AppManager->server);
  CResGFF::AddList(param_1,&local_14,param_2,"Creature List");
  iVar2 = 0;
  if (0 < param_3->size) {
    do {
      bVar1 = CGameObjectArray::GetGameObject(local_1c,param_3->data[iVar2],&local_18);
      if ((bVar1 == bool_false) &&
         (this_00 = (*local_18->vtable->AsSWSCreature)(), this_00->field430_0xa88 == 0)) {
        CResGFF::AddListElement(param_1,&CStack_20,&local_14,4);
        CResGFF::WriteFieldDWORD(param_1,&CStack_20,(this_00->object).game_object.id,"ObjectId");
        CSWSCreature::SerializeCreature_K2(this_00,param_1,(int)&CStack_20);
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < param_3->size);
  }
  return;
}
```

---

### SaveItems (K1 @ 0x00507750)

**Callees:** CSWSItem::SaveItem, CSWSObject::SaveObjectState.

```c
void __thiscall CSWSArea::SaveItems(undefined4 param_1,CResGFF *param_2,CResStruct *param_3,int *param_4)
{
  bool bVar1;
  CSWSItem *this;
  int iVar2;
  CResStruct CStack_20;
  CGameObjectArray *local_1c;
  CGameObject *local_18;
  CResList local_14;

  local_1c = CServerExoApp::GetObjectArray(AppManager->server);
  CResGFF::AddList(param_2,&local_14,param_3,"List");
  iVar2 = 0;
  if (0 < param_4[1]) {
    do {
      bVar1 = CGameObjectArray::GetGameObject(local_1c,*(ulong *)(*param_4 + iVar2 * 4),&local_18);
      if (bVar1 == bool_false) {
        this = (CSWSItem *)(*(code *)local_18->vtable->AsSWSItem)();
        CResGFF::AddListElement(param_2,&CStack_20,&local_14,0);
        CResGFF::WriteFieldDWORD(param_2,&CStack_20,(this->server_object).game_object.id,"ObjectId");
        CSWSItem::SaveItem(this,param_2,&CStack_20);
        CSWSObject::SaveObjectState(&this->server_object,(int **)param_2,&CStack_20.index);
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < param_4[1]);
  }
  return;
}
```

---

### SaveDoors (K1 @ 0x00507810)

**Callees:** CSWSDoor::SaveDoor, CSWSObject::SaveObjectState.

```c
void __thiscall CSWSArea::SaveDoors(undefined4 param_1,CResGFF *param_2,CResStruct *param_3,int *param_4)
{
  bool bVar1;
  CSWSDoor *this;
  int iVar2;
  CResStruct CStack_20;
  CGameObjectArray *local_1c;
  CGameObject *local_18;
  CResList local_14;

  local_1c = CServerExoApp::GetObjectArray(AppManager->server);
  CResGFF::AddList(param_2,&local_14,param_3,"Door List");
  iVar2 = 0;
  if (0 < param_4[1]) {
    do {
      bVar1 = CGameObjectArray::GetGameObject(local_1c,*(ulong *)(*param_4 + iVar2 * 4),&local_18);
      if (bVar1 == bool_false) {
        this = (CSWSDoor *)(*(code *)local_18->vtable->AsSWSDoor)();
        CResGFF::AddListElement(param_2,&CStack_20,&local_14,8);
        CResGFF::WriteFieldDWORD(param_2,&CStack_20,(this->object).game_object.id,"ObjectId");
        CSWSDoor::SaveDoor(this,param_2,&CStack_20);
        CSWSObject::SaveObjectState((CSWSObject *)this,(int **)param_2,&CStack_20.index);
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < param_4[1]);
  }
  return;
}
```

---

### SaveTriggers (K1 @ 0x005078d0)

**Callees:** CSWSTrigger::SaveTrigger, CSWSObject::SaveObjectState.

```c
void __thiscall CSWSArea::SaveTriggers(CSWSArea *this,CResGFF *param_1,CResStruct *param_2,CExoArrayList<uint> *param_3)
{
  bool bVar1;
  CSWSTrigger *this_00;
  int iVar2;
  CResStruct CStack_20;
  CGameObjectArray *local_1c;
  CGameObject *local_18;
  CResList local_14;

  local_1c = CServerExoApp::GetObjectArray(AppManager->server);
  CResGFF::AddList(param_1,&local_14,param_2,"TriggerList");
  iVar2 = 0;
  if (0 < (int)param_3->size) {
    do {
      bVar1 = CGameObjectArray::GetGameObject(local_1c,param_3->data[iVar2],&local_18);
      if (bVar1 == bool_false) {
        this_00 = (*local_18->vtable->AsSWSTrigger)();
        CResGFF::AddListElement(param_1,&CStack_20,&local_14,1);
        CResGFF::WriteFieldDWORD(param_1,&CStack_20,(this_00->object).game_object.id,"ObjectId");
        CSWSTrigger::SaveTrigger(this_00,param_1,&CStack_20);
        CSWSObject::SaveObjectState(&this_00->object,(int **)param_1,&CStack_20.index);
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < (int)param_3->size);
  }
  return;
}
```

---

### SaveEncounters (K1 @ 0x00507990)

**Callees:** CSWSEncounter::SaveEncounter, CSWSObject::SaveObjectState.

```c
void __thiscall CSWSArea::SaveEncounters(CSWSArea *this,CResGFF *param_1,CResStruct *param_2,CExoArrayList<uint> *param_3)
{
  bool bVar1;
  CSWSEncounter *this_00;
  int iVar2;
  CResStruct CStack_20;
  CGameObjectArray *local_1c;
  CGameObject *local_18;
  CResList local_14;

  local_1c = CServerExoApp::GetObjectArray(AppManager->server);
  CResGFF::AddList(param_1,&local_14,param_2,"Encounter List");
  iVar2 = 0;
  if (0 < (int)param_3->size) {
    do {
      bVar1 = CGameObjectArray::GetGameObject(local_1c,param_3->data[iVar2],&local_18);
      if (bVar1 == bool_false) {
        this_00 = (CSWSEncounter *)(*(code *)local_18->vtable->AsSWSEncounter)();
        CResGFF::AddListElement(param_1,&CStack_20,&local_14,7);
        CResGFF::WriteFieldDWORD(param_1,&CStack_20,(this_00->object).game_object.id,"ObjectId");
        CSWSEncounter::SaveEncounter(this_00,param_1,&CStack_20);
        CSWSObject::SaveObjectState((CSWSObject *)this_00,(int **)param_1,&CStack_20.index);
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < (int)param_3->size);
  }
  return;
}
```

---

### SaveWaypoints (K1 @ 0x00507a50)

**Callees:** CSWSWaypoint::SaveWaypoint, CSWSObject::SaveObjectState.

```c
void __thiscall CSWSArea::SaveWaypoints(undefined4 param_1,CResGFF *param_2,CResStruct *param_3,int *param_4)
{
  bool bVar1;
  CSWSWaypoint *this;
  int iVar2;
  CResStruct CStack_20;
  CGameObjectArray *local_1c;
  CGameObject *local_18;
  CResList local_14;

  local_1c = CServerExoApp::GetObjectArray(AppManager->server);
  CResGFF::AddList(param_2,&local_14,param_3,"WaypointList");
  iVar2 = 0;
  if (0 < param_4[1]) {
    do {
      bVar1 = CGameObjectArray::GetGameObject(local_1c,*(ulong *)(*param_4 + iVar2 * 4),&local_18);
      if (bVar1 == bool_false) {
        this = (CSWSWaypoint *)(*(code *)local_18->vtable->AsSWSWaypoint)();
        CResGFF::AddListElement(param_2,&CStack_20,&local_14,5);
        CResGFF::WriteFieldDWORD(param_2,&CStack_20,(this->object).game_object.id,"ObjectId");
        CSWSWaypoint::SaveWaypoint(this,param_2,&CStack_20);
        CSWSObject::SaveObjectState((CSWSObject *)this,(int **)param_2,&CStack_20.index);
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < param_4[1]);
  }
  return;
}
```

---

### SaveSounds (K1 @ 0x00507b10)

**Callees:** CSWSSoundObject::Save, CSWSObject::SaveObjectState.

```c
void __thiscall CSWSArea::SaveSounds(undefined4 param_1,CResGFF *param_2,CResStruct *param_3,int *param_4)
{
  bool bVar1;
  CSWSSoundObject *this;
  int iVar2;
  CResStruct CStack_20;
  CGameObjectArray *local_1c;
  CGameObject *local_18;
  CResList local_14;

  local_1c = CServerExoApp::GetObjectArray(AppManager->server);
  CResGFF::AddList(param_2,&local_14,param_3,"SoundList");
  iVar2 = 0;
  if (0 < param_4[1]) {
    do {
      bVar1 = CGameObjectArray::GetGameObject(local_1c,*(ulong *)(*param_4 + iVar2 * 4),&local_18);
      if (bVar1 == bool_false) {
        this = (CSWSSoundObject *)(*(code *)local_18->vtable->AsSWSSoundObject)();
        CResGFF::AddListElement(param_2,&CStack_20,&local_14,6);
        CResGFF::WriteFieldDWORD(param_2,&CStack_20,(this->object).game_object.id,"ObjectId");
        CSWSSoundObject::Save(this,param_2,&CStack_20);
        CSWSObject::SaveObjectState((CSWSObject *)this,(int **)param_2,&CStack_20.index);
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < param_4[1]);
  }
  return;
}
```

---

### SavePlaceables (K1 @ 0x00507bd0)

**Callees:** CSWSPlaceable::SavePlaceable, CSWSObject::SaveObjectState.

```c
void __thiscall CSWSArea::SavePlaceables(undefined4 param_1,CResGFF *param_2,CResStruct *param_3,int *param_4)
{
  bool bVar1;
  CSWSPlaceable *this;
  int iVar2;
  CResStruct CStack_20;
  CGameObjectArray *local_1c;
  CGameObject *local_18;
  CResList local_14;

  local_1c = CServerExoApp::GetObjectArray(AppManager->server);
  CResGFF::AddList(param_2,&local_14,param_3,"Placeable List");
  iVar2 = 0;
  if (0 < param_4[1]) {
    do {
      bVar1 = CGameObjectArray::GetGameObject(local_1c,*(ulong *)(*param_4 + iVar2 * 4),&local_18);
      if ((bVar1 == bool_false) &&
         (this = (*local_18->vtable->AsSWSPlaceable)(), this->is_corpse == 0)) {
        CResGFF::AddListElement(param_2,&CStack_20,&local_14,9);
        CResGFF::WriteFieldDWORD(param_2,&CStack_20,(this->object).game_object.id,"ObjectId");
        CSWSPlaceable::SavePlaceable(this,param_2,&CStack_20);
        CSWSObject::SaveObjectState(&this->object,(int **)param_2,&CStack_20.index);
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < param_4[1]);
  }
  return;
}
```

---

### SaveStores (K1 @ 0x00507ca0)

**Callees:** CSWSStore::SaveStore, CSWSObject::SaveObjectState.

```c
void __thiscall CSWSArea::SaveStores(CSWSArea *this,CResGFF *param_1,CResStruct *param_2,CExoArrayList<uint> *param_3)
{
  bool bVar1;
  CSWSStore *this_00;
  int iVar2;
  CResStruct CStack_20;
  CGameObjectArray *local_1c;
  CGameObject *local_18;
  CResList local_14;

  local_1c = CServerExoApp::GetObjectArray(AppManager->server);
  CResGFF::AddList(param_1,&local_14,param_2,"StoreList");
  iVar2 = 0;
  if (0 < (int)param_3->size) {
    do {
      bVar1 = CGameObjectArray::GetGameObject(local_1c,param_3->data[iVar2],&local_18);
      if (bVar1 == bool_false) {
        this_00 = (CSWSStore *)(*(code *)local_18->vtable->AsSWSStore)();
        CResGFF::AddListElement(param_1,&CStack_20,&local_14,0xb);
        CResGFF::WriteFieldDWORD(param_1,&CStack_20,(this_00->object).game_object.id,"ObjectId");
        CSWSStore::SaveStore(this_00,param_1,&CStack_20);
        CSWSObject::SaveObjectState((CSWSObject *)this_00,(int **)param_1,&CStack_20.index);
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < (int)param_3->size);
  }
  return;
}
```

---

### SaveAreaEffects (K1 @ 0x00507d60)

**Callees:** CSWSAreaOfEffectObject::SaveEffect, CSWSObject::SaveObjectState.

```c
void __thiscall CSWSArea::SaveAreaEffects(CSWSArea *this,CResGFF *param_1,CResStruct *param_2,CExoArrayList<uint> *param_3)
{
  bool bVar1;
  CSWSAreaOfEffectObject *this_00;
  int iVar2;
  CResStruct CStack_20;
  CGameObjectArray *local_1c;
  CGameObject *local_18;
  CResList local_14;

  local_1c = CServerExoApp::GetObjectArray(AppManager->server);
  CResGFF::AddList(param_1,&local_14,param_2,"AreaEffectList");
  iVar2 = 0;
  if (0 < (int)param_3->size) {
    do {
      bVar1 = CGameObjectArray::GetGameObject(local_1c,param_3->data[iVar2],&local_18);
      if (bVar1 == bool_false) {
        this_00 = (CSWSAreaOfEffectObject *)(*(code *)local_18->vtable->AsSWSAreaOfEffectObjec)();
        CResGFF::AddListElement(param_1,&CStack_20,&local_14,0xd);
        CResGFF::WriteFieldDWORD(param_1,&CStack_20,(this_00->object).game_object.id,"ObjectId");
        CSWSAreaOfEffectObject::SaveEffect(this_00,param_1,&CStack_20);
        CSWSObject::SaveObjectState((CSWSObject *)this_00,(int **)param_1,&CStack_20.index);
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < (int)param_3->size);
  }
  return;
}
```

---

### SaveProperties (K1 @ 0x00506090)

**Callees:** CSWSAmbientSound::Save (engine). Struct "AreaProperties" ID 100 (0x64).

```c
void __thiscall CSWSArea::SaveProperties(CSWSArea *this,CResGFF *gff,CResStruct *struct)
{
  CResGFF::AddStructToStruct(gff,(CResStruct *)&struct,struct,"AreaProperties",100);
  CSWSAmbientSound::Save(this->ambient_sounds,gff,(CResStruct *)&struct);
  CResGFF::WriteFieldBYTE(gff,(CResStruct *)&struct,(byte)this->unescapable,"Unescapable");
  CResGFF::WriteFieldBYTE(gff,(CResStruct *)&struct,(byte)this->restrict_mode,"RestrictMode");
  CResGFF::WriteFieldDWORD(gff,(CResStruct *)&struct,this->stealth_xp_max,"StealthXPMax");
  CResGFF::WriteFieldDWORD(gff,(CResStruct *)&struct,this->stealth_xp_current,"StealthXPCurrent");
  CResGFF::WriteFieldDWORD(gff,(CResStruct *)&struct,this->stealth_xp_loss,"StealthXPLoss");
  CResGFF::WriteFieldBYTE(gff,(CResStruct *)&struct,(byte)this->stealth_xp_enabled,"StealthXPEnabled");
  CResGFF::WriteFieldBYTE(gff,(CResStruct *)&struct,(byte)this->trans_pending,"TransPending");
  CResGFF::WriteFieldBYTE(gff,(CResStruct *)&struct,this->next_transition_pending_id,"TransPendNextID");
  CResGFF::WriteFieldBYTE(gff,(CResStruct *)&struct,this->trans_pend_curr_id,"TransPendCurrID");
  CResGFF::WriteFieldDWORD(gff,(CResStruct *)&struct,(this->sw_area).sun_fog_color,"SunFogColor");
  return;
}
```

---

### SaveMaps (K1 @ 0x005061d0)

**Callees:** CSWSAreaMap::GetMapData (engine). Struct "AreaMap" ID 0x65. Fields: AreaMapResX, AreaMapResY, AreaMapDataSize, AreaMapData.

```c
void __thiscall CSWSArea::SaveMaps(CSWSArea *this,CResGFF *param_1,CResStruct *param_2)
{
  CSWSAreaMap *this_00;
  CSWSModule *pCVar1;
  void *data;
  ulong value;
  CResStruct local_10;
  int local_c;
  ulong local_8;
  int local_4;

  pCVar1 = CServerExoApp::GetModule(AppManager->server);
  if (((pCVar1 != (CSWSModule *)0x0) &&
      (this_00 = pCVar1->field86_0x218, this_00 != (CSWSAreaMap *)0x0)) &&
     (this_00->field1_0x4 != 0)) {
    local_8 = 0;
    local_c = 0;
    data = (void *)CSWSAreaMap::GetMapData(this_00,&local_8,&local_4,&local_c);
    CResGFF::AddStructToStruct(param_1,&local_10,param_2,"AreaMap",0x65);
    CResGFF::WriteFieldINT(param_1,&local_10,local_4,"AreaMapResX");
    CResGFF::WriteFieldINT(param_1,&local_10,local_c,"AreaMapResY");
    value = local_8 << 2;
    CResGFF::WriteFieldDWORD(param_1,&local_10,value,"AreaMapDataSize");
    CResGFF::WriteFieldVOID(param_1,&local_10,data,value,"AreaMapData");
  }
  return;
}
```

---

### SavePlaceableCameras (K1 @ 0x005062a0)

**Callees:** CGuiInGame::GetPlaceableCamera (engine). List "CameraList", struct ID 14 (0xe). Fields: CameraID, Position, Orientation, Pitch, Height, FieldOfView, MicRange.

```c
void __thiscall CSWSArea::SavePlaceableCameras(CSWSArea *this,CResGFF *param_1,CResStruct *param_2)
{
  CGuiInGame *this_00;
  CPlaceableCamera *pCVar1;
  int iVar2;
  CResStruct local_18;
  CResList local_14;

  this_00 = CClientExoApp::GetInGameGui(AppManager->client);
  CResGFF::AddList(param_1,&local_14,param_2,"CameraList");
  iVar2 = 0;
  if (0 < (int)this_00->placeable_camera_count) {
    do {
      pCVar1 = CGuiInGame::GetPlaceableCamera(this_00,iVar2);
      if (pCVar1 != (CPlaceableCamera *)0x0) {
        CResGFF::AddListElement(param_1,&local_18,&local_14,0xe);
        CResGFF::WriteFieldINT(param_1,&local_18,pCVar1->id,"CameraID");
        CResGFF::WriteFieldVector(param_1,&local_18,&pCVar1->position,"Position");
        CResGFF::WriteFieldQuaternion(param_1,&local_18,&pCVar1->orientation,"Orientation");
        CResGFF::WriteFieldFLOAT(param_1,&local_18,pCVar1->pitch,"Pitch");
        CResGFF::WriteFieldFLOAT(param_1,&local_18,pCVar1->height,"Height");
        CResGFF::WriteFieldFLOAT(param_1,&local_18,pCVar1->fov,"FieldOfView");
        CResGFF::WriteFieldFLOAT(param_1,&local_18,pCVar1->mic_range,"MicRange");
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < (int)this_00->placeable_camera_count);
  }
  return;
}
```

---

---

## SerializeCreature_K2 (K1 @ 0x00500610)

**Callees:** CSWSCreatureStats::SaveStats, CSWSItem::SaveItem (equip + item list), CSWSCombatRound::SaveCombatRound, CSWSCreaturePartyFollowInfo::Save, CSWSObject::SaveListenData, CSWSObject::SaveObjectState.

```c
undefined4 __thiscall CSWSCreature::SerializeCreature_K2(CSWSCreature *this,CResGFF *param_1,int param_2)
{
  CSWVisibilityNode *pCVar1;
  CResGFF *this_00;
  int struct;
  CSWSItem *this_01;
  int iVar2;
  CItemRepository *this_02;
  uint uVar3;
  uint item_index;
  CResStruct local_48;
  CResStruct local_44;
  CResStruct local_40;
  int local_3c;
  CResRef local_38;
  CResList local_28;
  CResList local_14;

  this_00 = param_1;
  local_3c = this->field430_0xa88;
  SetInParty(this,0,1);
  struct = param_2;
  CSWSCreatureStats::SaveStats(this->creature_stats,param_1,(CResStruct *)param_2);
  CResGFF::WriteFieldBYTE(param_1,(CResStruct *)struct,this->detect_mode,"DetectMode");
  CResGFF::WriteFieldBYTE(param_1,(CResStruct *)struct,this->stealth_mode,"StealthMode");
  CResGFF::WriteFieldINT(param_1,(CResStruct *)struct,this->creature_size,"CreatureSize");
  CResGFF::WriteFieldBYTE(param_1,(CResStruct *)struct,(byte)(this->object).is_destroyable,"IsDestroyable");
  CResGFF::WriteFieldBYTE(param_1,(CResStruct *)struct,(byte)(this->object).is_raiseable,"IsRaiseable");
  CResGFF::WriteFieldBYTE(param_1,(CResStruct *)struct,(byte)(this->object).dead_selectable,"DeadSelectable");
  CResRef::CResRef(&local_38,this->script_resrefs);
  CResGFF::WriteFieldCResRef(param_1,(CResStruct *)struct,&local_38,"ScriptHeartbeat");
  CResRef::CResRef(&local_38,this->script_resrefs + 1);
  CResGFF::WriteFieldCResRef(param_1,(CResStruct *)struct,&local_38,"ScriptOnNotice");
  CResRef::CResRef(&local_38,this->script_resrefs + 2);
  CResGFF::WriteFieldCResRef(param_1,(CResStruct *)struct,&local_38,"ScriptSpellAt");
  CResRef::CResRef(&local_38,this->script_resrefs + 3);
  CResGFF::WriteFieldCResRef(param_1,(CResStruct *)struct,&local_38,"ScriptAttacked");
  CResRef::CResRef(&local_38,this->script_resrefs + 4);
  CResGFF::WriteFieldCResRef(param_1,(CResStruct *)struct,&local_38,"ScriptDamaged");
  CResRef::CResRef(&local_38,this->script_resrefs + 5);
  CResGFF::WriteFieldCResRef(param_1,(CResStruct *)struct,&local_38,"ScriptDisturbed");
  CResRef::CResRef(&local_38,this->script_resrefs + 6);
  CResGFF::WriteFieldCResRef(param_1,(CResStruct *)struct,&local_38,"ScriptEndRound");
  CResRef::CResRef(&local_38,this->script_resrefs + 7);
  CResGFF::WriteFieldCResRef(param_1,(CResStruct *)struct,&local_38,"ScriptDialogue");
  CResRef::CResRef(&local_38,this->script_resrefs + 8);
  CResGFF::WriteFieldCResRef(param_1,(CResStruct *)struct,&local_38,"ScriptSpawn");
  CResRef::CResRef(&local_38,this->script_resrefs + 9);
  CResGFF::WriteFieldCResRef(param_1,(CResStruct *)struct,&local_38,"ScriptRested");
  CResRef::CResRef(&local_38,this->script_resrefs + 10);
  CResGFF::WriteFieldCResRef(param_1,(CResStruct *)struct,&local_38,"ScriptDeath");
  CResRef::CResRef(&local_38,this->script_resrefs + 0xb);
  CResGFF::WriteFieldCResRef(param_1,(CResStruct *)struct,&local_38,"ScriptUserDefine");
  CResRef::CResRef(&local_38,this->script_resrefs + 0xc);
  CResGFF::WriteFieldCResRef(param_1,(CResStruct *)struct,&local_38,"ScriptOnBlocked");
  CResRef::CResRef(&local_38,this->script_resrefs + 0xd);
  CResGFF::WriteFieldCResRef(param_1,(CResStruct *)struct,&local_38,"ScriptEndDialogue");
  CResGFF::AddList(param_1,&local_28,(CResStruct *)struct,"Equip_ItemList");
  param_2 = 1;
  param_1 = (CResGFF *)0x12;
  do {
    this_01 = (CSWSItem *)CSWInventory::GetItemInSlot(this->inventory,param_2);
    if (this_01 != (CSWSItem *)0x0) {
      CResGFF::AddListElement(this_00,&local_48,&local_28,param_2);
      CResGFF::WriteFieldDWORD(this_00,&local_48,(this_01->server_object).game_object.id,"ObjectId");
      CSWSItem::SaveItem(this_01,this_00,&local_48);
    }
    param_2 = param_2 * 2;
    param_1 = (CResGFF *)((int)&param_1[-1].field33_0x9c + 3);
  } while (param_1 != (CResGFF *)0x0);
  CResGFF::AddList(this_00,&local_28,(CResStruct *)struct,"ItemList");
  uVar3 = 0;
  iVar2 = GetItemRepository(this,1);
  if (*(int *)(iVar2 + 0x10) != 0) {
    do {
      item_index = uVar3;
      this_02 = (CItemRepository *)GetItemRepository(this,1);
      param_2 = CItemRepository::ItemListGetItem(this_02,item_index);
      CResGFF::AddListElement(this_00,&local_48,&local_28,0);
      CResGFF::WriteFieldDWORD(this_00,&local_48,*(ulong *)(param_2 + 0x14),"ObjectId");
      CSWSItem::SaveItem((CSWSItem *)param_2,this_00,&local_48);
      uVar3 = uVar3 + 1;
      iVar2 = GetItemRepository(this,1);
    } while (uVar3 < *(uint *)(iVar2 + 0x10));
  }
  CResGFF::AddList(this_00,&local_14,(CResStruct *)struct,"PerceptionList");
  param_2 = 0;
  if (0 < (this->perceptions).size) {
    do {
      pCVar1 = (this->perceptions).data[param_2];
      if (pCVar1 != (CSWVisibilityNode *)0x0) {
        CResGFF::AddListElement(this_00,&local_44,&local_14,0);
        CResGFF::WriteFieldDWORD(this_00,&local_44,*(ulong *)pCVar1,"ObjectId");
        CResGFF::WriteFieldBYTE(this_00,&local_44,(byte)pCVar1[4] >> 3 & 2 | (byte)pCVar1[4] & 0xf,"PerceptionData");
      }
      param_2 = param_2 + 1;
    } while (param_2 < (this->perceptions).size);
  }
  CResGFF::AddStructToStruct(this_00,&local_40,(CResStruct *)struct,"CombatRoundData",0xcada);
  if (this->combat_round->round_started == 1) {
    CSWSCombatRound::SaveCombatRound(this->combat_round,this_00,&local_40);
  }
  CResGFF::WriteFieldDWORD(this_00,(CResStruct *)struct,(this->object).area_id,"AreaId");
  CResGFF::WriteFieldBYTE(this_00,(CResStruct *)struct,this->ambient_animation_state,"AmbientAnimState");
  CResGFF::WriteFieldINT(this_00,(CResStruct *)struct,(this->object).animation,"Animation");
  CResGFF::WriteFieldBYTE(this_00,(CResStruct *)struct,(byte)this->create_on_script_fired,"CreatnScrptFird");
  CResGFF::WriteFieldBYTE(this_00,(CResStruct *)struct,(byte)this->is_disguised,"PM_IsDisguised");
  if (this->is_disguised == 1) {
    CResGFF::WriteFieldWORD(this_00,(CResStruct *)struct,this->appearance,"PM_Appearance");
  }
  CResGFF::WriteFieldBYTE(this_00,(CResStruct *)struct,(byte)(this->object).listening,"Listening");
  CSWSObject::SaveListenData(&this->object,this_00,(CResStruct *)struct);
  CResGFF::WriteFieldFLOAT(this_00,(CResStruct *)struct,(this->object).position.x,"XPosition");
  CResGFF::WriteFieldFLOAT(this_00,(CResStruct *)struct,(this->object).position.y,"YPosition");
  CResGFF::WriteFieldFLOAT(this_00,(CResStruct *)struct,(this->object).position.z,"ZPosition");
  CResGFF::WriteFieldFLOAT(this_00,(CResStruct *)struct,(this->object).orientation.x,"XOrientation");
  CResGFF::WriteFieldFLOAT(this_00,(CResStruct *)struct,(this->object).orientation.y,"YOrientation");
  CResGFF::WriteFieldFLOAT(this_00,(CResStruct *)struct,(this->object).orientation.z,"ZOrientation");
  CResGFF::WriteFieldINT(this_00,(CResStruct *)struct,this->joining_xp,"JoiningXP");
  SetInParty(this,local_3c,1);
  if (this->party_follow_info != (CSWSCreaturePartyFollowInfo *)0x0) {
    CResGFF::AddStructToStruct(this_00,(CResStruct *)&param_2,(CResStruct *)struct,"FollowInfo",0);
    CSWSCreaturePartyFollowInfo::Save(this->party_follow_info,this_00,&param_2);
  }
  CSWSObject::SaveObjectState(&this->object,(int **)this_00,(ulong *)struct);
  return 1;
}
```

---

## Callees still to expand (save path)

The following are called from the save path and must be decompiled and appended with **full C/C++** for exhaustive documentation. **K1 addresses and Agentdecompile expansion instructions** (get-call-graph, get-functions) are in **[GFF-Save-Load-Exhaustive.md](GFF-Save-Load-Exhaustive.md)** § 2.5 (save callees) and § 3.3 (load callees).

| Callee | Called from |
|--------|-------------|
| **CSWSItem::SaveItem** | SaveItems, SerializeCreature_K2 (Equip_ItemList, ItemList) |
| **CSWSDoor::SaveDoor** | SaveDoors |
| **CSWSObject::SaveObjectState** | SaveItems, SaveDoors, SaveTriggers, SaveEncounters, SaveWaypoints, SaveSounds, SavePlaceables, SaveStores, SaveAreaEffects, SerializeCreature_K2 |
| **CSWSTrigger::SaveTrigger** | SaveTriggers |
| **CSWSEncounter::SaveEncounter** | SaveEncounters |
| **CSWSWaypoint::SaveWaypoint** | SaveWaypoints |
| **CSWSSoundObject::Save** | SaveSounds |
| **CSWSPlaceable::SavePlaceable** | SavePlaceables |
| **CSWSStore::SaveStore** | SaveStores |
| **CSWSAreaOfEffectObject::SaveEffect** | SaveAreaEffects |
| **CSWSCreatureStats::SaveStats** | SerializeCreature_K2 |
| **CSWSCombatRound::SaveCombatRound** | SerializeCreature_K2 |
| **CSWSCreaturePartyFollowInfo::Save** | SerializeCreature_K2 |
| **CSWSObject::SaveListenData** | SerializeCreature_K2 |

**Load path** — Full C/C++ for the top-level load flow is in the "Load path" section below. LoadCreatures @ 0x00504a70, LoadItems @ 0x00504de0, LoadDoors @ 0x0050a0e0, … and their callees: use **[GFF-Save-Load-Exhaustive.md](GFF-Save-Load-Exhaustive.md)** for addresses and Agentdecompile `get-functions` / `get-call-graph` commands to retrieve every loader and sub-callee.

---

## Load path: full C/C++

### LoadModuleInProgress (K1 @ 0x004c5720)

**Callees:** § LoadArea (CSWSArea::LoadArea).

```c
undefined4 __thiscall CSWSModule::LoadModuleInProgress(CSWSModule *this,int param_1)
{
  CSWSArea *this_00;
  int iVar1;
  ulong uVar2;
  CSWSMessage *this_01;
  ulong uVar3;
  CSWSArea *local_10;
  void *local_c;
  code *pcStack_8;
  undefined4 local_4;

  local_4 = 0xffffffff;
  pcStack_8 = FrameHandler_00719656;
  local_c = ExceptionList;
  if (this->is_save_game == 0) {
    ExceptionList = &local_c;
    local_10 = operator_new(0x2d4);
    local_4 = 1;
    if (local_10 != (CSWSArea *)0x0) {
      this_00 = (CSWSArea *)CSWSArea::CSWSArea(local_10,&this->area_name,0,0x7f000000);
      goto LAB_004c57ac;
    }
  }
  else {
    uVar2 = this->area_id;
    ExceptionList = &local_c;
    local_10 = operator_new(0x2d4);
    local_4 = 0;
    if (local_10 != (CSWSArea *)0x0) {
      this_00 = (CSWSArea *)CSWSArea::CSWSArea(local_10,&this->area_name,0,uVar2);
      goto LAB_004c57ac;
    }
  }
  this_00 = (CSWSArea *)0x0;
LAB_004c57ac:
  local_4 = 0xffffffff;
  if (this_00 == (CSWSArea *)0x0) {
    CRes::Release(&this->gff->resource);
    RemoveModuleResources(this,&this->field16_0x5c);
    ExceptionList = local_c;
    return 3;
  }
  iVar1 = CSWSArea::LoadArea(this_00,this->is_save_game);
  if (iVar1 != 0) {
    if (this->is_save_game == 0) {
      this->area_id = (this_00->game_object).id;
    }
    uVar2 = param_1 + 1;
    AppManager->reentrant_server_stats->field2_0x8 = uVar2;
    uVar3 = 0xffffffff;
    this_01 = (CSWSMessage *)CServerExoApp::GetSWSMessage(AppManager->server);
    CSWSMessage::SendServerToPlayerLoadBar_UpdateStallEvent(this_01,uVar2,uVar3);
    ExceptionList = local_c;
    return 0;
  }
  CRes::Release(&this->gff->resource);
  RemoveModuleResources(this,&this->field16_0x5c);
  (**(code **)(this_00->sw_area).vtable)(1);
  ExceptionList = local_10;
  return 4;
}
```

---

### LoadArea (K1 @ 0x0050e190)

**Callees:** LoadAreaHeader, LoadRoomInfo, § LoadGIT, LoadPathPoints (engine/area).

```c
undefined4 __thiscall CSWSArea::LoadArea(CSWSArea *this,int param_2)
{
  int extraout_EAX;
  CSWSModule *this_00;
  undefined2 unaff_SI;
  float10 extraout_ST0;
  longlong lVar1;
  CExoString *pCVar2;
  ulong uVar3;
  CResStruct local_4;

  local_4.index = (ulong)this;
  CRes::Demand(&((this->res_helper).gff)->resource);
  if (extraout_EAX == 0) {
    return 0;
  }
  CResGFF::GetTopLevelStruct((this->res_helper).gff,&local_4);
  LoadAreaHeader(this,&local_4);
  LoadRoomInfo(this,&local_4);
  LoadGIT(this,param_2);
  LoadPathPoints(this,&local_4);
  CRes::Release(&((this->res_helper).gff)->resource);
  uVar3 = (this->game_object).id;
  pCVar2 = &this->tag;
  this_00 = CServerExoApp::GetModule(AppManager->server);
  CSWSModule::AddObjectToLookupTable(this_00,pCVar2,uVar3);
  _ceilf((double)((float)((this->sw_area).bsp_width * (this->sw_area).bsp_height) * FLOAT_0073f400),unaff_SI);
  lVar1 = __ftol2(extraout_ST0);
  this->field57_0x223 = (byte)lVar1;
  return 1;
}
```

---

### LoadGIT (K1 @ 0x0050dd80)

**Callees:** LoadCreatures, LoadItems, LoadDoors, LoadTriggers, LoadEncounters, LoadWaypoints, LoadSounds, LoadPlaceables, LoadStores, LoadAreaEffects, LoadProperties, LoadMaps, LoadPlaceableCameras (each with full decompilation to be appended ).

```c
int __thiscall CSWSArea::LoadGIT(CSWSArea *this,int param_1)
{
  CResRef *templateResRef;
  ulong uVar1;
  byte bVar2;
  int iVar3;
  CResGFF *this_00;
  uint uVar4;
  CResStruct local_18;
  int local_14;
  CResGFF *local_10;
  void *local_c;
  code *pcStack_8;
  undefined4 local_4;

  local_4 = 0xffffffff;
  pcStack_8 = FrameHandler_0071b8eb;
  local_c = ExceptionList;
  templateResRef = &(this->res_helper).resref;
  ExceptionList = &local_c;
  iVar3 = CExoResMan::Exists(ExoResMan,templateResRef,GIT,(ulong *)0x0);
  if (iVar3 != 0) {
    local_10 = operator_new(0xa0);
    local_4 = 0;
    if (local_10 == (CResGFF *)0x0) {
      this_00 = (CResGFF *)0x0;
    }
    else {
      this_00 = CResGFF::CResGFF(local_10,GIT,"GIT ",templateResRef);
    }
    local_4 = 0xffffffff;
    if (this_00->field31_0x94 != 0) {
      CResGFF::GetTopLevelStruct(this_00,&local_18);
      if (param_1 != 0) {
        CSWSScriptVarTable::LoadVarTable((CSWSScriptVarTable *)&this->field43_0x1e4,this_00,&local_18);
        CSWVarTable::LoadVarTable((CSWVarTable *)&this->script_var_table,this_00,&local_18);
        bVar2 = CResGFF::ReadFieldBYTE(this_00,&local_18,"CurrentWeather",&local_14,0);
        *(byte *)&this->current_weather = bVar2;
        bVar2 = CResGFF::ReadFieldBYTE(this_00,&local_18,"WeatherStarted",&local_14,0);
        uVar1 = (this->sw_area).flags;
        this->weather_started = (uint)bVar2;
        if ((uVar1 & 1) != 0) {
          *(undefined1 *)&this->current_weather = 0xff;
          this->weather_started = 0;
        }
      }
      bVar2 = CResGFF::ReadFieldBYTE(this_00,&local_18,"UseTemplates",&local_14,0);
      uVar4 = (uint)bVar2;
      LoadCreatures(this,this_00,&local_18,param_1,uVar4);
      LoadItems(this,this_00,&local_18,param_1,uVar4);
      LoadDoors(this,this_00,&local_18,param_1,uVar4);
      LoadTriggers(this,this_00,&local_18,param_1,uVar4);
      LoadEncounters(this,this_00,&local_18,param_1,uVar4);
      LoadWaypoints(this,this_00,&local_18,param_1,uVar4);
      LoadSounds(this,this_00,&local_18,param_1,uVar4);
      LoadPlaceables(this,this_00,&local_18,param_1,uVar4);
      LoadStores(this,this_00,&local_18,param_1,uVar4);
      LoadAreaEffects(this,this_00,&local_18,param_1,uVar4);
      LoadProperties(this,this_00,&local_18);
      LoadMaps(this,this_00,&local_18);
      LoadPlaceableCameras(this,this_00,&local_18);
      (**(code **)(this_00->resource).vtable)(1);
      ExceptionList = local_10;
      return 1;
    }
    (**(code **)(this_00->resource).vtable)(1);
    CRes::Release(&((this->res_helper).gff)->resource);
  }
  ExceptionList = local_c;
  return 0;
}
```
