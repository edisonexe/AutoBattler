using Domain.Core;

namespace Domain.Combat.Effects
{
    public class EffectContext
    {
        public Fighter Attacker { get; set; }
        public Fighter Defender { get; set; }
    }
}