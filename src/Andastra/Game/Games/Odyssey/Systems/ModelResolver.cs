using System;
using Andastra.Runtime.Core.Enums;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Interfaces.Components;
using Andastra.Game.Games.Odyssey.Components;
using Andastra.Game.Games.Odyssey.Data;
using JetBrains.Annotations;
using AppearanceData = Andastra.Game.Games.Odyssey.Data.GameDataManager.AppearanceData;
using DoorData = Andastra.Game.Games.Odyssey.Data.GameDataManager.DoorData;
using PlaceableData = Andastra.Game.Games.Odyssey.Data.GameDataManager.PlaceableData;

namespace Andastra.Game.Games.Odyssey.Systems
{
    /// <summary>
    /// Resolves model ResRefs from entity appearance types and object types.
    /// </summary>
    /// <remarks>
    /// Model Resolver:
    /// - [TODO: Function name] @ (K1: TODO: Find this address, TSL: TODO: Find this address address) model resolution system
    /// - Located via string references: Model loading from appearance.2da, placeables.2da, genericdoors.2da
    ///   - ["Appearance_Type"] @ (K1: TODO: Find this address, TSL: 0x007c40f0) - appearance type field,
    ///   - ["AppearanceType"]  @ (K1: TODO: Find this address, TSL: 0x007c84c8) - appearance type field variant
    ///   - ["ModelResRef"]     @ (K1: TODO: Find this address, TSL: 0x007c2f6c) - model resource reference,
    ///   - ["ModelName"]       @ (K1: TODO: Find this address, TSL: 0x007c1c8c) - model name field
    ///   - ["ModelA"]           @ (K1: TODO: Find this address, TSL: 0x007bf4bc) - model A field,
    ///   - ["ModelVariation"]   @ (K1: TODO: Find this address, TSL: 0x007c0990) - model variation field
    ///   - ["Appearance"]       @ (K1: TODO: Find this address, TSL: 0x007bf144) - appearance field,
    ///   - ["Appearance_Head"]  @ (K1: TODO: Find this address, TSL: 0x007c40e0) - appearance head field
    ///   - ["Appearance_01"]     @ (K1: TODO: Find this address, TSL: 0x007c0220) - appearance slots,
    ///   - ["Appearance_02"]     @ (K1: TODO: Find this address, TSL: 0x007c0200) - appearance slots,
    ///   - ["Appearance_03"]     @ (K1: TODO: Find this address, TSL: 0x007c01e0) - appearance slots,
    ///   - ["Appearance_04"]     @ (K1: TODO: Find this address, TSL: 0x007c01c0) - appearance slots
    ///   - ["MODELTYPE"]         @ (K1: TODO: Find this address, TSL: 0x007c036c) - model type field,
    ///   - ["ModelType"]         @ (K1: TODO: Find this address, TSL: 0x007c4568) - model type field variant
    ///   - ["MODEL01"]             @ (K1: TODO: Find this address, TSL: 0x007c4b48) - model variation fields,
    ///   - ["MODEL02"]             @ (K1: TODO: Find this address, TSL: 0x007c4b34) - model variation fields,
    ///   - ["MODEL03"]             @ (K1: 0x0074b8ec, TSL: 0x007c4b20) - model variation fields
    /// - Model loading errors:
    ///   - ["CSWCCreature::LoadModel(): Failed to load creature model '%s'." @ (K1: TODO: Find this address, TSL: 0x007c82fc) - model loading error
    ///   - ["CSWCCreatureAppearance::CreateBTypeBody(): Failed to load model '%s'." @ (K1: TODO: Find this address, TSL: 0x007cdc40) - model loading error
    /// - Original implementation: Resolves model ResRefs from appearance IDs using 2DA tables
    /// - appearance.2da: ["modela"]/["modelb"] columns for creatures (ModelA for variation 0, ModelB for variation 1)
    /// - placeables.2da: ["modelname"] column for placeables (placeable model ResRef)
    /// - genericdoors.2da: ["modelname"] column for doors (door model ResRef)
    /// - Body variation determines which model variant to use (0 = ModelA, 1 = ModelB, etc.)
    /// - Model resolution order:
    ///   - 1) RenderableComponent.ModelResRef (if set)
    ///   - 2) Resolve from appearance.2da/placeables.2da/genericdoors.2da
    /// - [TODO: Function name] @ (K1: TODO: Find this address, TSL: 0x005261b0) - resolves creature model from appearance.2da row, based on appearance type and body variation
    /// </remarks>
    public static class ModelResolver
    {
        /// <summary>
        /// Resolves the model ResRef for a creature based on appearance type and body variation.
        /// </summary>
        /// <remarks>
        /// swkotor2.exe: 0x005261b0 - resolves creature model from appearance.2da row, based on appearance type and body variation
        /// 
        /// Model resolution logic:
        /// - For ModelType "B" (Character) or "P" (Player): Uses model columns (modela through modeln) based on body variation
        ///   - Body variation 0 = modela, 1 = modelb, ..., 13 = modeln
        ///   - Body variation is typically determined by equipped armor (from baseitems.2da "bodyvar" column)
        /// - For other ModelTypes ("S", "F", "L"): Uses "race" column (single model for the appearance)
        /// - Falls back to modela if body variation is out of range or model column is empty
        /// </remarks>
        /// <param name="gameData">Game data manager for 2DA lookups.</param>
        /// <param name="appearanceType">Appearance type index into appearance.2da.</param>
        /// <param name="bodyVariation">Body variation (0 = ["modela"], 1 = ["modelb"], ..., 13 = ["modeln"]).</param>
        /// <returns>Model ResRef or null if not found.</returns>
        [CanBeNull]
        public static string ResolveCreatureModel([NotNull] GameDataManager gameData, int appearanceType, int bodyVariation = 0)
        {
            if (gameData == null)
            {
                throw new ArgumentNullException("gameData");
            }

            AppearanceData appearance = gameData.GetAppearance(appearanceType);
            if (appearance == null)
            {
                return null;
            }

            // For Character (B) or Player (P) model types, use model columns based on body variation
            // For other model types (S, F, L), use the race column (single model)
            string modelType = appearance.ModelType;
            if (modelType == "B" || modelType == "P")
            {
                // Use model columns (modela through modeln) based on body variation
                // Body variation 0 = modela, 1 = modelb, ..., 13 = modeln
                string modelResRef = appearance.GetModelByVariation(bodyVariation);
                
                // If the specific variation model is empty, try to fall back to modela
                if (string.IsNullOrEmpty(modelResRef))
                {
                    modelResRef = appearance.ModelA;
                }
                
                // If modela is also empty, fall back to race column
                if (string.IsNullOrEmpty(modelResRef))
                {
                    modelResRef = appearance.Race;
                }
                
                return modelResRef;
            }
            else
            {
                // For Simple (S), Droid (F), or Large (L) model types, use race column
                return appearance.Race;
            }
        }

        /// <summary>
        /// Resolves the model ResRef for a placeable based on appearance type.
        /// </summary>
        /// <param name="gameData">Game data manager for 2DA lookups.</param>
        /// <param name="appearanceType">Appearance type index into placeables.2da.</param>
        /// <returns>Model ResRef or null if not found.</returns>
        [CanBeNull]
        public static string ResolvePlaceableModel([NotNull] GameDataManager gameData, int appearanceType)
        {
            if (gameData == null)
            {
                throw new ArgumentNullException("gameData");
            }

            PlaceableData placeable = gameData.GetPlaceable(appearanceType);
            if (placeable == null)
            {
                return null;
            }

            return placeable.ModelName;
        }

        /// <summary>
        /// Resolves the model ResRef for a door based on appearance type.
        /// </summary>
        /// <param name="gameData">Game data manager for 2DA lookups.</param>
        /// <param name="appearanceType">Appearance type index into genericdoors.2da.</param>
        /// <returns>Model ResRef or null if not found.</returns>
        [CanBeNull]
        public static string ResolveDoorModel([NotNull] GameDataManager gameData, int appearanceType)
        {
            if (gameData == null)
            {
                throw new ArgumentNullException("gameData");
            }

            DoorData door = gameData.GetDoor(appearanceType);
            if (door == null)
            {
                return null;
            }

            return door.ModelName;
        }

        /// <summary>
        /// Resolves the model ResRef for an entity based on its type and appearance.
        /// </summary>
        /// <param name="gameData">Game data manager for 2DA lookups.</param>
        /// <param name="entity">The entity to resolve model for.</param>
        /// <returns>Model ResRef or null if not found.</returns>
        [CanBeNull]
        public static string ResolveEntityModel([NotNull] GameDataManager gameData, [NotNull] IEntity entity)
        {
            if (gameData == null)
            {
                throw new ArgumentNullException("gameData");
            }
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            // Check if entity already has a renderable component with model
            IRenderableComponent renderable = entity.GetComponent<IRenderableComponent>();
            if (renderable != null && !string.IsNullOrEmpty(renderable.ModelResRef))
            {
                return renderable.ModelResRef;
            }

            // Resolve based on object type
            switch (entity.ObjectType)
            {
                case ObjectType.Creature:
                    CreatureComponent creature = entity.GetComponent<CreatureComponent>();
                    if (creature != null)
                    {
                        return ResolveCreatureModel(gameData, creature.AppearanceType, creature.BodyVariation);
                    }
                    break;

                case ObjectType.Placeable:
                    PlaceableComponent placeable = entity.GetComponent<PlaceableComponent>();
                    if (placeable != null)
                    {
                        return ResolvePlaceableModel(gameData, placeable.AppearanceType);
                    }
                    break;

                case ObjectType.Door:
                    OdysseyDoorComponent door = entity.GetComponent<OdysseyDoorComponent>();
                    if (door != null)
                    {
                        return ResolveDoorModel(gameData, door.GenericType);
                    }
                    break;
            }

            return null;
        }
    }
}

