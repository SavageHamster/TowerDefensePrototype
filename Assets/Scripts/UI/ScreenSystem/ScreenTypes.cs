using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class ScreenTypes
    {
        private static readonly Dictionary<string, Type> screenTypesByNames = new Dictionary<string, Type>()
        {
            { "GameScreen", typeof(GameScreen) },
            { "MenuScreen", typeof(MenuScreen) },
        };

        public static Type GetByName(string name)
        {
            screenTypesByNames.TryGetValue(name, out Type result);

            if (result == null)
            {
                Debug.LogError("Can't find screen type by name in the dictionary.");
            }

            return result;
        }
    }
}