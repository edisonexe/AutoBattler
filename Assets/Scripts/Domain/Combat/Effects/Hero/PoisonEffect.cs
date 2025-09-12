using Domain.Combat.Effects.Interfaces;

namespace Domain.Combat.Effects.Hero
{
    // яд
    public class PoisonEffect : IAttackEffect
    {
        public string EffectName => "Poison";
        public int Priority => 100;
        public int ModifyOutgoingDamage(EffectContext ctx, int damage) => 
            (ctx.Attacker.TurnsTaken > 1) ? damage + ctx.Attacker.TurnsTaken - 1 : damage;
    }
}