using Domain.Combat.Effects.Interfaces;
using Domain.Core;

namespace Domain.Combat.Effects.Monster
{
    public class SkeletonEffect : ITypeRule
    {
        public string EffectName => "SkeletonEffect";
        public int Priority => 300;
        public int ApplyTypeRule(EffectContext ctx, int damage) => 
            (ctx.Attacker.GetDamageType() == DamageType.Bludgeoning ? damage * 2 : damage);
    }
}