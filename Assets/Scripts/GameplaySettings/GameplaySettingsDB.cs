using DataLayer;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
sealed class GameplaySettingsDB : ScriptableObject, ISerializationCallbackReceiver
{
    [Serializable]
    public sealed class EnemyTypeData
    {
        public MonoBehaviour monoBehaviour;
        public EnemyData data;
    }

    [Serializable]
    public sealed class DefenseBuildingTypeData
    {
        public MonoBehaviour monoBehaviour;
        public DefenseBuildingData data;
    }

    [Serializable]
    public class PlayerSettings
    {
        [SerializeField]
        private int _health;
        [SerializeField]
        private int _gold;

        public int Health => _health;
        public int Gold => _gold;
    }

    [Serializable]
    public class GameSettings
    {
        [SerializeField]
        private int _enemiesCountDeltaPerWave;
        [SerializeField]
        private int _enemiesWavesCooldownSec;
        [SerializeField]
        private int _enemiesStatsDeltaPerWave;

        public int EnemiesCountDeltaPerWave => _enemiesCountDeltaPerWave;
        public int EnemiesWavesCooldownSec => _enemiesWavesCooldownSec;
        public int EnemiesStatsDeltaPerWave => _enemiesStatsDeltaPerWave;
    }

    [SerializeField]
    private PlayerSettings _playerSettings;
    [SerializeField]
    private GameSettings _gameSettings;
    [SerializeField] 
    private List<EnemyTypeData> _enemiesSettings;
    [SerializeField]
    private List<DefenseBuildingTypeData> _defenseBuildingsSettings;

    private readonly Dictionary<Type, EnemyData> _enemiesSettingsDic = new Dictionary<Type, EnemyData>();
    private readonly Dictionary<Type, DefenseBuildingData> _defenseBuildingsSettingsDic = new Dictionary<Type, DefenseBuildingData>();

    public Dictionary<Type, EnemyData> Enemies => _enemiesSettingsDic;
    public Dictionary<Type, DefenseBuildingData> DefenseBuildings => _defenseBuildingsSettingsDic;
    public PlayerSettings Player => _playerSettings;
    public GameSettings Game => _gameSettings;

    public void OnAfterDeserialize()
    {
        try
        {
            if (_enemiesSettingsDic.Count == 0)
            {
                foreach (var enemyData in _enemiesSettings)
                {
                    if (!_enemiesSettingsDic.ContainsKey(enemyData.monoBehaviour.GetType()))
                    {
                        _enemiesSettingsDic.Add(enemyData.monoBehaviour.GetType(), enemyData.data);
                    }
                }
            }

            if (_defenseBuildingsSettingsDic.Count == 0)
            {
                foreach (var defenseBuildingData in _defenseBuildingsSettings)
                {
                    if (!_defenseBuildingsSettingsDic.ContainsKey(defenseBuildingData.monoBehaviour.GetType()))
                    {
                        _defenseBuildingsSettingsDic.Add(defenseBuildingData.monoBehaviour.GetType(), defenseBuildingData.data);
                    }
                }
            }
        }
        catch
        {
            Debug.LogError("Gameplay settings are incorrect!");
        }
    }

    public void OnBeforeSerialize() { }
}

