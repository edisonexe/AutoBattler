using Domain.Combat.Effects.Interfaces;

namespace Domain.Combat.Effects.Hero
{
    // ярость
    public class RageEffect : IAttackEffect
    {
        public int Priority => 100;

        public int ModifyOutgoingDamage(EffectContext ctx, int damage)
        {
            if (ctx.Attacker.TurnsTaken < 3)
                return damage + 2; // первые 3 хода (0,1,2) +2
            else
                return damage - 1; // начиная с 3-го хода -1
        }
    }
}