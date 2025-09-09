using System;
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
    }
}