using DataLayer;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay
{
    internal sealed class EnemyManager : SingletonMonoBehaviour<EnemyManager>
    {
        [SerializeField]
        private Transform _castle;
        [SerializeField]
        private List<Transform> _spawnPoints;

        private float _nextWaveTime;

        private void Start()
        {
            Data.Session.IsGameOver.Subscribe(OnGameOverChanged);
        }

        private void OnDestroy()
        {
            Data.Session.IsGameOver.Unsubscribe(OnGameOverChanged);
        }

        private void Update()
        {
            if (!Data.Session.IsGameOver.Get() && Time.time >= _nextWaveTime)
            {
                SpawnEnemies();

                _nextWaveTime = Time.time + GameplaySettings.Game.EnemiesWavesCooldownSec;
            }
        }

        private void OnGameOverChanged()
        {
            if (Data.Session.IsGameOver.Get())
            {
                EnemyBase.ReleaseAll();
                _nextWaveTime = 0f;
            }
        }

        private void SpawnEnemies()
        {
            var enemiesCount = CalculateEnemiesCount();

            for (int i = 0; i < enemiesCount; i++)
            {
                SpawnEnemy(i);
            }

            Data.Session.EnemiesWaveNumber++;
        }

        private void SpawnEnemy(int number)
        {
            var enemyObj = GetRandomEnemyObj();
            var enemyComponent = enemyObj.GetComponent<EnemyBase>();
            var spawnPointIndex =  number % _spawnPoints.Count;

            enemyComponent.InitializePositions(_spawnPoints[spawnPointIndex].position, _castle.position);
        }

        private GameObject GetRandomEnemyObj()
        {
            // TODO: implement some algorithm.
            return Random.value >= 0.5f ? 
                Pool.Instance.Get<SphericalEnemy>() : 
                Pool.Instance.Get<CapsularEnemy>();
        }

        private int CalculateEnemiesCount()
        {
            var minValue = Data.Session.EnemiesWaveNumber;
            var maxValue = Data.Session.EnemiesWaveNumber + GameplaySettings.Game.EnemiesCountMaxDeltaPerWave;

            return Random.Range(minValue, maxValue);
        }
    }
}
