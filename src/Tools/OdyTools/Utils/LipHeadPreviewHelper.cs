using System.Collections.Generic;
using Avalonia.Controls;
using BioWare.Common;
using BioWare.Extract;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF.Generics.UTC;
using BioWare.Resource.Formats.LIP;
using BioWare.Resource.Formats.TwoDA;
using BioWare.Tools;
using OdyTools.Data;

namespace OdyTools.Utils
{
    /// <summary>
    /// Resolves creature head MDL/MDX from installation Appearance for OdyToolLIP preview.
    /// </summary>
    public static class LipHeadPreviewHelper
    {
        public const string NoInstallationMessage =
            "Configure a game installation in Settings to preview creature heads.";

        public static string GetMouthStateLabel(LIPShape? shape)
        {
            return shape.HasValue ? "Mouth: " + shape.Value : string.Empty;
        }

        public static string FormatPlaybackOverlay(string baseOverlay, LIPShape? shape)
        {
            string mouth = GetMouthStateLabel(shape);
            string text = baseOverlay ?? string.Empty;
            if (string.IsNullOrEmpty(mouth))
            {
                return text;
            }

            if (string.IsNullOrEmpty(text))
            {
                return mouth;
            }

            return text + " | " + mouth;
        }

        public static bool TryPopulateAppearanceCombo(OdyInstallation odyInstallation, ComboBox combo)
        {
            if (odyInstallation == null || combo == null)
            {
                return false;
            }

            try
            {
                odyInstallation.HtBatchCache2DA(new List<string> { OdyInstallation.TwoDAAppearances });
                TwoDA appearances = odyInstallation.HtGetCache2DA(OdyInstallation.TwoDAAppearances);
                if (appearances == null)
                {
                    return false;
                }

                combo.Items.Clear();
                List<string> labels = appearances.GetColumn("label");
                if (labels != null)
                {
                    foreach (string label in labels)
                    {
                        combo.Items.Add(label ?? string.Empty);
                    }
                }

                return combo.Items.Count > 0;
            }
            catch
            {
                return false;
            }
        }

        public static bool TryLoadHeadModel(
            OdyInstallation odyInstallation,
            int appearanceId,
            out byte[] mdlData,
            out byte[] mdxData,
            out string modelName)
        {
            mdlData = null;
            mdxData = null;
            modelName = null;

            if (odyInstallation == null || odyInstallation.Installation == null)
            {
                return false;
            }

            if (appearanceId < 0)
            {
                return false;
            }

            var utc = new UTC
            {
                AppearanceId = appearanceId,
                Alignment = 50,
            };

            (string headModel, string headTexture) = Creature.GetHeadModel(utc, odyInstallation.Installation);
            if (string.IsNullOrWhiteSpace(headModel))
            {
                return false;
            }

            modelName = headModel.Trim();
            ResourceResult mdlResult = odyInstallation.Resource(modelName, ResourceType.MDL, null);
            ResourceResult mdxResult = odyInstallation.Resource(modelName, ResourceType.MDX, null);
            if (mdlResult == null || mdlResult.Data == null || mdxResult == null || mdxResult.Data == null)
            {
                return false;
            }

            mdlData = mdlResult.Data;
            mdxData = mdxResult.Data;
            return true;
        }
    }
}
