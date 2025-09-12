using System.Collections.Generic;

namespace Domain.Combat
{
    public class BattleResult
    {
        public BattleOutcome Outcome { get; }
        public int Rounds { get; }

        private readonly List<string> _log;

        public BattleResult(BattleOutcome outcome, int rounds)
        {
            Outcome = outcome;
            Rounds = rounds;
        }
    }
}