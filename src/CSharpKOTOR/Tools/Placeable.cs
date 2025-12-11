using System;
using CSharpKOTOR.Formats.TwoDA;
using CSharpKOTOR.Installation;
using CSharpKOTOR.Logger;
using CSharpKOTOR.Resource.Generics;
using CSharpKOTOR.Resources;
using JetBrains.Annotations;

namespace CSharpKOTOR.Tools
{
    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/placeable.py
    public static class Placeable
    {
        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/placeable.py:20-50
        // Original: def get_model(utp: UTP, installation: Installation, *, placeables: TwoDA | SOURCE_TYPES | None = None) -> str:
        public static string GetModel(
            UTP utp,
            Installation.Installation installation,
            TwoDA placeables = null)
        {
            TwoDA placeables2DA;
            if (placeables == null)
            {
                var result = installation.Resources.LookupResource("placeables", ResourceType.TwoDA);
                if (result == null)
                {
                    throw new ArgumentException("Resource 'placeables.2da' not found in the installation, cannot get UTP model.");
                }
                var reader = new TwoDABinaryReader(result.Data);
                placeables2DA = reader.Load();
            }
            else
            {
                placeables2DA = placeables;
            }

            return placeables2DA.GetRow(utp.AppearanceId).GetString("modelname");
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/placeable.py:53-104
        // Original: def load_placeables_2da(installation: Installation, logger: RobustLogger | None = None) -> TwoDA | None:
        public static TwoDA LoadPlaceables2DA(
            Installation.Installation installation,
            RobustLogger logger = null)
        {
            if (logger == null)
            {
                logger = new RobustLogger();
            }

            TwoDA placeables2DA = null;

            // Try locations() first (more reliable, handles BIF files)
            try
            {
                // TODO: Implement installation.locations() equivalent
            }
            catch (Exception ex)
            {
                logger.Debug($"locations() failed for placeables.2da: {ex}");
            }

            // Fallback: try resource() if locations() didn't work
            if (placeables2DA == null)
            {
                try
                {
                    var placeablesResult = installation.Resources.LookupResource("placeables", ResourceType.TwoDA);
                    if (placeablesResult != null && placeablesResult.Data != null)
                    {
                        var reader = new TwoDABinaryReader(placeablesResult.Data);
                        placeables2DA = reader.Load();
                    }
                }
                catch (Exception ex)
                {
                    logger.Debug($"resource() also failed for placeables.2da: {ex}");
                }
            }

            return placeables2DA;
        }
    }
}

