using Domain.Combat.Effects.Interfaces;

namespace Domain.Combat.Effects.Monster
{
    public class DragonEffect : IAttackEffect
    {
        public string EffectName => "DragonEffect";
        public int ModifyOutgoingDamage(EffectContext ctx, int damage) => 
            (ctx.Attacker.TurnsTaken % 3 == 0) ? damage + 3 : damage;
    }
}