namespace Domain.Combat.Effects.Interfaces
{
    public interface IStartTurnEffect : IPriority // урон в начале хода
    {
        int AddStartTurnDamage(EffectContext ctx);
    }
}