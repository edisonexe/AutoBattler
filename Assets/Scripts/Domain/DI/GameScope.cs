using Data;
using Domain.Core;
using Domain.EntryPoint;
using Domain.Factories;
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
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_weaponRepository);
            builder.RegisterComponent(_classSelectionView);
            builder.RegisterComponent(_monsterSetConfig);
            builder.Register<MonsterFactory>(Lifetime.Singleton);
            builder.Register<HeroFactory>(Lifetime.Singleton);
            builder.Register<HeroProvider>(Lifetime.Singleton);
            builder.Register<ClassSelection>(Lifetime.Singleton);

            builder.RegisterEntryPoint<GameEntryPoint>();
        }
    }
}