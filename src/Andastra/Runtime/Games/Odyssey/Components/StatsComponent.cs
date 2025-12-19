using System;
using System.Collections.Generic;
using Andastra.Runtime.Core.Enums;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Interfaces.Components;
using Andastra.Runtime.Core.Combat;

namespace Andastra.Runtime.Engines.Odyssey.Components
{
    /// <summary>
    /// Concrete implementation of creature stats for KOTOR.
    /// </summary>
    /// <remarks>
    /// KOTOR D20 System:
    /// - Based on swkotor2.exe stats system
    /// - Located via string references: "CurrentHP" @ 0x007c1b40 (current HP field), "CurrentHP: " @ 0x007cb168 (debug display)
    /// - "Max_HPs" @ 0x007cb714 (max HP field), "InCombatHPBase" @ 0x007bf224 (in-combat HP base), "OutOfCombatHPBase" @ 0x007bf210 (out-of-combat HP base)
    /// - "DAM_HP" @ 0x007bf130 (HP damage type identifier)
    /// - "TimePerHP" @ 0x007bf234 (time per HP regen field), "FPRegenTime" @ 0x007bf524 (Force point regen time)
    /// - Force Points: "CurrentForce" @ 0x007c401c (current Force points field), "ForcePoints" @ 0x007c3410 (Force points field)
    /// - "MaxForcePoints" @ 0x007c4278 (max Force points field), "BonusForcePoints" @ 0x007bf640 (bonus Force points field)
    /// - Ability score fields: "AbilityScore" @ 0x007c2b74, "STR" @ 0x007c2b80, "DEX" @ 0x007c2b8c, "CON" @ 0x007c2b98, "INT" @ 0x007c2ba4, "WIS" @ 0x007c2bb0, "CHA" @ 0x007c2bbc
    /// - "ModSTR" @ 0x007c2bc8, "ModDEX" @ 0x007c2bd4, "ModCON" @ 0x007c2be0, "ModINT" @ 0x007c2bec, "ModWIS" @ 0x007c2bf8, "ModCHA" @ 0x007c2c04 (ability modifiers)
    /// - Save fields: "FortSave" @ 0x007c4764, "RefSave" @ 0x007c4750, "WillSave" @ 0x007c4758 (save throw fields)
    /// - "FortSaveThrow" @ 0x007c42b4, "RefSaveThrow" @ 0x007c42c4, "WillSaveThrow" @ 0x007c42d4 (save throw calculation fields)
    /// - "Save_DC" @ 0x007c048c (save DC field), "DC_SAVE" @ 0x007c0160 (DC save constant)
    /// - Original implementation: FUN_005226d0 @ 0x005226d0 (save creature stats to GFF including abilities, HP, saves)
    /// - FUN_004dfbb0 @ 0x004dfbb0 (load creature stats from GIT including abilities, HP, saves, BAB)
    /// - Ability scores, HP, BAB, saves stored in creature GFF structures at offsets in creature object
    /// - Ability scores: 1-30+ range, modifier = (score - 10) / 2 (D20 formula, rounded down)
    /// - Hit points: Based on class hit dice + Con modifier per level (from classes.2da HP column)
    /// - Attack: BAB + STR/DEX mod vs. Defense (natural 20 = auto hit, natural 1 = auto miss)
    /// - Defense: 10 + DEX mod + Armor + Natural + Deflection + Class bonus (AC = Armor Class)
    /// - Saves: Base + ability mod (Fort=CON, Ref=DEX, Will=WIS) + miscellaneous bonuses
    /// - InCombatHPBase vs OutOfCombatHPBase: Creatures have separate HP pools for combat/non-combat (combat HP is base for combat, non-combat HP is base for non-combat)
    /// - Force points: CurrentFP and MaxFP stored for Force-sensitive classes (Jedi Consular, Guardian, Sentinel, etc.)
    /// 
    /// Key 2DA tables:
    /// - classes.2da: Hit dice (HP column), BAB progression (BAB column), saves progression (Fort/Ref/Will columns)
    /// - appearance.2da: Walk/run speed (WALKRATE/RUNRATE columns), creature size (SIZE column)
    /// - abilities.2da: Ability score names and descriptions (not used for calculations, just display)
    /// </remarks>
    public class StatsComponent : IStatsComponent
    {
        private readonly Dictionary<Ability, int> _abilities;
        private readonly Dictionary<int, int> _skills; // Skill ID -> Skill Rank
        private readonly HashSet<int> _knownSpells; // Spell ID -> Known
        private int _currentHP;
        private int _maxHP;
        private int _baseLevel;
        private int _baseAttackBonus;
        private int _baseFortitude;
        private int _baseReflex;
        private int _baseWill;
        private int _armorBonus;
        private int _naturalArmor;
        private int _deflectionBonus;
        private int _effectACBonus; // AC bonus from effects
        private int _effectAttackBonus; // Attack bonus from effects
        private int _currentFP;
        private int _maxFP;

        public StatsComponent()
        {
            _abilities = new Dictionary<Ability, int>();
            _skills = new Dictionary<int, int>();
            _knownSpells = new HashSet<int>();
            
            // Default ability scores (10 = average human)
            foreach (Ability ability in Enum.GetValues(typeof(Ability)))
            {
                _abilities[ability] = 10;
            }
            
            // Initialize all skills to 0 (untrained)
            // KOTOR has 8 skills: COMPUTER_USE, DEMOLITIONS, STEALTH, AWARENESS, PERSUADE, REPAIR, SECURITY, TREAT_INJURY
            for (int i = 0; i < 8; i++)
            {
                _skills[i] = 0;
            }
            
            _currentHP = 10;
            _maxHP = 10;
            _currentFP = 0;
            _maxFP = 0;
            _baseLevel = 1;
            _baseAttackBonus = 0;
            _baseFortitude = 0;
            _baseReflex = 0;
            _baseWill = 0;
            _armorBonus = 0;
            _naturalArmor = 0;
            _deflectionBonus = 0;
            _effectACBonus = 0;
            _effectAttackBonus = 0;
            
            // Default movement speeds (from appearance.2da averages)
            _baseWalkSpeed = 1.75f;
            _baseRunSpeed = 4.0f;
        }

        #region IComponent Implementation

        public IEntity Owner { get; set; }

        public void OnAttach()
        {
            // Initialize from entity data if available
            if (Owner != null)
            {
                // Try to load stats from entity's stored data
                LoadFromEntityData();
            }
        }

        public void OnDetach()
        {
            // Save stats back to entity data if needed
        }

        #endregion

        #region IStatsComponent Implementation

        public int CurrentHP
        {
            get { return _currentHP; }
            set { _currentHP = Math.Max(0, Math.Min(value, MaxHP)); }
        }

        public int MaxHP
        {
            get { return _maxHP; }
            set { _maxHP = Math.Max(1, value); }
        }

        public int GetAbility(Ability ability)
        {
            int value;
            if (_abilities.TryGetValue(ability, out value))
            {
                return value;
            }
            return 10;
        }

        public void SetAbility(Ability ability, int value)
        {
            _abilities[ability] = Math.Max(1, Math.Min(100, value));
        }

        public int GetAbilityModifier(Ability ability)
        {
            // D20 formula: (score - 10) / 2, rounded down
            int score = GetAbility(ability);
            return (score - 10) / 2;
        }

        public bool IsDead
        {
            get { return _currentHP <= 0; }
        }

        public int BaseAttackBonus
        {
            get
            {
                // BAB + STR modifier for melee (or DEX for ranged/finesse) + effect bonuses
                int effectBonus = _effectAttackBonus;
                
                // Query EffectSystem for additional attack bonuses if available
                if (Owner != null && Owner.World != null && Owner.World.EffectSystem != null)
                {
                    foreach (ActiveEffect activeEffect in Owner.World.EffectSystem.GetEffects(Owner))
                    {
                        if (activeEffect.Effect.Type == EffectType.AttackIncrease)
                        {
                            effectBonus += activeEffect.Effect.Amount;
                        }
                        else if (activeEffect.Effect.Type == EffectType.AttackDecrease)
                        {
                            effectBonus -= activeEffect.Effect.Amount;
                        }
                    }
                }
                
                return _baseAttackBonus + GetAbilityModifier(Ability.Strength) + effectBonus;
            }
        }

        public int ArmorClass
        {
            get
            {
                // Defense = 10 + DEX mod + Armor + Natural + Deflection + Effect bonuses
                int effectBonus = _effectACBonus;
                
                // Query EffectSystem for additional AC bonuses if available
                if (Owner != null && Owner.World != null && Owner.World.EffectSystem != null)
                {
                    foreach (ActiveEffect activeEffect in Owner.World.EffectSystem.GetEffects(Owner))
                    {
                        if (activeEffect.Effect.Type == EffectType.ACIncrease)
                        {
                            effectBonus += activeEffect.Effect.Amount;
                        }
                        else if (activeEffect.Effect.Type == EffectType.ACDecrease)
                        {
                            effectBonus -= activeEffect.Effect.Amount;
                        }
                    }
                }
                
                return 10 
                    + GetAbilityModifier(Ability.Dexterity)
                    + _armorBonus
                    + _naturalArmor
                    + _deflectionBonus
                    + effectBonus;
            }
        }

        public int FortitudeSave
        {
            get { return _baseFortitude + GetAbilityModifier(Ability.Constitution); }
        }

        public int ReflexSave
        {
            get { return _baseReflex + GetAbilityModifier(Ability.Dexterity); }
        }

        public int WillSave
        {
            get { return _baseWill + GetAbilityModifier(Ability.Wisdom); }
        }

        private float _baseWalkSpeed;
        private float _baseRunSpeed;

        public float WalkSpeed
        {
            get
            {
                return CalculateMovementSpeed(_baseWalkSpeed);
            }
            set
            {
                _baseWalkSpeed = value;
            }
        }

        public float RunSpeed
        {
            get
            {
                return CalculateMovementSpeed(_baseRunSpeed);
            }
            set
            {
                _baseRunSpeed = value;
            }
        }

        public int GetSkillRank(int skill)
        {
            // Returns skill rank, or 0 if untrained, or -1 if skill doesn't exist
            if (skill < 0 || skill >= 8)
            {
                return -1; // Invalid skill ID
            }
            
            int rank;
            if (_skills.TryGetValue(skill, out rank))
            {
                return rank;
            }
            
            return 0; // Untrained (default)
        }

        /// <summary>
        /// Sets the skill rank for a given skill.
        /// </summary>
        public void SetSkillRank(int skill, int rank)
        {
            if (skill >= 0 && skill < 8)
            {
                _skills[skill] = Math.Max(0, rank);
            }
        }

        #endregion

        #region Spell Knowledge

        /// <summary>
        /// Checks if the creature knows a spell/Force power.
        /// </summary>
        public bool HasSpell(int spellId)
        {
            return _knownSpells.Contains(spellId);
        }

        /// <summary>
        /// Adds a spell to the creature's known spells list.
        /// </summary>
        public void AddSpell(int spellId)
        {
            _knownSpells.Add(spellId);
        }

        /// <summary>
        /// Removes a spell from the creature's known spells list.
        /// </summary>
        public void RemoveSpell(int spellId)
        {
            _knownSpells.Remove(spellId);
        }

        /// <summary>
        /// Gets all known spells.
        /// </summary>
        public System.Collections.Generic.IEnumerable<int> GetKnownSpells()
        {
            return _knownSpells;
        }

        #endregion

        #region Extended Properties

        /// <summary>
        /// Character level (total class levels).
        /// </summary>
        public int Level
        {
            get { return _baseLevel; }
            set { _baseLevel = Math.Max(1, value); }
        }

        /// <summary>
        /// Experience points.
        /// </summary>
        public int Experience { get; set; }

        /// <summary>
        /// Current force points.
        /// </summary>
        public int CurrentFP
        {
            get { return _currentFP; }
            set { _currentFP = Math.Max(0, Math.Min(value, MaxFP)); }
        }

        /// <summary>
        /// Maximum force points.
        /// </summary>
        public int MaxFP
        {
            get { return _maxFP; }
            set { _maxFP = Math.Max(0, value); }
        }

        /// <summary>
        /// Armor bonus from equipped armor.
        /// </summary>
        public int ArmorBonus
        {
            get { return _armorBonus; }
            set { _armorBonus = Math.Max(0, value); }
        }

        /// <summary>
        /// Natural armor bonus.
        /// </summary>
        public int NaturalArmor
        {
            get { return _naturalArmor; }
            set { _naturalArmor = Math.Max(0, value); }
        }

        /// <summary>
        /// Deflection bonus (from shields, effects).
        /// </summary>
        public int DeflectionBonus
        {
            get { return _deflectionBonus; }
            set { _deflectionBonus = Math.Max(0, value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the maximum HP.
        /// </summary>
        public void SetMaxHP(int value)
        {
            _maxHP = Math.Max(1, value);
            if (_currentHP > _maxHP)
            {
                _currentHP = _maxHP;
            }
        }

        /// <summary>
        /// Sets the base attack bonus.
        /// </summary>
        public void SetBaseAttackBonus(int value)
        {
            _baseAttackBonus = Math.Max(0, value);
        }

        /// <summary>
        /// Sets the base saving throws.
        /// </summary>
        public void SetBaseSaves(int fortitude, int reflex, int will)
        {
            _baseFortitude = fortitude;
            _baseReflex = reflex;
            _baseWill = will;
        }

        /// <summary>
        /// Adds an AC bonus from an effect.
        /// </summary>
        public void AddEffectACBonus(int bonus)
        {
            _effectACBonus += bonus;
        }

        /// <summary>
        /// Removes an AC bonus from an effect.
        /// </summary>
        public void RemoveEffectACBonus(int bonus)
        {
            _effectACBonus -= bonus;
        }

        /// <summary>
        /// Adds an attack bonus from an effect.
        /// </summary>
        public void AddEffectAttackBonus(int bonus)
        {
            _effectAttackBonus += bonus;
        }

        /// <summary>
        /// Removes an attack bonus from an effect.
        /// </summary>
        public void RemoveEffectAttackBonus(int bonus)
        {
            _effectAttackBonus -= bonus;
        }

        /// <summary>
        /// Applies damage to the creature.
        /// </summary>
        /// <param name="damage">Amount of damage</param>
        /// <returns>Actual damage dealt</returns>
        public int TakeDamage(int damage)
        {
            if (damage <= 0)
            {
                return 0;
            }

            int actualDamage = Math.Min(damage, _currentHP);
            _currentHP -= actualDamage;
            return actualDamage;
        }

        /// <summary>
        /// Heals the creature.
        /// </summary>
        /// <param name="amount">Amount to heal</param>
        /// <returns>Actual amount healed</returns>
        public int Heal(int amount)
        {
            if (amount <= 0 || IsDead)
            {
                return 0;
            }

            int actualHeal = Math.Min(amount, _maxHP - _currentHP);
            _currentHP += actualHeal;
            return actualHeal;
        }

        /// <summary>
        /// Makes a saving throw.
        /// </summary>
        /// <param name="saveType">Type of save (0=Fort, 1=Ref, 2=Will)</param>
        /// <param name="dc">Difficulty class</param>
        /// <param name="roll">The d20 roll result</param>
        /// <returns>True if save succeeded</returns>
        public bool MakeSavingThrow(int saveType, int dc, int roll)
        {
            int bonus;
            switch (saveType)
            {
                case 0:
                    bonus = FortitudeSave;
                    break;
                case 1:
                    bonus = ReflexSave;
                    break;
                case 2:
                    bonus = WillSave;
                    break;
                default:
                    bonus = 0;
                    break;
            }

            // Natural 20 always succeeds, natural 1 always fails
            if (roll == 20)
            {
                return true;
            }
            if (roll == 1)
            {
                return false;
            }

            return roll + bonus >= dc;
        }

        /// <summary>
        /// Calculates XP needed for next level.
        /// </summary>
        public int XPForNextLevel()
        {
            // KOTOR uses: XP = level * (level - 1) * 500
            int nextLevel = Level + 1;
            return nextLevel * (nextLevel - 1) * 500;
        }

        /// <summary>
        /// Checks if creature can level up.
        /// </summary>
        public bool CanLevelUp()
        {
            return Experience >= XPForNextLevel() && Level < 20;
        }

        /// <summary>
        /// Loads stats from entity's stored data.
        /// </summary>
        private void LoadFromEntityData()
        {
            // Stats are now loaded via SetMaxHP, SetAbility, etc.
            // TODO: PLACEHOLDER - This method is a placeholder for future entity data integration.
        }

        /// <summary>
        /// Calculates final movement speed after applying all movement speed modifiers.
        /// </summary>
        /// <param name="baseSpeed">Base movement speed before modifiers</param>
        /// <returns>Final movement speed with all effects applied</returns>
        /// <remarks>
        /// Movement Speed Calculation (swkotor2.exe, nwmain.exe):
        /// - Based on swkotor2.exe: Haste/Slow effects modify movement speed
        ///   Located via string references: "Haste" @ routine 119, "Slow" @ routine 120
        ///   Original implementation: Haste doubles speed (2.0x), Slow halves speed (0.5x)
        /// - Based on nwmain.exe: GetWalkRate returns GetMovementRateFactor(this) * baseWalkRate * constant
        ///   Located via function: GetWalkRate @ 0x140396730 (nwmain.exe)
        ///   Movement rate factor accumulates all movement speed modifiers
        /// - EffectMovementSpeedIncrease (script function 165):
        ///   If nNewSpeedPercent < 100: final speed = (100 + nNewSpeedPercent)%
        ///   If nNewSpeedPercent >= 100: final speed = nNewSpeedPercent%
        ///   Example: 50 -> 150%, 200 -> 200%
        /// - EffectMovementSpeedDecrease (script function 451):
        ///   nPercentChange expected to be 1-99 (percentage reduction)
        ///   If negative: results in speed increase
        ///   If >= 100: effect is deleted/ignored
        ///   Example: 50 -> 50% reduction (speed becomes 50% of original)
        /// - Effects are applied multiplicatively in order:
        ///   1. Haste/Slow (fixed multipliers)
        ///   2. MovementSpeedIncrease (percentage-based, replaces speed)
        ///   3. MovementSpeedDecrease (percentage reduction)
        /// - Based on original engine behavior: effects stack multiplicatively
        /// </remarks>
        private float CalculateMovementSpeed(float baseSpeed)
        {
            if (baseSpeed <= 0.0f)
            {
                return 0.1f; // Minimum speed
            }

            float speed = baseSpeed;
            float speedMultiplier = 1.0f;
            float speedPercent = 100.0f; // Percentage-based speed (100% = unchanged)
            bool hasSpeedPercent = false;

            // Query EffectSystem for all movement speed modifiers
            if (Owner != null && Owner.World != null && Owner.World.EffectSystem != null)
            {
                foreach (ActiveEffect activeEffect in Owner.World.EffectSystem.GetEffects(Owner))
                {
                    EffectType effectType = activeEffect.Effect.Type;
                    int effectAmount = activeEffect.Effect.Amount;

                    if (effectType == EffectType.Haste)
                    {
                        // Haste doubles movement speed (100% increase = 2.0x multiplier)
                        // Based on swkotor2.exe: Haste effect @ routine 119
                        speedMultiplier *= 2.0f;
                    }
                    else if (effectType == EffectType.Slow)
                    {
                        // Slow halves movement speed (50% reduction = 0.5x multiplier)
                        // Based on swkotor2.exe: Slow effect @ routine 120
                        speedMultiplier *= 0.5f;
                    }
                    else if (effectType == EffectType.MovementSpeedIncrease)
                    {
                        // EffectMovementSpeedIncrease: Percentage-based speed modifier
                        // Based on script function 165: EffectMovementSpeedIncrease(int nNewSpeedPercent)
                        // If nNewSpeedPercent < 100: add 100 to get final percentage (e.g., 50 -> 150%)
                        // If nNewSpeedPercent >= 100: use directly as percentage (e.g., 200 -> 200%)
                        // This replaces any previous speed percentage (does not stack with other MovementSpeedIncrease)
                        if (effectAmount < 100)
                        {
                            speedPercent = 100.0f + effectAmount;
                        }
                        else
                        {
                            speedPercent = effectAmount;
                        }
                        hasSpeedPercent = true;
                    }
                    else if (effectType == EffectType.MovementSpeedDecrease)
                    {
                        // EffectMovementSpeedDecrease: Percentage reduction
                        // Based on script function 451: EffectMovementSpeedDecrease(int nPercentChange)
                        // Expected to be 1-99 (percentage to reduce by)
                        // If negative: results in speed increase
                        // If >= 100: effect is ignored/deleted
                        if (effectAmount > 0 && effectAmount < 100)
                        {
                            // Reduce speed by percentage (e.g., 50 means 50% reduction, speed becomes 50% of current)
                            speedPercent *= (100.0f - effectAmount) / 100.0f;
                        }
                        else if (effectAmount < 0)
                        {
                            // Negative values result in speed increase (unusual but documented behavior)
                            speedPercent *= (100.0f - effectAmount) / 100.0f;
                        }
                        // If >= 100, ignore the effect
                    }
                }
            }

            // Apply multipliers first (Haste/Slow)
            speed = speed * speedMultiplier;

            // Apply percentage-based modifiers (MovementSpeedIncrease/Decrease)
            if (hasSpeedPercent || speedPercent != 100.0f)
            {
                speed = speed * (speedPercent / 100.0f);
            }

            // Ensure minimum speed to prevent zero/negative values
            return Math.Max(0.1f, speed);
        }

        #endregion
    }
}
