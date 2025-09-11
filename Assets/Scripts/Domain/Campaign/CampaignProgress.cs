namespace Domain.Campaign
{
    public class CampaignProgress
    {
        private const int DefaultTotalBattles = 5;
        private readonly int _totalBattles;
        private int _currentBattle;

        public CampaignProgress()
        {
            _totalBattles = DefaultTotalBattles;
            _currentBattle = 0;
        }

        public int TotalBattles => _totalBattles;
        public int CurrentBattle => _currentBattle;
        public bool IsFinished => _currentBattle >= _totalBattles;

        public bool BeginNextBattle()
        {
            if (IsFinished) return false;
            _currentBattle++;
            return true;
        }

        public void Reset() => _currentBattle = 0;
    }
}