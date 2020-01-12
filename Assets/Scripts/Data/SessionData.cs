namespace DataLayer
{
    public class SessionData
    {
        public int EnemiesWaveNumber { get; set; }
        public ObservableProperty<bool> IsGameOver { get; set; }
        public ObservableProperty<int> Gold { get; private set; }
        public ObservableProperty<int> PlayerHealth { get; private set; }
        public ObservableProperty<int> Score { get; private set; }

        public SessionData()
        {
            EnemiesWaveNumber = 1;

            IsGameOver = new ObservableProperty<bool>(false);
            Gold = new ObservableProperty<int>(GameplaySettings.Player.Gold);
            PlayerHealth = new ObservableProperty<int>(GameplaySettings.Player.Health);
            Score = new ObservableProperty<int>(0);
        }

        public void Reset()
        {
            EnemiesWaveNumber = 1;

            IsGameOver.Set(false);
            Gold.Set(GameplaySettings.Player.Gold);
            PlayerHealth.Set(GameplaySettings.Player.Health);
            Score.Set(0);
        }
    }
}
