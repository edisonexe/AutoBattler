using Domain.Core;

namespace Domain.Combat.Effects
{
    public class EffectContext
    {
        public Fighter Attacker { get; }
        public Fighter Defender { get; }

        public EffectContext(Fighter attacker, Fighter defender)
        {
            Attacker = attacker;
            Defender = defender;
        }
    }
}