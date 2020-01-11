namespace DataLayer
{
    public class SessionData
    {
        public int EnemiesWaveNumber { get; set; }
        public bool IsGameOver { get; set; }
        public ObservableProperty<int> Gold { get; private set; }
        public ObservableProperty<int> PlayerHealth { get; private set; }
        public ObservableProperty<int> Score { get; private set; }

        public SessionData()
        {
            Reset();
        }

        public void Reset()
        {
            EnemiesWaveNumber = 1;
            IsGameOver = false;

            Gold = new ObservableProperty<int>();
            Gold.Set(GameplaySettings.Player.Gold);

            PlayerHealth = new ObservableProperty<int>();
            PlayerHealth.Set(GameplaySettings.Player.Health);

            Score = new ObservableProperty<int>();
            Score.Set(0);
        }
    }
}
