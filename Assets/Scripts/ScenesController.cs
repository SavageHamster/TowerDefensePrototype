using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesController : SingletonMonoBehaviour<ScenesController>
{
    public enum Scene
    {
        None,
        Preloader,
        Menu,
        Loading,
        Game
    }

    public event Action<Scene> SceneLoaded;

    private Scene _sceneIsLoading = Scene.None;
    private Scene _loadingScene = Scene.None;
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

    public void LoadScene(Scene scene, Action callback = null)
    {
        if (scene == Scene.None)
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

        var preloaderSceneName = Scene.Preloader.ToString();
        
        if (activeScene.name != preloaderSceneName)
        {
            _loadingScene = Scene.Loading;
            SceneManager.LoadScene(_loadingScene.ToString());
        }
        else
        {
            _loadingScene = Scene.Preloader;
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

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == _sceneIsLoading.ToString())
        {
            _sceneIsLoading = Scene.None;
            _sceneLoadStartTime = 0f;
            SceneManager.UnloadSceneAsync(_loadingScene.ToString());

            if (_callbackBySceneType.ContainsKey(scene.name))
            {
                _callbackBySceneType[scene.name].Invoke();
                _callbackBySceneType.Remove(scene.name);
            }

            Scene sceneValue;

            try
            {
                sceneValue = (Scene)Enum.Parse(typeof(Scene), scene.name);
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
