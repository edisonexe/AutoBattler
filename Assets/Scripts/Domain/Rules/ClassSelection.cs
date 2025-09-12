using System;
using Domain.Core;

namespace Domain.Rules
{
    public class ClassSelection
    {
        public const int MaxTotalLevel = 3;

        public bool CanLevelUp(Hero hero) =>
            hero.TotalLevel < MaxTotalLevel;
        
        public void ApplyPick(Hero hero, HeroClass cls)
        {
            if (hero.TotalLevel >= MaxTotalLevel) return;

            var key = cls.ToString();
            var nextLevel = hero.GetLevel(key) + 1;
            
            int prevSta = hero.Stats.Stamina;
            int prevMaxHp = hero.MaxHp;
            
            hero.AddLevel(key, 1);
            HeroClassRules.ApplyLevelBonuses(hero, cls, nextLevel);
            
            int hpGainFromClass = HeroClassRules.HpPerLevel(cls);
            int staDelta = hero.Stats.Stamina - prevSta;
            
            int newMax = Math.Max(1, prevMaxHp + hpGainFromClass + staDelta);
            hero.SetMaxHp(newMax, true);
        }

    }
}