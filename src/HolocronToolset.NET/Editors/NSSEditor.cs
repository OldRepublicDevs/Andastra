using System;
using System.Text;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CSharpKOTOR.Resources;
using HolocronToolset.NET.Data;

namespace HolocronToolset.NET.Editors
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py
    // Original: class NSSEditor(Editor):
    public class NSSEditor : Editor
    {
        private string _sourceCode;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py
        // Original: def __init__(self, parent, installation):
        public NSSEditor(Window parent = null, HTInstallation installation = null)
            : base(parent, "NSS Editor", "nss",
                new[] { ResourceType.NSS, ResourceType.NCS },
                new[] { ResourceType.NSS },
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
                SetupProgrammaticUI();
            }
        }

        private void SetupProgrammaticUI()
        {
            // Create basic UI structure programmatically
            var panel = new StackPanel();
            Content = panel;
        }

        private void SetupUI()
        {
            // Additional UI setup if needed
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py
        // Original: def load(self, filepath, resref, restype, data):
        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            if (data == null || data.Length == 0)
            {
                _sourceCode = "";
                return;
            }

            try
            {
                if (restype == ResourceType.NSS)
                {
                    // NSS is plaintext
                    _sourceCode = Encoding.UTF8.GetString(data);
                }
                else if (restype == ResourceType.NCS)
                {
                    // NCS needs to be decompiled
                    if (_installation != null)
                    {
                        // _sourceCode = ScriptDecompiler.HtDecompileScript(data, _installation.Path, _installation.Tsl);
                        _sourceCode = ""; // Will be implemented when decompiler is available
                    }
                    else
                    {
                        _sourceCode = "";
                    }
                }
            }
            catch
            {
                _sourceCode = "";
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py
        // Original: def build(self) -> tuple[bytes, bytes]:
        public override Tuple<byte[], byte[]> Build()
        {
            if (string.IsNullOrEmpty(_sourceCode))
            {
                return Tuple.Create(new byte[0], new byte[0]);
            }

            // Compile NSS to NCS if installation is available
            if (_installation != null)
            {
                // byte[] compiled = ScriptCompiler.HtCompileScript(_sourceCode, _installation.Path, _installation.Tsl);
                // return Tuple.Create(compiled, new byte[0]);
                // Will be implemented when compiler is available
            }

            // Return source as UTF-8 bytes
            byte[] sourceBytes = Encoding.UTF8.GetBytes(_sourceCode);
            return Tuple.Create(sourceBytes, new byte[0]);
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py
        // Original: def new(self):
        public override void New()
        {
            base.New();
            _sourceCode = "";
        }

        public override void SaveAs()
        {
            Save();
        }
    }
}
