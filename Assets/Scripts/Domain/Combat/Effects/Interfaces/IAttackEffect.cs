namespace Domain.Combat.Effects.Interfaces
{
    public interface IAttackEffect : INamedEffect // смена исходящего урона
    {
        int ModifyOutgoingDamage(EffectContext ctx, int damage);
    }
}