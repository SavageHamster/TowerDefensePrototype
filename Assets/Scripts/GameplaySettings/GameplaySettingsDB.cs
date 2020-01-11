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

        public int EnemiesCountDeltaPerWave => _enemiesCountDeltaPerWave;
        public int EnemiesWavesCooldownSec => _enemiesWavesCooldownSec;
    }

    [SerializeField]
    private PlayerSettings _playerSettings;
    [SerializeField] 
    private List<EnemyTypeData> _enemiesSettings;
    [SerializeField]
    private GameSettings _gameSettings;

    private readonly Dictionary<Type, EnemyData> _enemiesSettingsDic = new Dictionary<Type, EnemyData>();

    public Dictionary<Type, EnemyData> Enemies => _enemiesSettingsDic;
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
        }
        catch
        {
            Debug.LogError("Gameplay settings are incorrect!");
        }
    }

    public void OnBeforeSerialize() { }
}

