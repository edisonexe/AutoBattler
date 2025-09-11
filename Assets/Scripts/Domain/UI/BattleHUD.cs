using Domain.Campaign;
using Domain.Core;
using UnityEngine;
using Domain.UI;
using Domain.UI.Interfaces;

public class BattleHud : MonoBehaviour, IUIEvents
{
    [SerializeField] private BattleView _heroView;
    [SerializeField] private BattleView _monsterView;
    [SerializeField] private EndPanelView _endPanelView;
    [SerializeField] private CampaignProgress _campaign;
    
    private Fighter _hero, _monster;

    public void OnBind(Fighter hero, Fighter monster)
    {
        _hero = hero; _monster = monster;
        _heroView.BindFighter(hero);
        _monsterView.BindFighter(monster);
    }

    public void OnMiss(Fighter attacker, Fighter defender, int roll, int defAgi, int round)
    {
        var view = ReferenceEquals(defender, _hero) ? _heroView : _monsterView;
        view.ShowMiss(Color.black);
    }

    public void OnHit(Fighter attacker, Fighter defender, int damage, int round)
    {
        var view = ReferenceEquals(defender, _hero) ? _heroView : _monsterView;
        view.ShowDamage(damage);
    }

    public void OnHpChanged(Fighter target)
    {
        if (ReferenceEquals(target, _hero)) _heroView.UpdateHealth(_hero);
        else if (ReferenceEquals(target, _monster)) _monsterView.UpdateHealth(_monster);
    }
}