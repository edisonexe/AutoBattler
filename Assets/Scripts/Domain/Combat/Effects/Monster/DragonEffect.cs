using Domain.Combat.Effects.Interfaces;

namespace Domain.Combat.Effects.Monster
{
    public class DragonEffect : IStartTurnEffect
    {
        public int Priority => 50;
        public int AddStartTurnDamage(EffectContext ctx) => 
            ((ctx.Attacker.TurnsTaken + 1) % 3 == 0) ? 3 : 0;
    }
}