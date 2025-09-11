using System;
using Data;
using Domain.Core;
using Domain.Rules;

namespace Domain.Factories
{
    public class MonsterFactory
    {
        private MonsterSetConfig _monsterSet;

        public MonsterFactory(MonsterSetConfig monsterSet)
        {
            _monsterSet  = monsterSet;
        }
        
        public Monster CreateMonster()
        {
            MonsterClassConfig conf = Utils.Utils.GetRandomMonsterClass(_monsterSet);
            Stats stats = new Stats(conf.Strength, conf.Agility, conf.Stamina);
            int maxHp = Math.Max(1, conf.MaxHp + stats.Stamina);
            var weapon = WeaponFactory.FromConfig(conf.WeaponConfig);
            var reward = WeaponFactory.FromConfig(conf.Reward);
            
            Monster monster = new Monster(conf.Name, stats, maxHp, weapon, reward);
            MonsterClassRules.Apply(monster, conf.MonsterClass);
            monster.PrintInfoAboutFighter();
            return monster;
        }
    }
}