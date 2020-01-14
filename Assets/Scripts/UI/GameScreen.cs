using DataLayer;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    internal sealed class GameScreen : BaseScreen
    {
        [SerializeField]
        private Button _backToMenuButton;
        [SerializeField]
        private TMPro.TextMeshProUGUI _healthLabel;
        [SerializeField]
        private TMPro.TextMeshProUGUI _goldLabel;
        [SerializeField]
        private TMPro.TextMeshProUGUI _killsCountLabel;
        [SerializeField]
        private GameObject _gameOverObj;
        [SerializeField]
        private Button _tryAgainButton;
        [SerializeField]
        private Button _gameOverBackToMenuButton;
        
        private void Start()
        {
            SubscribeOnUIEvents();

            Data.Session.Gold.Subscribe(OnGoldValueChanged);
            Data.Session.PlayerHealth.Subscribe(OnPlayerHealthChanged);
            Data.Session.IsGameOver.Subscribe(OnGameOver);
        }

        private void OnDestroy()
        {
            UnsubscribeFromUIEvents();

            Data.Session.Gold.Unsubscribe(OnGoldValueChanged);
            Data.Session.PlayerHealth.Unsubscribe(OnPlayerHealthChanged);
            Data.Session.IsGameOver.Unsubscribe(OnGameOver);
        }

        protected override void OnShow()
        {
            ResetScreen();

            base.OnShow();
        }

        private void OnGameOver()
        {
            if (Data.Session.IsGameOver.Get())
            {
                _gameOverObj.SetActive(true);
                _killsCountLabel.text = Data.Session.KillsCount.ToString();
            }
        }

        private void OnPlayerHealthChanged()
        {
            _healthLabel.text = Data.Session.PlayerHealth.Get().ToString();
        }

        private void OnGoldValueChanged()
        {
            _goldLabel.text = Data.Session.Gold.Get().ToString();
        }

        private void OnGameOverBackToMenuButtonClick()
        {
            Data.Session.IsGameOver.Set(true);
            OpenMenu();
        }

        private void OnBackToMenuButtonClick()
        {
            Data.Session.IsGameOver.Set(true);
            OpenMenu();
        }

        private void OnTryAgainButtonClick()
        {
            SessionsManager.Instance.RestartSession();
            ResetScreen();
        }

        private void ResetScreen()
        {
            _gameOverObj.SetActive(false);
        }

        private void OpenMenu()
        {
            ScenesController.Instance.LoadScene(ScenesController.Scene.Menu, () => ScreenController.Instance.Show<MenuScreen>());
        }

        private void SubscribeOnUIEvents()
        {
            _backToMenuButton.onClick.AddListener(OnBackToMenuButtonClick);
            _gameOverBackToMenuButton.onClick.AddListener(OnGameOverBackToMenuButtonClick);
            _tryAgainButton.onClick.AddListener(OnTryAgainButtonClick);
        }

        private void UnsubscribeFromUIEvents()
        {
            _backToMenuButton.onClick.RemoveListener(OnBackToMenuButtonClick);
            _gameOverBackToMenuButton.onClick.RemoveListener(OnGameOverBackToMenuButtonClick);
            _tryAgainButton.onClick.RemoveListener(OnTryAgainButtonClick);
        }
    }
}