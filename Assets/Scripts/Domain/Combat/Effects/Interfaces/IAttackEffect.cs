namespace Domain.Combat.Effects.Interfaces
{
    public interface IAttackEffect : IPriority, INamedEffect // смена исходящего урона
    {
        int ModifyOutgoingDamage(EffectContext ctx, int damage);
    }
}