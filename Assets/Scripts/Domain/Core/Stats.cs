using System;

namespace Domain.Core
{
    public class Stats
    {
        public int Strenght { get; private set; }
        public int Agility { get; private set; }
        public int Stamina { get; private set; }
        
        public Stats(int strenght, int agility, int stamina)
        {
            SetStats(strenght, agility, stamina);
        }

        public void SetStats(int strenght, int agility, int stamina)
        {
            Strenght = strenght < 0 ? 0 : strenght;
            Agility = agility  < 0 ? 0 : agility;
            Stamina = stamina  > 0 ? stamina : 0;;
        }

        public void StrengthAdd(int value) => Strenght = Math.Max(0, Strenght + value);
        public void AgilityAdd(int value) => Agility = Math.Max(0, Agility + value);
        public void StaminaAdd(int value) => Stamina = Math.Max(0, Stamina + value);
    }
}