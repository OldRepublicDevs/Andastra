using System;

namespace OdyTools.Data
{
    // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utc.py:1163-1181
    // Original: class UTCSettings:
    public class OdyToolUTCSettings : Settings
    {
        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utc.py:1168-1173
        // Original: def saveUnusedFields(self) -> bool:
        public bool SaveUnusedFields
        {
            get => GetValue("saveUnusedFields", true);
            set => SetValue("saveUnusedFields", value);
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utc.py:1176-1181
        // Original: def alwaysSaveK2Fields(self) -> bool:
        public bool AlwaysSaveK2Fields
        {
            get => GetValue("alwaysSaveK2Fields", false);
            set => SetValue("alwaysSaveK2Fields", value);
        }

        public OdyToolUTCSettings() : base("OdyToolUTC")
        {
        }
    }
}

