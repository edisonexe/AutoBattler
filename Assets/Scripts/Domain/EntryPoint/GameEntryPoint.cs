using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Domain.Campaign;
using Domain.Combat;
using Domain.Core;
using Domain.Factories;
using Domain.Flow;
using Domain.Rules;
using Domain.UI;
using VContainer.Unity;

namespace Domain.EntryPoint
{
    public class GameEntryPoint : IStartable, IDisposable
    {
        private readonly HeroFactory _heroFactory;
        private readonly HeroProvider _heroProvider;
        private readonly ClassSelectionView _classSelectionView;
        private readonly EndPanelView _endPanelView;
        private readonly CampaignProgress _campaign;
        private readonly ClassSelection _classes;
        private readonly BattleFlow _battleFlow;
        private readonly RewardFlow _rewardFlow;
        private readonly LevelUpFlow _levelUpFlow;
        private readonly BattleHud _battleHud; 
        private readonly CancellationTokenSource _cts = new();

        private bool _heroCreated;

        public GameEntryPoint(HeroFactory heroFactory, HeroProvider heroProvider, ClassSelectionView classSelectionView,
            EndPanelView endPanelView, CampaignProgress campaign, ClassSelection classes, BattleFlow battleFlow,
            RewardFlow rewardFlow, LevelUpFlow levelUpFlow, BattleHud battleHud)
        {
            _heroFactory = heroFactory;
            _heroProvider = heroProvider;
            _classSelectionView = classSelectionView;
            _endPanelView = endPanelView;
            _campaign = campaign;
            _classes = classes;
            _battleFlow = battleFlow;
            _rewardFlow = rewardFlow;
            _levelUpFlow = levelUpFlow;
            _battleHud = battleHud;
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
                var stats = Utils.Utils.GetRandomStats();
                var hero  = _heroFactory.CreateHero("Hero", stats, picked);
                _heroProvider.Set(hero);
                _heroCreated = true;
                _classSelectionView.HidePanel();
                StartNextBattle().Forget();
                return;
            }

            var h = _heroProvider.Current;
            if (h != null && _classes.CanLevelUp(h))
            {
                _classes.ApplyPick(h, picked);
                _battleHud.OnHpChanged(h);
            }

            _classSelectionView.HidePanel();
            StartNextBattle().Forget();
        }

        private async UniTask StartNextBattle()
        {
            var hero = _heroProvider.Current;

            _battleHud.SetBattlesInfo(_campaign.CurrentBattle + 1, _campaign.TotalBattles);
            
            // 1) Запуск боя
            var (result, monster) = await _battleFlow.RunNextAsync(hero);
            
            if (result.Outcome != BattleOutcome.HeroWon) { _endPanelView.ShowPanel(result.Outcome); return; } // 2) Поражение
            if (_campaign.IsFinished) { _endPanelView.ShowPanel(BattleOutcome.HeroWon); return; }             // 3) Победа

            // 4) Предложение награды оружием
            bool rewardShown = await _rewardFlow.TryOfferWeaponAsync(hero, monster.Reward);
            if (rewardShown) _battleHud.OnHpChanged(hero);

            // 5) Прокачка (если доступна) или продолжение кампании
            bool canLevel = _levelUpFlow.CanLevelUp(hero);
            if (canLevel) { _levelUpFlow.ShowPicker(); return; }

            // 6) Если ещё не финал следующий бой; иначе финал
            if (!_campaign.IsFinished) await StartNextBattle();
            else _endPanelView.ShowPanel(BattleOutcome.HeroWon);
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

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}