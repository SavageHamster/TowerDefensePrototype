using DataLayer;
using System;

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

    private void OnSceneLoaded(ScenesController.Scene scene)
    {
        switch (scene)
        {
            case ScenesController.Scene.Menu:
                break;
            case ScenesController.Scene.Game:
                RestartSession();
                break;
            case ScenesController.Scene.None:
            case ScenesController.Scene.Preloader:
                break;
        }
    }
}
