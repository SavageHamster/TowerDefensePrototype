using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class ScenesController : SingletonMonoBehaviour<ScenesController>
{
    public enum Scenes
    {
        None,
        Preloader,
        Menu,
        Game
    }

    public event Action<Scenes> SceneLoaded;

    private Scenes _sceneIsLoading = Scenes.None;
    private float _sceneLoadStartTime;
    private readonly Dictionary<string, Action> _callbackBySceneType = new Dictionary<string, Action>();

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadScene(Scenes scene, Action callback = null)
    {
        if (scene == Scenes.None)
        {
            Debug.LogError("An attempt to load scene None.");
            return;
        }

        var sceneName = scene.ToString();
        var activeScene = SceneManager.GetActiveScene();

        if (activeScene != null && sceneName == activeScene.name)
        {
            Debug.LogError("An attempt to load already active scene.");
            return;
        }

        var preloaderSceneName = Scenes.Preloader.ToString();

        if (activeScene.name != preloaderSceneName)
        {
            SceneManager.LoadScene(Scenes.Preloader.ToString());
        }

        SceneManager.LoadSceneAsync(scene.ToString(), LoadSceneMode.Additive);
        _sceneIsLoading = scene;
        _sceneLoadStartTime = Time.time;

        if (callback != null)
        {
            if (!_callbackBySceneType.ContainsKey(sceneName))
            {
                _callbackBySceneType.Add(sceneName, callback);
            }
            else
            {
                _callbackBySceneType[sceneName] += callback;
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == _sceneIsLoading.ToString())
        {
            _sceneIsLoading = Scenes.None;
            _sceneLoadStartTime = 0f;
            SceneManager.UnloadSceneAsync(Scenes.Preloader.ToString());

            if (_callbackBySceneType.ContainsKey(scene.name))
            {
                _callbackBySceneType[scene.name].Invoke();
                _callbackBySceneType.Remove(scene.name);
            }

            Scenes sceneValue;

            try
            {
                sceneValue = (Scenes)Enum.Parse(typeof(Scenes), scene.name);
                SceneLoaded?.Invoke(sceneValue);
            }
            catch(Exception ex)
            {
                Debug.LogError(ex);
            }

            Debug.Log("Scene load time = " + (Time.time - _sceneLoadStartTime) + " sec.");
        }
    }
}
