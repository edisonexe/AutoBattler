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

        public void SetStats(int strenght, int agility, int stamina)
        {
            Strength = strenght < 0 ? 0 : strenght;
            Agility = agility  < 0 ? 0 : agility;
            Stamina = stamina  > 0 ? stamina : 0;
        }

        public void StrengthAdd(int value) => Strength = Math.Max(0, Strength + value);
        public void AgilityAdd(int value) => Agility = Math.Max(0, Agility + value);
        public void StaminaAdd(int value) => Stamina = Math.Max(0, Stamina + value);
    }
}