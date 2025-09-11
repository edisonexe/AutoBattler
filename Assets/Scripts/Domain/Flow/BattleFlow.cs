using Cysharp.Threading.Tasks;
using Domain.Campaign;
using Domain.Combat;
using Domain.Core;
using Domain.Factories;

namespace Domain.Flow
{
    public class BattleFlow
    {
        private readonly CampaignProgress _campaign;
        private readonly MonsterFactory _monsterFactory;
        private readonly BattleManager _battle;
        private readonly BattleHud _hud;

        public BattleFlow(CampaignProgress campaign, MonsterFactory monsterFactory, BattleManager battle, BattleHud hud)
        {
            _campaign = campaign;
            _monsterFactory = monsterFactory;
            _battle = battle;
            _hud = hud;
        }

        /// Запускает следующий бой. Возвращает (result, monster) для последующей обработки.
        public async UniTask<(BattleResult result, Monster monster)> RunNextAsync(Hero hero)
        {
            if (!_campaign.CanNextBattle())
                return (new BattleResult(BattleOutcome.HeroWon, 0, new()), null);

            var monster = _monsterFactory.CreateMonster();
            var result  = await _battle.FightAsync(hero, monster, uiEvents: _hud);

            return (result, monster);
        }
    }
}