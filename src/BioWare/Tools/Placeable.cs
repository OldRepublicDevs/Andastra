using System;
using System.Collections.Generic;
using System.IO;
using BioWare.Common;
using BioWare.Resource.Formats.TwoDA;
using BioWare.Extract;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF.Generics;
using BioWare.Common.Logger;
using JetBrains.Annotations;

namespace BioWare.Tools
{
    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/placeable.py
    public static class Placeable
    {
        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/placeable.py:20-50
        // Original: def get_model(utp: UTP, installation: Installation, *, placeables: 2DA | SOURCE_TYPES | None = None) -> str:
        public static string GetModel(
            UTP utp,
            Installation installation,
            TwoDA placeables = null)
        {
            TwoDA placeables2DA = placeables;
            if (placeables2DA == null)
            {
                placeables2DA = TwoDAResourceLoader.LoadFromInstallation(installation, "placeables");
                if (placeables2DA == null)
                {
                    throw new ArgumentException("Resource 'placeables.2da' not found in the installation, cannot get UTP model.");
                }
            }

            return placeables2DA.GetRow(utp.AppearanceId).GetString("modelname");
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/placeable.py:53-104
        // Original: def load_placeables_2da(installation: Installation, logger: RobustLogger | None = None) -> 2DA | None:
        public static TwoDA LoadPlaceables2DA(
            Installation installation,
            RobustLogger logger = null)
        {
            return TwoDAResourceLoader.LoadFromInstallation(installation, "placeables", logger);
        }
    }
}
