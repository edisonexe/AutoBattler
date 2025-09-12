using Domain.Combat.Effects.Interfaces;

namespace Domain.Combat.Effects.Hero
{
    // щит
    public class ShieldEffect : IDefenseEffect
    {
        public string EffectName => "Shield";
        public int Priority => 200;
        public int ModifyIncomingDamage(EffectContext ctx, int damage) =>
            (ctx.Defender.Stats.Strength > ctx.Attacker.Stats.Strength) ? damage -= 3 : damage;
    }
}