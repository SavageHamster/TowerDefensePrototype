using DataLayer;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    internal sealed class EnemyManager : SingletonMonoBehaviour<EnemyManager>
    {
        [SerializeField]
        private Transform _castle;
        [SerializeField]
        private List<Transform> _spawnPoints;

        private float _nextWaveTime;

        private void Update()
        {
            if (Time.time >= _nextWaveTime)
            {
                SpawnEnemies();

                _nextWaveTime = Time.time + GameplaySettings.Game.EnemiesWavesCooldownSec;
            }
        }

        private void SpawnEnemies()
        {
            var enemiesCount = CalculateEnemiesCount();

            for (int i = 1; i <= enemiesCount; i++)
            {
                SpawnEnemy(i);
            }

            Data.Session.EnemiesWaveNumber++;
        }

        private void SpawnEnemy(int number)
        {
            var enemyObj = GetRandomEnemyObj();
            var spawnPointIndex = _spawnPoints.Count % number;

            enemyObj.GetComponent<EnemyBase>().InitializePositions(_spawnPoints[spawnPointIndex].position, _castle.position);
        }

        private GameObject GetRandomEnemyObj()
        {
            // TODO: implement some algorithm.
            return UnityEngine.Random.value >= 0.5f ? 
                Pool.Instance.Get<SphericalEnemy>() : 
                Pool.Instance.Get<CapsularEnemy>();
        }

        private int CalculateEnemiesCount()
        {
            var minValue = Data.Session.EnemiesWaveNumber;
            var maxValue = Data.Session.EnemiesWaveNumber + GameplaySettings.Game.EnemiesCountDeltaPerWave;

            return UnityEngine.Random.Range(minValue, maxValue);
        }
    }
}
