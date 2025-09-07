using System;
using Data;
using Domain.Core;
using Domain.Rules;

namespace Domain.Factories
{
    public class HeroFactory
    {
        private WeaponRepository _weaponRepository;

        public HeroFactory(WeaponRepository weaponRepository)
        {
            _weaponRepository = weaponRepository;
            HeroClassRules.Init(_weaponRepository);
        }

        public Hero CreateHero(string name, Stats stats, HeroClass cls)
        {
            int startHp = HeroClassRules.HpPerLevel(cls) + stats.Stamina;
            WeaponConfig weapon = HeroClassRules.StartingWeapon(cls);
            Hero hero = new Hero(name, stats, Math.Max(1, startHp), weapon);
            HeroClassRules.ApplyLevelBonuses(hero, cls, 1);
            hero.PrintDescription();
            return hero;
        }
    }
}