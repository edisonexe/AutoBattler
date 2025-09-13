using Domain.Core;
using UnityEngine;
using Domain.UI;
using Domain.UI.Interfaces;
using TMPro;

public class BattleHud : MonoBehaviour, IUIEvents
{
    [SerializeField] private BattleView _heroView;
    [SerializeField] private BattleView _monsterView;
    [SerializeField] private EndPanelView _endPanelView;
    [SerializeField] private TMP_Text _heroEffectsText;
    [SerializeField] private TMP_Text _monsterEffectsText;
    [SerializeField] private TMP_Text _roundText; 
    private int _battleIndex = 1;
    private int _battleTotal = 5;  
    
    private Fighter _hero, _monster;

    public void OnBind(Fighter hero, Fighter monster)
    {
        _hero = hero; _monster = monster;
        _heroView.BindFighter(hero);
        _monsterView.BindFighter(monster);
        _heroEffectsText.text   = Utils.Utils.UpdateEffectsText(_hero);
        _monsterEffectsText.text= Utils.Utils.UpdateEffectsText(_monster);
    }

    public void OnMiss(Fighter defender)
    {
        var view = ReferenceEquals(defender, _hero) ? _heroView : _monsterView;
        view.ShowMiss(Color.black);
    }

    public void OnHit(Fighter defender, int damage)
    {
        var view = ReferenceEquals(defender, _hero) ? _heroView : _monsterView;
        view.ShowDamage(damage);
    }

    public void OnHpChanged(Fighter target)
    {
        if (ReferenceEquals(target, _hero)) _heroView.UpdateHealth(_hero);
        else if (ReferenceEquals(target, _monster)) _monsterView.UpdateHealth(_monster);
    }

    public void SetBattlesInfo(int currentIndex, int total)
    {
        _battleIndex = currentIndex;
        _battleTotal = total;
        UpdateRoundText();
    }
    
    private void UpdateRoundText()
    {
        if (_roundText != null)
            _roundText.text = $"BATTLE {_battleIndex}/{_battleTotal}";
    }
    
    
}