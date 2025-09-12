using System;
using System.Text;
using Data;
using Domain.Core;

namespace Utils
{
    public static class Utils
    {
        private static readonly Random _rng = new Random();
        
        public static int RandomInt(int minInclusive, int maxInclusive)
        {
            return _rng.Next(minInclusive, maxInclusive + 1);
        }
        
        public static MonsterClassConfig GetRandomMonsterClass(MonsterSetConfig set)
        {
            if (set.Monsters == null || set.Monsters.Length == 0)
                throw new ArgumentException("Нет доступных монстров для выбора.");

            int idx = _rng.Next(0, set.Monsters.Length);
            return set.Monsters[idx];
        }
        
        public static Stats GetRandomStats()
        {
            int str = _rng.Next(1, 4);
            int agi = _rng.Next(1, 4);
            int sta = _rng.Next(1, 4);
            return new Stats(str, agi, sta);
        }
        
        public static string UpdateEffectsText(Fighter f)
        {
            var sb = new StringBuilder();

            void add(string s)
            {
                if (sb.Length > 0) 
                    sb.AppendLine();
                sb.Append(s);
            }

            foreach (var e in f.Attack) add(e.EffectName);
            foreach (var e in f.Defense) add(e.EffectName);
            foreach (var e in f.TypeRules) add(e.EffectName);

            return sb.Length == 0 ? "-" : sb.ToString();
        }

    }
}