using System;

namespace Domain.Core
{
    public class Weapon
    {
        public string Name { get; set; }
        public int BaseDamage { get; set; }
        public DamageType Type { get; set; }
        
        public Weapon(string name, int baseDamage, DamageType type)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Weapon name cannot be empty.", nameof(name));
            if (baseDamage < 0)
                throw new ArgumentOutOfRangeException(nameof(baseDamage), "BaseDamage must be >= 0.");
            
            Name = name;
            BaseDamage = baseDamage;
            Type = type;
        }
    }
}