namespace Domain.Combat.Effects.Interfaces
{
    public interface IDefenseEffect : IPriority // защита от входящего урона
    {
        int ModifyIncomingDamage(EffectContext ctx, int damage);
    }
}