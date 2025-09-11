using Data;
using Domain.Campaign;
using Domain.Combat;
using Domain.Core;
using Domain.EntryPoint;
using Domain.Factories;
using Domain.Flow;
using Domain.Rules;
using Domain.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Domain.DI
{
    public class GameScope : LifetimeScope
    {
        [SerializeField] private WeaponRepository _weaponRepository;
        [SerializeField] private ClassSelectionView _classSelectionView;
        [SerializeField] private MonsterSetConfig _monsterSetConfig;
        [SerializeField] private EndPanelView _endPanelView;
        [SerializeField] private BattleHud _battleHud;
        [SerializeField] private WeaponSelectionView _weaponSelectionView;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_weaponRepository);
            builder.RegisterComponent(_classSelectionView);
            builder.RegisterComponent(_endPanelView);
            builder.RegisterComponent(_battleHud);
            builder.RegisterComponent(_weaponSelectionView);
            
            builder.RegisterInstance(_monsterSetConfig);
            
            builder.Register<HeroFactory>(Lifetime.Singleton);
            builder.Register<HeroProvider>(Lifetime.Singleton);
            builder.Register<MonsterFactory>(Lifetime.Singleton);
            builder.Register<ClassSelection>(Lifetime.Singleton);
            builder.Register<BattleManager>(Lifetime.Singleton);
            builder.Register<CampaignProgress>(Lifetime.Singleton);
            builder.Register<BattleFlow>(Lifetime.Singleton);
            builder.Register<RewardFlow>(Lifetime.Singleton);
            builder.Register<LevelUpFlow>(Lifetime.Singleton);

            builder.RegisterEntryPoint<GameEntryPoint>();
        }
    }
}