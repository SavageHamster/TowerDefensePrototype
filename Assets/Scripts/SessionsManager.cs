using DataLayer;
using System;

namespace Gameplay
{
    public sealed class SessionsManager : SingletonMonoBehaviour<SessionsManager>
    {
        public event Action SessionStarted;

        private void Start()
        {
            ScenesController.Instance.SceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            if (ScenesController.Instance != null)
            {
                ScenesController.Instance.SceneLoaded -= OnSceneLoaded;
            }
        }

        public void FinishSession()
        {
            Data.Session.IsGameOver.Set(true);
        }

        public void RestartSession()
        {
            Data.Session.Reset();

            SessionStarted?.Invoke();
        }

        private void OnSceneLoaded(ScenesController.Scenes scene)
        {
            switch (scene)
            {
                case ScenesController.Scenes.Menu:
                    break;
                case ScenesController.Scenes.Game:
                    RestartSession();
                    break;
                case ScenesController.Scenes.None:
                case ScenesController.Scenes.Preloader:
                    break;
            }
        }
    }
}
