using System;
using System.Collections.Generic;
using Data;
using Domain.Combat.Effects;
using Domain.Combat.Effects.Interfaces;

namespace Domain.Core
{
    public abstract class Fighter
    {
        public List<IStartTurnEffect> StartTurn = new();
        public List<IAttackEffect>    Attack    = new();
        public List<IDefenseEffect>   Defense   = new();
        public List<ITypeRule>        TypeRules = new();
        
        public string Name { get; private set; }
        public Stats Stats { get; }
        public int MaxHp { get; private set; }
        public int Hp { get; private set; }
        public WeaponConfig Weapon { get; private set; }
        public int TurnsTaken { get; private set; }

        protected Fighter(string name, Stats stats, int maxHp, WeaponConfig weapon)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));
            if (stats == null)
                throw new ArgumentNullException(nameof(stats));
            if (weapon == null)
                throw new ArgumentNullException(nameof(weapon));
            if (maxHp <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxHp), "MaxHp must be > 0.");

            Name = name;
            Stats = stats;
            MaxHp = maxHp;
            Hp = maxHp;
            Weapon = weapon;
            TurnsTaken = 0;
        }
        
        public bool IsAlive => Hp > 0;
        
        public void ResetForCombat()
        {
            TurnsTaken = 0;
            Hp = MaxHp;
        }
        
        public void HealToFull() => Hp = MaxHp;
        
        public void TakeDamage(int amount)
        {
            if (amount <= 0) return;
            Hp = Math.Max(0, Hp - amount);
        }
        
        public void IncreaseMaxHp(int amount, bool healNow = true)
        {
            if (amount <= 0) return;
            MaxHp += amount;
            if (healNow) Hp = MaxHp;
        }
        
        public void SetWeapon(WeaponConfig newWeapon) => Weapon = newWeapon;

        public void IncrementTurn() => TurnsTaken++;
        
        public int GetBaseDamage() => (Weapon?.BaseDamage ?? 0) + Stats.Strenght;
        
        public DamageType GetDamageType() => Weapon != null ? Weapon.Type : DamageType.Slashing;
    }
}