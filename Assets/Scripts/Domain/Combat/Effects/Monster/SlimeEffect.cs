using Domain.Combat.Effects.Interfaces;
using Domain.Core;

namespace Domain.Combat.Effects.Monster
{
    public class SlimeEffect : ITypeRule
    {
        public string EffectName => "SlimeEffect";
        public int Priority => 300;
        public int ApplyTypeRule(EffectContext ctx, int damage)
        {
            if (ctx.Attacker.GetDamageType() == DamageType.Slashing)
            {
                int withoutWeapon = damage - (ctx.Attacker.Weapon?.BaseDamage ?? 0);
                return withoutWeapon < 0 ? 0 : withoutWeapon;
            }
            return damage;
        }
    }
}