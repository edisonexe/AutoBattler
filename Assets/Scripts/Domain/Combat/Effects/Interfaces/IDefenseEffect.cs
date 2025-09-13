namespace Domain.Combat.Effects.Interfaces
{
    public interface IDefenseEffect : INamedEffect // защита от входящего урона
    {
        int ModifyIncomingDamage(EffectContext ctx, int damage);
    }
}