using Domain.Core;
using Domain.Factories;
using Domain.Rules;
using Domain.UI;
using VContainer;
using VContainer.Unity;

namespace Domain.EntryPoint
{
    public class GameEntryPoint : IStartable
    {
        private readonly HeroFactory _factory;
        private readonly HeroProvider _heroProvider;
        private ClassSelectionView _classSelectionView;

        public GameEntryPoint(HeroFactory factory, HeroProvider heroProvider, ClassSelectionView classSelectionView)
        {
            _factory = factory;
            _heroProvider = heroProvider;
            _classSelectionView = classSelectionView;
        }

        public void Start()
        {
            _classSelectionView.OnClassPicked += OnClassPicked;
            _classSelectionView.ShowPanel();
        }

        private void OnClassPicked(HeroClass picked)
        {
            var stats = Utils.Utils.GetRandomStats();
            var hero = _factory.CreateHero("Hero", stats, picked);
            _heroProvider.Set(hero);

            _classSelectionView.OnClassPicked -= OnClassPicked;
            _classSelectionView.HidePanel();
        }
    }
}