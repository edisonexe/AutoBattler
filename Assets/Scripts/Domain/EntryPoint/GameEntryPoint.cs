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
using UnityEngine;
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
            Debug.Log($"[Entry] StartNextBattle → hero={(hero != null ? hero.Name : "NULL")}");

            _battleHud.SetBattlesInfo(_campaign.CurrentBattle + 1, _campaign.TotalBattles);
            
            // 1) Запускаем бой 
            var (result, monster) = await _battleFlow.RunNextAsync(hero);
            Debug.Log($"[Entry] BattleFlow.RunNextAsync returned → monster={(monster != null ? monster.Name : "NULL")}," +
                      $" outcome={(result != null ? result.Outcome.ToString() : "NULL")}," +
                      $" campaign={_campaign.CurrentBattle}/{_campaign.TotalBattles}, finished={_campaign.IsFinished}");
            
            if (monster == null) { _endPanelView.ShowPanel(BattleOutcome.HeroWon); return; }
            if (result.Outcome != BattleOutcome.HeroWon) { _endPanelView.ShowPanel(result.Outcome); return; } // 2) Поражение
            if (_campaign.IsFinished) { _endPanelView.ShowPanel(BattleOutcome.HeroWon); return; }             // 3) Победа

            // 4) Предложение награды оружием
            bool rewardShown = await _rewardFlow.TryOfferWeaponAsync(hero, monster.Reward);
            Debug.Log($"[Entry] Reward offered: shown={rewardShown}, reward={(monster.Reward != null ? monster.Reward.Name : "NULL")}");
            if (rewardShown) _battleHud.OnHpChanged(hero);

            // 5) Прокачка (если доступна) или продолжение кампании
            bool canLevel = _levelUpFlow.CanLevelUp(hero);
            Debug.Log($"[Entry] CanLevelUp={canLevel}");
            if (canLevel)
            {
                Debug.Log("[Entry] Show class selection view");
                _levelUpFlow.ShowPicker();
                return;
            }

            // 6) Если ещё не финал следующий бой; иначе финал
            if (!_campaign.IsFinished)
            {
                Debug.Log($"[Entry] Continue to next battle → campaign={_campaign.CurrentBattle}/{_campaign.TotalBattles}");
                await StartNextBattle();
            }
            else
            {
                Debug.Log("[Entry] Campaign finished after processing → show final");
                _endPanelView.ShowPanel(BattleOutcome.HeroWon);
            }
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