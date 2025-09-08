using Data;
using UnityEngine;

namespace Domain.Core
{
    public class Monster : Fighter
    {
        public WeaponConfig Reward { get; private set; }

        public Monster(string name, Stats stats, int maxHp, WeaponConfig weapon, WeaponConfig reward)
            : base(name, stats, maxHp, weapon)
        {
            Reward = reward;
        }

        public void SetReward(WeaponConfig reward)
        {
            if (reward != null) Reward = reward;
        }
    }
}