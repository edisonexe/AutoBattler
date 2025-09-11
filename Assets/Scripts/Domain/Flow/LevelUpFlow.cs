using Domain.Core;
using Domain.Rules;
using Domain.UI;

namespace Domain.Flow
{
    public class LevelUpFlow
    {
        private readonly ClassSelection _classes;
        private readonly ClassSelectionView _classView;

        public LevelUpFlow(ClassSelection classes, ClassSelectionView classView)
        {
            _classes = classes;
            _classView = classView;
        }

        public bool CanLevelUp(Hero hero) => _classes.CanLevelUp(hero);

        public void ShowPicker() => _classView.ShowPanel();
    }
}