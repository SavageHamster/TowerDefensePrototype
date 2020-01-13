namespace DataLayer
{
    public class SessionData
    {
        public int EnemiesWaveNumber { get; set; }
        public int KillsCount { get; set; }
        public ObservableProperty<bool> IsGameOver { get; set; }
        public ObservableProperty<int> Gold { get; private set; }
        public ObservableProperty<int> PlayerHealth { get; private set; }

        public SessionData()
        {
            IsGameOver = new ObservableProperty<bool>();
            Gold = new ObservableProperty<int>();
            PlayerHealth = new ObservableProperty<int>();

            Reset();
        }

        public void Reset()
        {
            EnemiesWaveNumber = 1;
            KillsCount = 0;

            IsGameOver.Set(false);
            Gold.Set(GameplaySettings.Player.Gold);
            PlayerHealth.Set(GameplaySettings.Player.Health);
        }
    }
}
