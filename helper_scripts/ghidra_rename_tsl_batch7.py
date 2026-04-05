from ghidra.program.model.symbol import SourceType
fm = currentProgram.getFunctionManager()
sm = currentProgram.getSymbolTable()
renamed = 0; errors = 0
try:
  fn = fm.getFunctionAt(toAddr(0x4824F0))
  if fn:
    fn.setName('Load2DArrays_Regeneration', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTwoDimArrays', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTwoDimArrays', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4824F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x481740))
  if fn:
    fn.setName('Load2DArrays_SurfaceMaterial', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTwoDimArrays', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTwoDimArrays', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x481740: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x481920))
  if fn:
    fn.setName('Load2DArrays_VisualEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTwoDimArrays', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTwoDimArrays', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x481920: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x483370))
  if fn:
    fn.setName('Load2DArrays_WeaponSounds', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTwoDimArrays', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTwoDimArrays', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x483370: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x489180))
  if fn:
    fn.setName('LoadIPRPParamTables', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTwoDimArrays', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTwoDimArrays', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x489180: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x640920))
  if fn:
    fn.setName('CVirtualMachine', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachine', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachine', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x640920: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x640DD0))
  if fn:
    fn.setName('DeleteScriptSituation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachine', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachine', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x640DD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x640B80))
  if fn:
    fn.setName('GetRunScriptReturnValue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachine', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachine', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x640B80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6467D0))
  if fn:
    fn.setName('LoadScriptSituation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachine', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachine', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6467D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x640BA0))
  if fn:
    fn.setName('SetCommandImplementer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachine', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachine', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x640BA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6409D0))
  if fn:
    fn.setName('_CVirtualMachine', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachine', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachine', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6409D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x645C50))
  if fn:
    fn.setName('GetRunScriptReturnValue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachineInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachineInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x645C50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x646AB0))
  if fn:
    fn.setName('SetCommandImplementer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachineInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachineInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x646AB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6463E0))
  if fn:
    fn.setName('StackPopCommand', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachineInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachineInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6463E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x645ED0))
  if fn:
    fn.setName('StackPopFloat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachineInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachineInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x645ED0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x645E00))
  if fn:
    fn.setName('StackPopInteger', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachineInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachineInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x645E00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x646070))
  if fn:
    fn.setName('StackPopString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachineInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachineInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x646070: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x645FA0))
  if fn:
    fn.setName('StackPopVector', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachineInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachineInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x645FA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x646280))
  if fn:
    fn.setName('StackPushEngineStructure', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachineInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachineInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x646280: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x645E80))
  if fn:
    fn.setName('StackPushInteger', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachineInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachineInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x645E80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x646390))
  if fn:
    fn.setName('StackPushObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachineInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachineInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x646390: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x646100))
  if fn:
    fn.setName('StackPushString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachineInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachineInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x646100: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x646010))
  if fn:
    fn.setName('StackPushVector', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachineInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachineInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x646010: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6421C0))
  if fn:
    fn.setName('LoadStack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachineStack', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachineStack', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6421C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x641FC0))
  if fn:
    fn.setName('SaveStack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachineStack', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachineStack', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x641FC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6461C0))
  if fn:
    fn.setName('SetStackPointer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachineStack', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachineStack', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6461C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80B3D0))
  if fn:
    fn.setName('AddWorldTimes', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80B3D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80B8C0))
  if fn:
    fn.setName('AdvanceToTime', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80B8C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80AC50))
  if fn:
    fn.setName('CWorldTimer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80AC50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80B4C0))
  if fn:
    fn.setName('CompareWorldTimes', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80B4C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80AF80))
  if fn:
    fn.setName('ConvertFromCalendarDay', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80AF80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80AED0))
  if fn:
    fn.setName('ConvertHourMinSecMsToMs', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80AED0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80B750))
  if fn:
    fn.setName('GetCalendarDayFromSeconds', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80B750: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80AE00))
  if fn:
    fn.setName('GetSnapshotTime', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80AE00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80B1B0))
  if fn:
    fn.setName('GetTimeDifferenceFromWorldTime', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80B1B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80B7E0))
  if fn:
    fn.setName('GetTimeOfDayFromSeconds', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80B7E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80B530))
  if fn:
    fn.setName('PauseWorldTimer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80B530: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80B110))
  if fn:
    fn.setName('SetWorldTime', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80B110: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80B440))
  if fn:
    fn.setName('SubtractWorldTimes', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80B440: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40B960))
  if fn:
    fn.setName('ChangeResolutionForMovie', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40B960: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x457EA0))
  if fn:
    fn.setName('CheatSetSkill', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x457EA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x866F10))
  if fn:
    fn.setName('ConsoleFuncs_Remove', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x866F10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40B750))
  if fn:
    fn.setName('DeactivateRenderWindow', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40B750: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80BA70))
  if fn:
    fn.setName('DegToRad', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80BA70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x408DE0))
  if fn:
    fn.setName('DoConsoleHist', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x408DE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F8BB0))
  if fn:
    fn.setName('EffectTypeToGameEffect', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F8BB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E20B0))
  if fn:
    fn.setName('ExecuteCommandAddJournalWorldEntry', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E20B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8F70E0))
  if fn:
    fn.setName('FindAnimation', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8F70E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x849110))
  if fn:
    fn.setName('FindPart', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x849110: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x883520))
  if fn:
    fn.setName('Transform', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('GLRender', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'GLRender', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x883520: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x846F70))
  if fn:
    fn.setName('GatherBones', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x846F70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6084B0))
  if fn:
    fn.setName('GetGameVersionInfo', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6084B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x716310))
  if fn:
    fn.setName('GetObstructionFactor', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x716310: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6270E0))
  if fn:
    fn.setName('GetProperty0x24', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6270E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9211C0))
  if fn:
    fn.setName('GetProperty0x30', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9211C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60D2D0))
  if fn:
    fn.setName('GetProperty0x4', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60D2D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x472990))
  if fn:
    fn.setName('GetProperty0x68', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x472990: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4BCF10))
  if fn:
    fn.setName('GetProperty0xc', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4BCF10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x867D60))
  if fn:
    fn.setName('GetResource', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x867D60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x84C400))
  if fn:
    fn.setName('IsInCutscene', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Gob', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Gob', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x84C400: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x859B50))
  if fn:
    fn.setName('VisibilityCheck', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Gob', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Gob', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x859B50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x409C70))
  if fn:
    fn.setName('HandleWMKeyDownMessage', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x409C70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x488DC0))
  if fn:
    fn.setName('HitGroundEvent', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x488DC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8103A0))
  if fn:
    fn.setName('Homogenize', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8103A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4082B0))
  if fn:
    fn.setName('InitGameApp', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4082B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x408600))
  if fn:
    fn.setName('InitOpenGLWindow', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x408600: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x903F60))
  if fn:
    fn.setName('Read', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Input', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Input', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x903F60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x904290))
  if fn:
    fn.setName('Read', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('InputBinary', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'InputBinary', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x904290: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5D2950))
  if fn:
    fn.setName('LoadFromGFF', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('KOTOR_AUTOSAVE_PARAMS', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'KOTOR_AUTOSAVE_PARAMS', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D2950: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BF810))
  if fn:
    fn.setName('SaveToGFF', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('KOTOR_AUTOSAVE_PARAMS', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'KOTOR_AUTOSAVE_PARAMS', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BF810: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x810220))
  if fn:
    fn.setName('MakeMatrix4x4', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x810220: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x81A740))
  if fn:
    fn.setName('ManageSceneBSP', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x81A740: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8F4580))
  if fn:
    fn.setName('Init', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Material', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Material', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8F4580: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8F4CB0))
  if fn:
    fn.setName('ParseField', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Material', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Material', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8F4CB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8F5380))
  if fn:
    fn.setName('SetBlendingMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Material', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Material', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8F5380: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x457AC0))
  if fn:
    fn.setName('Matrix', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Matrix', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Matrix', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x457AC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8CBC40))
  if fn:
    fn.setName('InternalParseField', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('MdlNodeTriMesh', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'MdlNodeTriMesh', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8CBC40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x845300))
  if fn:
    fn.setName('DelRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Model', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Model', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x845300: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8450C0))
  if fn:
    fn.setName('_Model', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Model', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Model', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8450C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6DF080))
  if fn:
    fn.setName('NVThunkItemGameObject_AIUpdate', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6DF080: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x47E5F0))
  if fn:
    fn.setName('NVThunkSWCItmSWCObject_AIUpdate', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x47E5F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x730D40))
  if fn:
    fn.setName('_NWPlayerCharacterList_st', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('NWPlayerCharacterList_st', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'NWPlayerCharacterList_st', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x730D40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x540530))
  if fn:
    fn.setName('OnAnimKeyEvent', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x540530: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x741E90))
  if fn:
    fn.setName('OnApplyBeam', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x741E90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x744FE0))
  if fn:
    fn.setName('OnApplyCurse', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x744FE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7487D0))
  if fn:
    fn.setName('OnApplyDarkness', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7487D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x748DB0))
  if fn:
    fn.setName('OnApplyDisappear', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x748DB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7489A0))
  if fn:
    fn.setName('OnApplyDisappearAppear', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7489A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x747330))
  if fn:
    fn.setName('OnApplyDisguise', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x747330: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7457D0))
  if fn:
    fn.setName('OnApplyDispelAllMagic', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7457D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x74BF20))
  if fn:
    fn.setName('OnApplyForceFizzle', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x74BF20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x749EC0))
  if fn:
    fn.setName('OnApplyForceJump', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x749EC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7422A0))
  if fn:
    fn.setName('OnApplyForceResistanceDecrease', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7422A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x74BE00))
  if fn:
    fn.setName('OnApplyForceResisted', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x74BE00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x743FD0))
  if fn:
    fn.setName('OnApplyHasteInternal', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x743FD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x749BA0))
  if fn:
    fn.setName('OnApplyHitPointChangeWhenDying', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x749BA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x744D20))
  if fn:
    fn.setName('OnApplyLight', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x744D20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x743F50))
  if fn:
    fn.setName('OnApplyLink', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x743F50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x748EB0))
  if fn:
    fn.setName('OnApplyNegativeLevel', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x748EB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x74C480))
  if fn:
    fn.setName('OnApplyPureEvilPowers', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x74C480: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x74C010))
  if fn:
    fn.setName('OnApplyPureGoodPowers', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x74C010: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x740720))
  if fn:
    fn.setName('OnApplySetStateInternal', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x740720: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x745540))
  if fn:
    fn.setName('OnApplySilence', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x745540: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x741470))
  if fn:
    fn.setName('OnApplySkillDecrease', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x741470: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x73C300))
  if fn:
    fn.setName('OnApplySummonParty', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x73C300: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7460F0))
  if fn:
    fn.setName('OnApplyTaunt', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7460F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x747660))
  if fn:
    fn.setName('OnApplyTimestop', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x747660: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x744E00))
  if fn:
    fn.setName('OnApplyVision', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x744E00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7496B0))
  if fn:
    fn.setName('OnApplyWounding', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7496B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x748B70))
  if fn:
    fn.setName('OnRemoveDisappearAppear', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x748B70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7474A0))
  if fn:
    fn.setName('OnRemoveDisguise', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7474A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7C9C80))
  if fn:
    fn.setName('OnRemoveForcePush', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C9C80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x743AE0))
  if fn:
    fn.setName('OnRemoveHasteOrSlow', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x743AE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x744940))
  if fn:
    fn.setName('OnRemoveSanctuary', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x744940: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x740440))
  if fn:
    fn.setName('OnRemoveSetState', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x740440: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x741150))
  if fn:
    fn.setName('OnRemoveSetStateInternal', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x741150: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8A7C20))
  if fn:
    fn.setName('AnimateParticles', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('PartEmitter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'PartEmitter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8A7C20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x84AE20))
  if fn:
    fn.setName('PartLight', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('PartLight', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'PartLight', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x84AE20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x853600))
  if fn:
    fn.setName('PartSetCanDownSample', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x853600: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x882D60))
  if fn:
    fn.setName('DrawBumpedOutShell', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('PartSkin', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'PartSkin', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x882D60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x881670))
  if fn:
    fn.setName('DrawBumpedOutShellVP', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('PartSkin', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'PartSkin', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x881670: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8D5F60))
  if fn:
    fn.setName('SetHighlighted', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('PartTrigger', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'PartTrigger', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8D5F60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80D820))
  if fn:
    fn.setName('Pitch', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80D820: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80C380))
  if fn:
    fn.setName('PlaneHeightAtPoint', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80C380: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x811430))
  if fn:
    fn.setName('Quaternion', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Quaternion', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Quaternion', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x811430: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x828800))
  if fn:
    fn.setName('axis', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Quaternion', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Quaternion', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x828800: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x883810))
  if fn:
    fn.setName('getmatrix', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Quaternion', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Quaternion', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x883810: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x42A8F0))
  if fn:
    fn.setName('zdir', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Quaternion', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Quaternion', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x42A8F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x888430))
  if fn:
    fn.setName('RenderEMLM', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x888430: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x883F00))
  if fn:
    fn.setName('RenderFlatDecal', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x883F00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x877A20))
  if fn:
    fn.setName('RenderSkinned', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x877A20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x87F060))
  if fn:
    fn.setName('RenderSkinnedVPBlur', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x87F060: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40BA50))
  if fn:
    fn.setName('RestoreResolutionFromMovie', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40BA50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80D900))
  if fn:
    fn.setName('Roll', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80D900: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80DF50))
  if fn:
    fn.setName('RotationArc', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80DF50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C0310))
  if fn:
    fn.setName('SJournalEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('SJournalEntry', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'SJournalEntry', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C0310: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x85AED0))
  if fn:
    fn.setName('DoGobShadowsForThisLight', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Scene', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Scene', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x85AED0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x861E10))
  if fn:
    fn.setName('LoadVisibility', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Scene', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Scene', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x861E10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x85E510))
  if fn:
    fn.setName('UpdateEmitters', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Scene', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Scene', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x85E510: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x86D460))
  if fn:
    fn.setName('SceneBSPGetTileData', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x86D460: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x86DE00))
  if fn:
    fn.setName('SceneBSPRemovePart', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x86DE00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x86D690))
  if fn:
    fn.setName('SceneBSPRemoveTiledata', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x86D690: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x43A5E0))
  if fn:
    fn.setName('SetDisplayDevMode', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x43A5E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x408910))
  if fn:
    fn.setName('SetWindowTitle', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x408910: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x409920))
  if fn:
    fn.setName('SetupOpenGL', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x409920: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40F4A0))
  if fn:
    fn.setName('SwitchWeaponEvent', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40F4A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x855B00))
  if fn:
    fn.setName('_TileData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('TileData', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'TileData', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x855B00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8834F0))
  if fn:
    fn.setName('Transform', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8834F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x413720))
  if fn:
    fn.setName('operator__', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Vector', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Vector', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x413720: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40A260))
  if fn:
    fn.setName('WinMessageHandler', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40A260: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80D740))
  if fn:
    fn.setName('Yaw', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80D740: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x92E8BD))
  if fn:
    fn.setName('__unlock', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x92E8BD: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x927726))
  if fn:
    fn.setName('_rename', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x927726: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x928B98))
  if fn:
    fn.setName('_stat', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x928B98: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4585B0))
  if fn:
    fn.setName('adddarkside', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4585B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4582A0))
  if fn:
    fn.setName('addexp', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4582A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8831C0))
  if fn:
    fn.setName('countreset', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8831C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9274B0))
  if fn:
    fn.setName('floor', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9274B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x867F30))
  if fn:
    fn.setName('getnextline_file', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x867F30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x867FA0))
  if fn:
    fn.setName('getnextline_res', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x867FA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x458730))
  if fn:
    fn.setName('giveitem', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x458730: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80C7D0))
  if fn:
    fn.setName('glProgramEnvParameter4fARB', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80C7D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x92183B))
  if fn:
    fn.setName('inline_arrayUnwind', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x92183B: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x92590C))
  if fn:
    fn.setName('inline_array_unwind', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x92590C: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x92B8AA))
  if fn:
    fn.setName('inline_decrement_ptd_translator', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x92B8AA: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x924896))
  if fn:
    fn.setName('inline_lock_8', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x924896: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x92279E))
  if fn:
    fn.setName('inline_unlock_1', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x92279E: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x92489F))
  if fn:
    fn.setName('inline_unlock_8', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x92489F: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x92276F))
  if fn:
    fn.setName('inline_unlock_file2', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x92276F: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x458FB0))
  if fn:
    fn.setName('lockcamera', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x458FB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80C170))
  if fn:
    fn.setName('normalize', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80C170: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x413280))
  if fn:
    fn.setName('operator_', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x413280: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80F810))
  if fn:
    fn.setName('operator__', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80F810: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80DCF0))
  if fn:
    fn.setName('pointto', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80DCF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80C0A0))
  if fn:
    fn.setName('quaternionScalarMult', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80C0A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x93DAEB))
  if fn:
    fn.setName('return_0', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x93DAEB: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61AAC0))
  if fn:
    fn.setName('return_true', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61AAC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C72D0))
  if fn:
    fn.setName('return_true_4', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C72D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x51BBD0))
  if fn:
    fn.setName('self_return', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x51BBD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x45DED0))
  if fn:
    fn.setName('caseD_1', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('switchD_00621238', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'switchD_00621238', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x45DED0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x924FE4))
  if fn:
    fn.setName('unlock_8', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x924FE4: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x81B710))
  if fn:
    fn.setName('vectorSum', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x81B710: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40C510))
  if fn:
    fn.setName('void_return_this_4', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40C510: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x458B20))
  if fn:
    fn.setName('warp', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x458B20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x458CA0))
  if fn:
    fn.setName('whereami', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x458CA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40D250))
  if fn:
    fn.setName('StopMenuMusic', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40D250: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4894B0))
  if fn:
    fn.setName('GetIPRPParamTable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTwoDimArrays', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTwoDimArrays', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4894B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B07A0))
  if fn:
    fn.setName('SetExtent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiDialogCinematic', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiDialogCinematic', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B07A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F5A80))
  if fn:
    fn.setName('GetCurrentName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiQuickPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiQuickPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F5A80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64BE70))
  if fn:
    fn.setName('Initialize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64BE70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64C090))
  if fn:
    fn.setName('MovePlayerToArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64C090: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BB0D0))
  if fn:
    fn.setName('GetWaypointByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BB0D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BB220))
  if fn:
    fn.setName('GetStoreByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BB220: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x897BB0))
  if fn:
    fn.setName('StaticPartNeedsMat', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x897BB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8F5530))
  if fn:
    fn.setName('SetMaterialColor', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Material', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Material', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8F5530: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5D1D20))
  if fn:
    fn.setName('LoadPCAutoSave', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D1D20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5CD8F0))
  if fn:
    fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5CD8F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6969A0))
  if fn:
    fn.setName('Rest', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6969A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4132E0))
  if fn:
    fn.setName('AnimationUpdate', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4132E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x705D10))
  if fn:
    fn.setName('ComputeSafeLocationInDirection', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x705D10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7D85F0))
  if fn:
    fn.setName('WriteRepositoryUpdate', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D85F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x506820))
  if fn:
    fn.setName('MoveNextDoorState', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x506820: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5EF6F0))
  if fn:
    fn.setName('CreateOption', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5EF6F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4952B0))
  if fn:
    fn.setName('DetermineFeat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiFeatsCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiFeatsCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4952B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64CAB0))
  if fn:
    fn.setName('ExecuteLoadModule', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64CAB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40D640))
  if fn:
    fn.setName('SetMoveToModuleStartWaypoint', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40D640: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x474420))
  if fn:
    fn.setName('AnimationStationary', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x474420: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DFAB0))
  if fn:
    fn.setName('CResGFF', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResGFF', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResGFF', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DFAB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64B940))
  if fn:
    fn.setName('SetLoadingModule', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64B940: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x669990))
  if fn:
    fn.setName('ItemListGetItemObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CItemRepository', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CItemRepository', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x669990: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7CA390))
  if fn:
    fn.setName('AddActiveItemPropertiesToMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7CA390: '+str(e)); errors += 1
print('Batch 7 done: renamed='+str(renamed)+' errors='+str(errors))