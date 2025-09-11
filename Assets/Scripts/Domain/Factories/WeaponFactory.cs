using Data;
using Domain.Core;

namespace Domain.Factories
{
    public static class WeaponFactory
    {
        public static Weapon FromConfig(WeaponConfig config)
        {
            if (config == null)
                throw new System.ArgumentNullException(nameof(config));
            
            return new Weapon(config.Name, config.BaseDamage, config.Type, config.Sprite);
        }
    }
}