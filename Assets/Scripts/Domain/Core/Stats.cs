using System;

namespace Domain.Core
{
    public class Stats
    {
        public int Strength { get; private set; }
        public int Agility { get; private set; }
        public int Stamina { get; private set; }
        
        public Stats(int strength, int agility, int stamina)
        {
            SetStats(strength, agility, stamina);
        }

        public void SetStats(int strength, int agility, int stamina)
        {
            Strength = strength < 0 ? 0 : strength;
            Agility = agility  < 0 ? 0 : agility;
            Stamina = stamina  < 0 ? 0: stamina;
        }

        public void StrengthAdd(int value) => Strength = Math.Max(0, Strength + value);
        public void AgilityAdd(int value) => Agility = Math.Max(0, Agility + value);
        public void StaminaAdd(int value) => Stamina = Math.Max(0, Stamina + value);
    }
}