using Domain.Core;

namespace Domain.UI.Interfaces
{
    public interface IUIEvents
    {
        void OnBind(Fighter hero, Fighter monster);
        void OnMiss(Fighter attacker, Fighter defender, int roll, int defAgi, int round);
        void OnHit(Fighter attacker, Fighter defender, int damage, int round);
        void OnHpChanged(Fighter target);
    }
}