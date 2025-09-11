using Domain.Combat;
using Domain.Core;
using UnityEngine;
using Domain.UI;
using Domain.UI.Interfaces;

public class BattleHud : MonoBehaviour, IUIEvents
{
    [SerializeField] private BattleView heroView;
    [SerializeField] private BattleView monsterView;

    private Fighter _hero, _monster;

    public void OnBind(Fighter hero, Fighter monster)
    {
        _hero = hero; _monster = monster;
        heroView.BindFighter(hero);
        monsterView.BindFighter(monster);
    }

    public void OnMiss(Fighter attacker, Fighter defender, int roll, int defAgi, int round)
    {
        var view = ReferenceEquals(defender, _hero) ? heroView : monsterView;
        view.ShowMiss(Color.black);
    }

    public void OnHit(Fighter attacker, Fighter defender, int damage, int round)
    {
        var view = ReferenceEquals(defender, _hero) ? heroView : monsterView;
        view.ShowDamage(damage);
    }

    public void OnHpChanged(Fighter target)
    {
        if (ReferenceEquals(target, _hero)) heroView.UpdateHealth(_hero);
        else if (ReferenceEquals(target, _monster)) monsterView.UpdateHealth(_monster);
    }

    public void OnEnd(BattleResult result)
    {
    }
}