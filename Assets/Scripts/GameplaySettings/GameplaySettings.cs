using DataLayer;
using System;
using System.Collections.Generic;
using UnityEngine;

sealed class GameplaySettings : SingletonMonoBehaviour<GameplaySettings>
{
    [SerializeField]
    private GameplaySettingsDB _db;

    public static Dictionary<Type, EnemyData> Enemies => Instance._db.Enemies;
    public static Dictionary<Type, DefenseBuildingData> DefenseBuildings => Instance._db.DefenseBuildings;
    public static GameplaySettingsDB.PlayerSettings Player => Instance._db.Player;
    public static GameplaySettingsDB.GameSettings Game => Instance._db.Game;
}
