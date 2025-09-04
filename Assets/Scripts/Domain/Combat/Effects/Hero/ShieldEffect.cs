using Domain.Combat.Effects.Interfaces;

namespace Domain.Combat.Effects.Hero
{
    // щит
    public class ShieldEffect : IDefenseEffect
    {
        public int Priority => 200;
        public int ModifyIncomingDamage(EffectContext ctx, int damage) =>
            (ctx.Defender.Stats.Strenght > ctx.Attacker.Stats.Strenght) ? damage -= 3 : damage;
    }
}