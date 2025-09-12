using Domain.Combat.Effects.Interfaces;

namespace Domain.Combat.Effects.Hero
{
    // скрытая атака
    public class HiddenAttack : IAttackEffect
    {
        public string EffectName => "HiddenAttack";
        public int Priority => 100;
        public int ModifyOutgoingDamage(EffectContext ctx, int damage) => 
            (ctx.Attacker.Stats.Agility > ctx.Defender.Stats.Agility) ? damage + 1 : damage;
    }
}