namespace Domain.Combat.Effects.Interfaces
{
    public interface ITypeRule : INamedEffect // иммунитет/уязвимость
    {
        int ApplyTypeRule(EffectContext ctx, int damage);
    }
}