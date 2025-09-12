using Domain.Core;

namespace Domain.UI.Interfaces
{
    public interface IUIEvents
    {
        void OnBind(Fighter hero, Fighter monster);
        void OnMiss(Fighter defender);
        void OnHit(Fighter defender, int damage);
        void OnHpChanged(Fighter target);
    }
}