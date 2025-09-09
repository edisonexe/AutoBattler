using Domain.Combat;
using Domain.Core;

namespace Domain.Rules
{
    public sealed class LevelProgression
    {
        private readonly BattleManager _battle;
        private readonly ClassSelection _classes;

        public LevelProgression(BattleManager battle, ClassSelection classes)
        {
            _battle = battle; 
            _classes = classes;
        }

        public BattleResult FightAndMaybeLevelUp(Hero hero, Fighter monster, HeroClass? pickedIfWin)
        {
            var result = _battle.Fight(hero, monster);

            if (result.Outcome == BattleOutcome.HeroWon && pickedIfWin.HasValue && _classes.CanLevelUp(hero))
                _classes.ApplyPick(hero, pickedIfWin.Value);
                
            return result;
        }
    }
}