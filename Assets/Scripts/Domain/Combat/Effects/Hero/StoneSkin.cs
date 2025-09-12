using Domain.Combat.Effects.Interfaces;

namespace Domain.Combat.Effects.Hero
{
    // каменная кожа
    public class StoneSkin : IDefenseEffect 
    {
        public string EffectName => "StoneSkin";
        public int Priority => 200;
        public int ModifyIncomingDamage(EffectContext ctx, int damage) =>
            damage - ctx.Defender.Stats.Stamina;
    }
}