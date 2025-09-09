using System.Collections.Generic;

namespace Domain.Combat
{
    public class BattleResult
    {
        public BattleOutcome Outcome { get; }
        public int Rounds { get; }
        public IReadOnlyList<string> Log => _log;

        private readonly List<string> _log;

        public BattleResult(BattleOutcome outcome, int rounds, List<string> log)
        {
            Outcome = outcome;
            Rounds = rounds;
            _log = log;
        }
    }
}