using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using Andastra.Runtime.Core.Enums;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Interfaces.Components;
using Andastra.Runtime.Games.Common.Components;
using Moq;

namespace Andastra.Tests.Runtime.TestHelpers
{
    /// <summary>
    /// Helper class for creating test entities and components.
    /// </summary>
    public static class EntityTestHelper
    {
        /// <summary>
        /// Creates a mock world for testing.
        /// </summary>
        public static IWorld CreateMockWorld()
        {
            var mockWorld = new Mock<IWorld>(MockBehavior.Loose);
            return mockWorld.Object;
        }

        /// <summary>
        /// Creates a test transform component with specified values.
        /// </summary>
        public static ITransformComponent CreateTestTransformComponent(
            float x = 10.0f,
            float y = 20.0f,
            float z = 30.0f,
            float facing = 1.57f,
            float scaleX = 1.0f,
            float scaleY = 1.0f,
            float scaleZ = 1.0f)
        {
            // Use BaseTransformComponent as a concrete implementation
            // We'll need to create an engine-specific one, but for testing we can use reflection
            // or create a minimal mock
            var mockTransform = new Mock<ITransformComponent>(MockBehavior.Strict);
            mockTransform.Setup(t => t.Position).Returns(new Vector3(x, y, z));
            mockTransform.Setup(t => t.Facing).Returns(facing);
            mockTransform.Setup(t => t.Scale).Returns(new Vector3(scaleX, scaleY, scaleZ));
            mockTransform.Setup(t => t.Parent).Returns((IEntity)null);
            return mockTransform.Object;
        }

        /// <summary>
        /// Creates a test stats component with specified values.
        /// </summary>
        public static IStatsComponent CreateTestStatsComponent(
            int currentHP = 100,
            int maxHP = 100,
            int currentFP = 50,
            int maxFP = 50)
        {
            var mockStats = new Mock<IStatsComponent>(MockBehavior.Strict);
            mockStats.Setup(s => s.CurrentHP).Returns(currentHP);
            mockStats.Setup(s => s.MaxHP).Returns(maxHP);
            mockStats.Setup(s => s.CurrentFP).Returns(currentFP);
            mockStats.Setup(s => s.MaxFP).Returns(maxFP);
            mockStats.Setup(s => s.IsDead).Returns(currentHP <= 0);
            mockStats.Setup(s => s.BaseAttackBonus).Returns(5);
            mockStats.Setup(s => s.ArmorClass).Returns(15);
            mockStats.Setup(s => s.FortitudeSave).Returns(8);
            mockStats.Setup(s => s.ReflexSave).Returns(6);
            mockStats.Setup(s => s.WillSave).Returns(4);
            mockStats.Setup(s => s.WalkSpeed).Returns(2.0f);
            mockStats.Setup(s => s.RunSpeed).Returns(4.0f);
            mockStats.Setup(s => s.Level).Returns(5);

            // Setup ability getters
            mockStats.Setup(s => s.GetAbility(Ability.Strength)).Returns(16);
            mockStats.Setup(s => s.GetAbility(Ability.Dexterity)).Returns(14);
            mockStats.Setup(s => s.GetAbility(Ability.Constitution)).Returns(15);
            mockStats.Setup(s => s.GetAbility(Ability.Intelligence)).Returns(12);
            mockStats.Setup(s => s.GetAbility(Ability.Wisdom)).Returns(13);
            mockStats.Setup(s => s.GetAbility(Ability.Charisma)).Returns(10);

            // Setup ability modifiers
            mockStats.Setup(s => s.GetAbilityModifier(Ability.Strength)).Returns(3);
            mockStats.Setup(s => s.GetAbilityModifier(Ability.Dexterity)).Returns(2);
            mockStats.Setup(s => s.GetAbilityModifier(Ability.Constitution)).Returns(2);
            mockStats.Setup(s => s.GetAbilityModifier(Ability.Intelligence)).Returns(1);
            mockStats.Setup(s => s.GetAbilityModifier(Ability.Wisdom)).Returns(1);
            mockStats.Setup(s => s.GetAbilityModifier(Ability.Charisma)).Returns(0);

            // Setup setters
            mockStats.SetupProperty(s => s.CurrentHP, currentHP);
            mockStats.SetupProperty(s => s.MaxHP, maxHP);
            mockStats.SetupProperty(s => s.CurrentFP, currentFP);
            mockStats.SetupProperty(s => s.MaxFP, maxFP);
            mockStats.SetupProperty(s => s.WalkSpeed, 2.0f);
            mockStats.SetupProperty(s => s.RunSpeed, 4.0f);

            // Setup SetAbility
            var abilityScores = new Dictionary<Ability, int>
            {
                { Ability.Strength, 16 },
                { Ability.Dexterity, 14 },
                { Ability.Constitution, 15 },
                { Ability.Intelligence, 12 },
                { Ability.Wisdom, 13 },
                { Ability.Charisma, 10 }
            };

            mockStats.Setup(s => s.SetAbility(It.IsAny<Ability>(), It.IsAny<int>()))
                .Callback<Ability, int>((ability, value) => abilityScores[ability] = value);
            mockStats.Setup(s => s.GetAbility(It.IsAny<Ability>()))
                .Returns<Ability>(ability => abilityScores[ability]);

            return mockStats.Object;
        }

        /// <summary>
        /// Creates a test script hooks component with specified scripts and local variables.
        /// </summary>
        public static IScriptHooksComponent CreateTestScriptHooksComponent(
            Dictionary<ScriptEvent, string> scripts = null,
            Dictionary<string, int> localInts = null,
            Dictionary<string, float> localFloats = null,
            Dictionary<string, string> localStrings = null)
        {
            scripts = scripts ?? new Dictionary<ScriptEvent, string>();
            localInts = localInts ?? new Dictionary<string, int>();
            localFloats = localFloats ?? new Dictionary<string, float>();
            localStrings = localStrings ?? new Dictionary<string, string>();

            // Use BaseScriptHooksComponent as concrete implementation
            var component = new BaseScriptHooksComponent();

            // Set scripts
            foreach (var kvp in scripts)
            {
                component.SetScript(kvp.Key, kvp.Value);
            }

            // Set local variables using reflection
            Type componentType = typeof(BaseScriptHooksComponent);
            FieldInfo localIntsField = componentType.GetField("_localInts", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo localFloatsField = componentType.GetField("_localFloats", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo localStringsField = componentType.GetField("_localStrings", BindingFlags.NonPublic | BindingFlags.Instance);

            if (localIntsField != null)
            {
                var dict = localIntsField.GetValue(component) as Dictionary<string, int>;
                if (dict != null)
                {
                    foreach (var kvp in localInts)
                    {
                        dict[kvp.Key] = kvp.Value;
                    }
                }
            }

            if (localFloatsField != null)
            {
                var dict = localFloatsField.GetValue(component) as Dictionary<string, float>;
                if (dict != null)
                {
                    foreach (var kvp in localFloats)
                    {
                        dict[kvp.Key] = kvp.Value;
                    }
                }
            }

            if (localStringsField != null)
            {
                var dict = localStringsField.GetValue(component) as Dictionary<string, string>;
                if (dict != null)
                {
                    foreach (var kvp in localStrings)
                    {
                        dict[kvp.Key] = kvp.Value;
                    }
                }
            }

            return component;
        }

        /// <summary>
        /// Creates a test inventory component with specified items.
        /// </summary>
        public static IInventoryComponent CreateTestInventoryComponent(
            Dictionary<int, IEntity> items = null)
        {
            items = items ?? new Dictionary<int, IEntity>();

            var mockInventory = new Mock<IInventoryComponent>(MockBehavior.Strict);

            // Setup GetItemInSlot
            mockInventory.Setup(i => i.GetItemInSlot(It.IsAny<int>()))
                .Returns<int>(slot => items.ContainsKey(slot) ? items[slot] : null);

            // Setup HasItemByTag
            mockInventory.Setup(i => i.HasItemByTag(It.IsAny<string>()))
                .Returns<string>(tag => 
                {
                    foreach (var item in items.Values)
                    {
                        if (item != null && item.Tag == tag)
                            return true;
                    }
                    return false;
                });

            // Setup GetAllItems
            mockInventory.Setup(i => i.GetAllItems())
                .Returns(() => new List<IEntity>(items.Values));

            return mockInventory.Object;
        }

        /// <summary>
        /// Creates a test door component with specified values.
        /// </summary>
        public static IDoorComponent CreateTestDoorComponent(
            bool isOpen = false,
            bool isLocked = false,
            int lockDC = 20,
            int hitPoints = 50,
            int maxHitPoints = 50)
        {
            var mockDoor = new Mock<IDoorComponent>(MockBehavior.Strict);
            mockDoor.SetupProperty(d => d.IsOpen, isOpen);
            mockDoor.SetupProperty(d => d.IsLocked, isLocked);
            mockDoor.SetupProperty(d => d.LockableByScript, true);
            mockDoor.SetupProperty(d => d.LockDC, lockDC);
            mockDoor.SetupProperty(d => d.IsBashed, false);
            mockDoor.SetupProperty(d => d.HitPoints, hitPoints);
            mockDoor.SetupProperty(d => d.MaxHitPoints, maxHitPoints);
            mockDoor.SetupProperty(d => d.Hardness, 5);
            mockDoor.SetupProperty(d => d.KeyTag, "");
            mockDoor.SetupProperty(d => d.KeyRequired, false);
            mockDoor.SetupProperty(d => d.OpenState, 0);
            mockDoor.SetupProperty(d => d.LinkedTo, "");
            mockDoor.SetupProperty(d => d.LinkedToModule, "");
            return mockDoor.Object;
        }

        /// <summary>
        /// Creates a test placeable component with specified values.
        /// </summary>
        public static IPlaceableComponent CreateTestPlaceableComponent(
            bool isUseable = true,
            bool hasInventory = false,
            bool isOpen = false,
            bool isLocked = false)
        {
            var mockPlaceable = new Mock<IPlaceableComponent>(MockBehavior.Strict);
            mockPlaceable.SetupProperty(p => p.IsUseable, isUseable);
            mockPlaceable.SetupProperty(p => p.HasInventory, hasInventory);
            mockPlaceable.SetupProperty(p => p.IsStatic, false);
            mockPlaceable.SetupProperty(p => p.IsOpen, isOpen);
            mockPlaceable.SetupProperty(p => p.IsLocked, isLocked);
            mockPlaceable.SetupProperty(p => p.LockDC, 15);
            mockPlaceable.SetupProperty(p => p.KeyTag, "");
            mockPlaceable.SetupProperty(p => p.HitPoints, 30);
            mockPlaceable.SetupProperty(p => p.MaxHitPoints, 30);
            mockPlaceable.SetupProperty(p => p.Hardness, 3);
            mockPlaceable.SetupProperty(p => p.AnimationState, 0);
            return mockPlaceable.Object;
        }

        /// <summary>
        /// Sets custom data on an entity using reflection.
        /// </summary>
        public static void SetCustomData(IEntity entity, string key, object value)
        {
            Type baseEntityType = typeof(BaseEntity);
            FieldInfo dataField = baseEntityType.GetField("_data", BindingFlags.NonPublic | BindingFlags.Instance);
            if (dataField != null)
            {
                var data = dataField.GetValue(entity) as Dictionary<string, object>;
                if (data == null)
                {
                    data = new Dictionary<string, object>();
                    dataField.SetValue(entity, data);
                }
                data[key] = value;
            }
        }

        /// <summary>
        /// Gets custom data from an entity using reflection.
        /// </summary>
        public static object GetCustomData(IEntity entity, string key)
        {
            Type baseEntityType = typeof(BaseEntity);
            FieldInfo dataField = baseEntityType.GetField("_data", BindingFlags.NonPublic | BindingFlags.Instance);
            if (dataField != null)
            {
                var data = dataField.GetValue(entity) as Dictionary<string, object>;
                if (data != null && data.ContainsKey(key))
                {
                    return data[key];
                }
            }
            return null;
        }

        /// <summary>
        /// Creates a simple test entity for inventory items.
        /// </summary>
        public static IEntity CreateTestItemEntity(uint objectId, string tag, ObjectType objectType)
        {
            var mockItem = new Mock<IEntity>(MockBehavior.Strict);
            mockItem.Setup(i => i.ObjectId).Returns(objectId);
            mockItem.SetupProperty(i => i.Tag, tag);
            mockItem.Setup(i => i.ObjectType).Returns(objectType);
            mockItem.Setup(i => i.IsValid).Returns(true);
            mockItem.SetupProperty(i => i.AreaId, 0u);
            mockItem.SetupProperty(i => i.World, (IWorld)null);
            return mockItem.Object;
        }
    }
}

