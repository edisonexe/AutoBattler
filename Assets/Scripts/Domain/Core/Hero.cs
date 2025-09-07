using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Domain.Core
{
    public class Hero : Fighter
    {
        private readonly Dictionary<string, int> _classLevels = new();

        public IReadOnlyDictionary<string, int> ClassLevels => _classLevels;

        public int TotalLevel
        {
            get
            {
                int sum = 0;
                foreach (var kv in _classLevels) sum += kv.Value;
                return sum;
            }
        }
        
        public Hero(string name, Stats stats, int maxHp, WeaponConfig weapon) : base(name, stats, maxHp, weapon) {}
        
        public int GetLevel(string className) =>
            _classLevels.TryGetValue(className, out var lvl) ? lvl : 0;

        public void SetLevel(string className, int level) => _classLevels[className] = level;

        public void AddLevel(string className, int delta = 1)
        {
            if (delta <= 0) return;
            _classLevels[className] = GetLevel(className) + delta;
        }

        public void PrintDescription()
        {
            Debug.Log($"Имя {Name}");
            Debug.Log($"С {Stats.Strenght}, Л {Stats.Agility}, В {Stats.Stamina}");
            Debug.Log($"Макс зд {MaxHp}");
            Debug.Log($"Оружие {Weapon.Name}");
        }
        
    }
}