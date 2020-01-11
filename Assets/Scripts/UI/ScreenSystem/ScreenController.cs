using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    internal sealed class ScreenController : SingletonMonoBehaviour<ScreenController>
    {
        [SerializeField]
        private Canvas _mainCanvas;
        [SerializeField]
        private List<BaseScreen> _prefabs;

        private readonly List<BaseScreen> _activeScreens = new List<BaseScreen>();
        private readonly List<BaseScreen> _inactiveScreens = new List<BaseScreen>();
        private readonly Dictionary<Type, BaseScreen> _prefabsByType = new Dictionary<Type, BaseScreen>();

        private void Start()
        {
            InitScreensDict();
            ScreenController.Instance.Show<MenuScreen>();
        }

        public void Show<T>() where T : BaseScreen
        {
            var screen = _activeScreens.Find(_ => _ is T);
            _activeScreens.Remove(screen);

            if (screen == null)
            {
                screen = _inactiveScreens.Find(_ => _ is T);
                _inactiveScreens.Remove(screen);
            }

            if (screen == null)
            {
                _prefabsByType.TryGetValue(typeof(T), out BaseScreen screenPrototype);

                if (screenPrototype != null)
                {
                    var screenInstance = Instantiate(screenPrototype.gameObject, _mainCanvas.transform);
                    screen = screenInstance.GetComponent<T>();
                }
            }

            if (screen != null)
            {
                foreach (var activeScreen in _activeScreens)
                {
                    activeScreen.Hide();
                    _inactiveScreens.Add(activeScreen);
                }

                _activeScreens.Clear();
                screen.Show();
                _activeScreens.Add(screen);
            }
            else
            {
                Debug.LogError("Can't find screen " + typeof(T).ToString());
            }
        }

        public void Hide<T>()
        {
            var screen = _activeScreens.Find(_ => _ is T);

            if (screen != null)
            {
                screen.Hide();
                _inactiveScreens.Add(screen);
                _activeScreens.Remove(screen);
            }
        }

        private void InitScreensDict()
        {
            foreach (var prefab in _prefabs)
            {
                var screenType = ScreenTypes.GetByName(prefab.name);

                if (screenType != null)
                {
                    _prefabsByType.Add(screenType, prefab);
                }
            }
        }
    }
}
