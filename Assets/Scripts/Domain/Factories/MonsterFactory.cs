using System;
using Data;
using Domain.Core;
using UnityEngine;

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
            Stats stats = new Stats(conf.Strenght, conf.Agility, conf.Stamina);
            int maxHp = Math.Max(1, conf.MaxHp + stats.Stamina);
            Monster monster = new Monster(conf.Name, stats, maxHp, conf.Damage, conf.Reward);
            monster.PrintInfoAboutFighter();
            return monster;
        }
    }
}