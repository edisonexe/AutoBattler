using System;
using System.Collections.Generic;
using System.Text;
using Data;
using Domain.Combat.Effects.Interfaces;
using UnityEngine;

namespace Domain.Core
{
    public abstract class Fighter
    {
        public List<IAttackEffect>    Attack    = new();
        public List<IDefenseEffect>   Defense   = new();
        public List<ITypeRule>        TypeRules = new();
        
        public string Name { get; }
        public Stats Stats { get; }
        public int MaxHp { get; private set; }
        public int Hp { get; private set; }
        public Weapon Weapon { get; private set; }
        public int TurnsTaken { get; private set; }

        protected Fighter(string name, Stats stats, int maxHp, Weapon weapon)
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
        
        public void SetMaxHp(int value, bool healNow = true)
        {
            if (value <= 0) return;
            MaxHp = value;
            if (healNow) HealToFull();
        }
        
        public void SetWeapon(Weapon newWeapon) => Weapon = newWeapon;

        public void IncrementTurn() => TurnsTaken++;
        
        public int GetBaseDamage() => (Weapon?.BaseDamage ?? 0) + Stats.Strength;
        
        public DamageType GetDamageType() => Weapon != null ? Weapon.Type : DamageType.Slashing;
        
        public void PrintInfoAboutFighter()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Имя: {Name}");
            sb.AppendLine($"Сила: {Stats.Strength}, Ловкость: {Stats.Agility}, Выносливость: {Stats.Stamina}");
            sb.AppendLine($"Макс. здоровье: {MaxHp}");
            sb.AppendLine($"Оружие: {Weapon.Name}, {Weapon.BaseDamage} урона");

            Debug.Log(sb.ToString());
        }
    }
}