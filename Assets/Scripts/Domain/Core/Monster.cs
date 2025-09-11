using Data;
using UnityEngine;

namespace Domain.Core
{
    public class Monster : Fighter
    {
        public Weapon Reward { get; private set; }

        public Monster(string name, Stats stats, int maxHp, Weapon weapon, Weapon reward)
            : base(name, stats, maxHp, weapon)
        {
            Reward = reward;
        }

        public void SetReward(Weapon reward)
        {
            if (reward != null) Reward = reward;
        }
    }
}