using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    internal sealed class MenuScreen : BaseScreen
    {
        [SerializeField]
        private Button _playButton;
        [SerializeField]
        private Button _exitButton;

        private void Start()
        {
            SubscribeOnUIEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromUIEvents();
        }

        private void SubscribeOnUIEvents()
        {
            _playButton.onClick.AddListener(OnPlayButtonClick);
            _exitButton.onClick.AddListener(OnExitButtonClick);
        }

        private void UnsubscribeFromUIEvents()
        {
            _playButton.onClick.RemoveListener(OnPlayButtonClick);
            _exitButton.onClick.RemoveListener(OnExitButtonClick);
        }

        private void OnExitButtonClick()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
        }

        private void OnPlayButtonClick()
        {
            ScenesController.Instance.LoadScene(ScenesController.Scene.Game, () => ScreenController.Instance.Show<GameScreen>());
        }
    }
}
