using Domain.Combat.Effects.Interfaces;

namespace Domain.Combat.Effects.Hero
{
    // порыв к действию
    public class ActionSurge : IAttackEffect
    {
        public string EffectName => "ActionSurge";
        public int Priority => 100;
        public int ModifyOutgoingDamage(EffectContext ctx, int damage) => 
            (ctx.Attacker.TurnsTaken == 1) ? damage + ctx.Attacker.Weapon.BaseDamage : damage;
    }
}