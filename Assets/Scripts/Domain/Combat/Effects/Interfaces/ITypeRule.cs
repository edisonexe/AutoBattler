namespace Domain.Combat.Effects.Interfaces
{
    public interface ITypeRule : IPriority, INamedEffect // иммунитет/уязвимость
    {
        int ApplyTypeRule(EffectContext ctx, int damage);
    }
}