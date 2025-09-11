using Domain.Campaign;
using Domain.Combat;
using Domain.Core;
using Domain.Factories;
using Domain.Rules;
using Domain.UI;
using UnityEngine;
using VContainer.Unity;

namespace Domain.EntryPoint
{
    public class GameEntryPoint : IStartable
    {
        private readonly HeroFactory _heroFactory;
        private readonly HeroProvider _heroProvider;
        private readonly ClassSelectionView _classSelectionView;
        private readonly MonsterFactory _monsterFactory;
        private readonly BattleManager _battle;
        private readonly ClassSelection _classes;
        private readonly EndPanelView _endPanelView;
        private readonly BattleHud _battleHud;
        private readonly CampaignProgress _campaign;
        
        private bool _heroCreated;

        public GameEntryPoint(HeroFactory heroFactory, HeroProvider heroProvider, ClassSelectionView classSelectionView,
            MonsterFactory monsterFactory, BattleManager battle, ClassSelection classes,  EndPanelView endPanelView,
            BattleHud battleHud, CampaignProgress campaign)
        {
            _heroFactory = heroFactory;
            _heroProvider = heroProvider;
            _classSelectionView = classSelectionView;
            _monsterFactory = monsterFactory;
            _battle = battle;
            _classes = classes;
            _endPanelView = endPanelView;
            _battleHud = battleHud;
            _campaign = campaign;
        }

        public void Start()
        {
            _campaign.Reset();
            _classSelectionView.OnClassPicked += OnClassPicked;
            _endPanelView.OnPlayAgain += OnPlayAgainClicked;
            _classSelectionView.ShowPanel();
        }

        private void OnClassPicked(HeroClass picked)
        {
            if (!_heroCreated)
            {
                Debug.Log("Hero Created");
                var stats = Utils.Utils.GetRandomStats();
                Hero hero = _heroFactory.CreateHero("Hero", stats, picked);
                _heroProvider.Set(hero);
                _heroCreated = true;
                _classSelectionView.HidePanel();
                StartNextBattle();
                return;
            }
            var h = _heroProvider.Current;
            if (h != null && _classes.CanLevelUp(h))
            {
                _classes.ApplyPick(h, picked);
                _battleHud.OnHpChanged(_heroProvider.Current);
            }

            _classSelectionView.HidePanel();
            StartNextBattle();
        }
        
        private async void StartNextBattle()
        {
            if (!_campaign.BeginNextBattle())
            {
                _endPanelView.ShowPanel(BattleOutcome.HeroWon);
                return;
            }

            var hero = _heroProvider.Current;
            var monster = _monsterFactory.CreateMonster();

            hero.PrintEffects();
            monster.PrintEffects();

            var result = await _battle.FightAsync(hero, monster, uiEvents: _battleHud);

            if (result.Outcome == BattleOutcome.HeroWon)
            {
                if (_classes.CanLevelUp(hero))
                {
                    Debug.Log(string.Join("\n", result.Log));
                    Debug.Log($"\n Герой выиграл бой {_campaign.CurrentBattle}/{_campaign.TotalBattles}!");
                    _classSelectionView.ShowPanel();
                    return;
                }
                
                if (!_campaign.IsFinished)
                {
                    Debug.Log(string.Join("\n", result.Log));
                    Debug.Log($"\n Герой выиграл бой {_campaign.CurrentBattle}/{_campaign.TotalBattles}, но прокачка завершена!");
                    StartNextBattle();
                    return;
                }
            }
            
            _endPanelView.ShowPanel(result.Outcome);
            Debug.Log(string.Join("\n", result.Log));
        }

        
        private void OnPlayAgainClicked()
        {
            _campaign.Reset();
            _endPanelView.HidePanel();
            
            _heroCreated = false;
            _heroProvider.Set(null);
            
            _classSelectionView.OnClassPicked -= OnClassPicked;
            _classSelectionView.OnClassPicked += OnClassPicked;
            _classSelectionView.ShowPanel();
        }
    }
}