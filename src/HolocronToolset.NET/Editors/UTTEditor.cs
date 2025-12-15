using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CSharpKOTOR.Common;
using CSharpKOTOR.Formats.GFF;
using CSharpKOTOR.Resource.Generics;
using CSharpKOTOR.Resources;
using HolocronToolset.NET.Data;
using GFFAuto = CSharpKOTOR.Formats.GFF.GFFAuto;

namespace HolocronToolset.NET.Editors
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/utt.py:23
    // Original: class UTTEditor(Editor):
    public class UTTEditor : Editor
    {
        private UTT _utt;
        private GFF _originalGff;

        public UTTEditor(Window parent = null, HTInstallation installation = null)
            : base(parent, "Trigger Editor", "trigger",
                new[] { ResourceType.UTT, ResourceType.BTT },
                new[] { ResourceType.UTT, ResourceType.BTT },
                installation)
        {
            InitializeComponent();
            SetupUI();
            New();
        }

        private void InitializeComponent()
        {
            if (!TryLoadXaml())
            {
                SetupUI();
            }
        }

        private void SetupUI()
        {
            var panel = new StackPanel();
            Content = panel;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/utt.py:121-131
        // Original: def load(self, filepath, resref, restype, data):
        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);

            try
            {
                // Store original GFF for roundtrip preservation
                _originalGff = GFF.FromBytes(data);
                var utt = UTTAuto.ReadUtt(data);
                LoadUTT(utt);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Failed to load UTT: {ex}");
                New();
                _originalGff = null;
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/utt.py:133-185
        // Original: def _loadUTT(self, utt: UTT):
        private void LoadUTT(UTT utt)
        {
            _utt = utt;
            // UI loading will be implemented when UI elements are available
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/utt.py:187-240
        // Original: def build(self) -> tuple[bytes, bytes]:
        public override Tuple<byte[], byte[]> Build()
        {
            Game game = _installation?.Game ?? Game.K2;
            var gff = UTTHelpers.DismantleUtt(_utt, game);

            // Preserve unmodified fields from original GFF that aren't yet supported by UTT object model
            if (_originalGff != null)
            {
                var originalRoot = _originalGff.Root;
                var newRoot = gff.Root;

                // List of fields that UTTHelpers.DismantleUtt explicitly sets
                // Note: Some fields that may have default value issues are handled specially below
                var fieldsSetByDismantle = new System.Collections.Generic.HashSet<string>
                {
                    "Tag", "ResRef", "Comment", "Type", "LinkedTo", "LinkedToFlags",
                    "KeyName", "TrapDetectable", "TrapDetectDC", "DisarmDC", "TrapFlag",
                    "TrapType", "IsTrap", "KeyRequired", "Lockable", "Locked", "Hardness",
                    "KeyName2", "ScriptHeartbeat", "ScriptOnEnter", "ScriptOnExit", "ScriptUserDefine",
                    "TransitionDestin"
                    // Note: TrapOneShot, PaletteID, TrapDisarmable, Cursor, Faction are handled in fieldsToRestore
                };

                // Fields that may have default value mismatches - always restore from original
                // These fields are set by DismantleUtt but may use default values instead of original values
                var fieldsToRestore = new System.Collections.Generic.HashSet<string> { "PaletteID", "TrapDisarmable", "Cursor", "Faction", "TrapOneShot" };
                foreach (var fieldName in fieldsToRestore)
                {
                    if (originalRoot.Exists(fieldName))
                    {
                        var originalFieldType = originalRoot.GetFieldType(fieldName);
                        if (originalFieldType.HasValue)
                        {
                            // Always restore these fields from original, overwriting whatever DismantleUtt set
                            // Remove first to ensure clean copy
                            if (newRoot.Exists(fieldName))
                            {
                                newRoot.Remove(fieldName);
                            }
                            CopyGffField(originalRoot, newRoot, fieldName, originalFieldType.Value);
                        }
                    }
                }
                
                // Verify the restore worked by checking if field exists and matches
                // (This is for debugging - can remove later)

                // Copy all fields from original that aren't explicitly set by DismantleUtt
                foreach (var (label, fieldType, value) in originalRoot)
                {
                    if (!fieldsSetByDismantle.Contains(label) && !fieldsToRestore.Contains(label) && !newRoot.Exists(label))
                    {
                        CopyGffField(originalRoot, newRoot, label, fieldType);
                    }
                }
            }

            byte[] data = GFFAuto.BytesGff(gff, ResourceType.UTT);
            return Tuple.Create(data, new byte[0]);
        }

        private void CopyGffField(GFFStruct sourceStruct, GFFStruct targetStruct, string label, GFFFieldType fieldType)
        {
            switch (fieldType)
            {
                case GFFFieldType.UInt8: targetStruct.SetUInt8(label, sourceStruct.GetUInt8(label)); break;
                case GFFFieldType.Int8: targetStruct.SetInt8(label, sourceStruct.GetInt8(label)); break;
                case GFFFieldType.UInt16: targetStruct.SetUInt16(label, sourceStruct.GetUInt16(label)); break;
                case GFFFieldType.Int16: targetStruct.SetInt16(label, sourceStruct.GetInt16(label)); break;
                case GFFFieldType.UInt32: targetStruct.SetUInt32(label, sourceStruct.GetUInt32(label)); break;
                case GFFFieldType.Int32: targetStruct.SetInt32(label, sourceStruct.GetInt32(label)); break;
                case GFFFieldType.UInt64: targetStruct.SetUInt64(label, sourceStruct.GetUInt64(label)); break;
                case GFFFieldType.Int64: targetStruct.SetInt64(label, sourceStruct.GetInt64(label)); break;
                case GFFFieldType.Single: targetStruct.SetSingle(label, sourceStruct.GetSingle(label)); break;
                case GFFFieldType.Double: targetStruct.SetDouble(label, sourceStruct.GetDouble(label)); break;
                case GFFFieldType.String: targetStruct.SetString(label, sourceStruct.GetString(label)); break;
                case GFFFieldType.ResRef: targetStruct.SetResRef(label, sourceStruct.GetResRef(label)); break;
                case GFFFieldType.LocalizedString: targetStruct.SetLocString(label, sourceStruct.GetLocString(label)); break;
                case GFFFieldType.Binary: targetStruct.SetBinary(label, sourceStruct.GetBinary(label)); break;
                case GFFFieldType.Vector3: targetStruct.SetVector3(label, sourceStruct.GetVector3(label)); break;
                case GFFFieldType.Vector4: targetStruct.SetVector4(label, sourceStruct.GetVector4(label)); break;
                case GFFFieldType.Struct: targetStruct.SetStruct(label, sourceStruct.GetStruct(label)); break;
                case GFFFieldType.List: targetStruct.SetList(label, sourceStruct.GetList(label)); break;
                default:
                    // Log or handle unsupported types
                    break;
            }
        }

        public override void New()
        {
            base.New();
            _utt = new UTT();
        }

        public override void SaveAs()
        {
            Save();
        }
    }
}
