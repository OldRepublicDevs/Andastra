using System;

namespace OdyTools.Data
{
    public class UTCSettings : Settings
    {
        public UTCSettings() : base("OdyToolUTC")
        {
        }

        public bool SaveUnusedFields
        {
            get => GetValue("saveUnusedFields", true);
            set => SetValue("saveUnusedFields", value);
        }

        public bool AlwaysSaveK2Fields
        {
            get => GetValue("alwaysSaveK2Fields", false);
            set => SetValue("alwaysSaveK2Fields", value);
        }
    }
}
