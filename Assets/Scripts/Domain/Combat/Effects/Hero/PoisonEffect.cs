using Domain.Combat.Effects.Interfaces;

namespace Domain.Combat.Effects.Hero
{
    public class PoisonEffect : IAttackEffect
    {
        public int Priority => 100;
        public int ModifyOutgoingDamage(EffectContext ctx, int damage) => 
            (ctx.Attacker.TurnsTaken > 0) ? damage + ctx.Attacker.TurnsTaken : damage;
    }
}