using Data;
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

        private bool _heroCreated;

        public GameEntryPoint(HeroFactory heroFactory, HeroProvider heroProvider, ClassSelectionView classSelectionView,
            MonsterFactory monsterFactory, BattleManager battle, ClassSelection classes)
        {
            _heroFactory = heroFactory;
            _heroProvider = heroProvider;
            _classSelectionView = classSelectionView;
            _monsterFactory = monsterFactory;
            _battle = battle;
            _classes = classes;
        }

        public void Start()
        {
            _classSelectionView.OnClassPicked += OnClassPicked;
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
            Debug.Log("Hero Lvlup");
            // выбор класса после победы
            var h = _heroProvider.Current;
            if (h != null && _classes.CanLevelUp(h))
            {
                _classes.ApplyPick(h, picked);
            }

            _classSelectionView.HidePanel();
            StartNextBattle();
        }

        private void StartNextBattle()
        {
            var hero = _heroProvider.Current;
            var monster = _monsterFactory.CreateMonster();
            
            var result = _battle.Fight(hero, monster);

            if (result.Outcome == BattleOutcome.HeroWon && _classes.CanLevelUp(hero))
            {
                Debug.Log(string.Join("\n", result.Log));
                _classSelectionView.ShowPanel();
            }
            else
            {
                Debug.Log("\n Герой проиграл");
                Debug.Log(string.Join("\n", result.Log));
            }
        }
    }
}