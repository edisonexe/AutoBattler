namespace Domain.Combat.Effects.Interfaces
{
    public interface ITypeRule : IPriority // иммунитет/уязвимость
    {
        int ApplyTypeRule(EffectContext ctx, int damage);
    }
}