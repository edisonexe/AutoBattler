using Data;
using Domain.Core;
using Domain.Combat.Effects.Hero;

namespace Domain.Rules
{
    public static class HeroClassRules
    {
        private static WeaponRepository _weaponRepository;

        public static void Init(WeaponRepository weaponRepository) => _weaponRepository = weaponRepository;

        public static int HpPerLevel(HeroClass c) => c switch
        {
            HeroClass.Rogue     => 4,
            HeroClass.Warrior   => 5,
            HeroClass.Barbarian => 6,
            _ => 0
        };

        public static WeaponConfig StartingWeapon(HeroClass c) => c switch
        {
            HeroClass.Rogue     => _weaponRepository.GetByName("Dagger"),
            HeroClass.Warrior   => _weaponRepository.GetByName("Sword"),
            HeroClass.Barbarian => _weaponRepository.GetByName("Cudgel"),
            _ => null
        };


        public static void ApplyLevelBonuses(Hero hero, HeroClass c, int newLevel)
        {
            switch (c)
            {
                case HeroClass.Rogue:
                    if (newLevel == 1) hero.Attack.Add(new HiddenAttack());
                    if (newLevel == 2) hero.Stats.AgilityAdd(1);
                    if (newLevel == 3) hero.Attack.Add(new PoisonEffect());
                    break;

                case HeroClass.Warrior:
                    if (newLevel == 1) hero.Attack.Add(new ActionSurge());
                    if (newLevel == 2) hero.Defense.Add(new ShieldEffect());
                    if (newLevel == 3) hero.Stats.StrengthAdd(1);
                    break;

                case HeroClass.Barbarian:
                    if (newLevel == 1) hero.Attack.Add(new RageEffect());
                    if (newLevel == 2) hero.Defense.Add(new StoneSkin());
                    if (newLevel == 3) hero.Stats.StaminaAdd(1);
                    break;
            }
        }
    }
}