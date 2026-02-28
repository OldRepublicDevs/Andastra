using System;

namespace OdyTools.Data
{
    public class ModelRendererSettings : Settings
    {
        public ModelRendererSettings() : base("ModelRenderer")
        {
        }

        public bool UtcShowByDefault
        {
            get => GetValue("utcShowByDefault", false);
            set => SetValue("utcShowByDefault", value);
        }

        public int BackgroundColour
        {
            get => GetValue("backgroundColour", 0);
            set => SetValue("backgroundColour", value);
        }
    }
}

