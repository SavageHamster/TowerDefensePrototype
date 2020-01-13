using DataLayer;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using VFX;

namespace Gameplay
{
    internal abstract class EnemyBase : MonoBehaviour
    {
        [SerializeField]
        private NavMeshAgent _navMeshAgent;
        [SerializeField]
        private Health _health;
        [SerializeField]
        private CollisionDamageDealer _damageDealer;

        private static readonly List<EnemyBase> _enabledEnemies = new List<EnemyBase>();
        private EnemyData _settings;

        public static List<EnemyBase> EnabledEnemies => _enabledEnemies;
        public int Health => _health.Current;

        private void OnEnable()
        {
            _settings = GameplaySettings.Enemies[this.GetType()];
            InitializeComponents();

            _health.Died += OnDied;

            _enabledEnemies.Add(this);
        }

        private void OnDisable()
        {
            _health.Died -= OnDied;

            _enabledEnemies.Remove(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<Player>();

            if (player != null)
            {
                Release();
            }
        }

        public static void ReleaseAll()
        {
            var enabledEmemiesCopy = new List<EnemyBase>(_enabledEnemies);

            foreach (var enemy in enabledEmemiesCopy)
            {
                enemy.Release();
            }
        }

        public void InitializePositions(Vector3 startingPosition, Vector3 targetPosition)
        {
            _navMeshAgent.Warp(startingPosition);
            _navMeshAgent.destination = targetPosition;
        }

        public void TakeDamage(int damage)
        {
            ActivateTakeDamageVFX();
            _health.TakeDamage(damage);
        }

        protected abstract void Release();

        private void OnDied()
        {
            Release();

            Data.Session.Gold.Set(Data.Session.Gold.Get() + _settings.scorePoints);
            Data.Session.KillsCount++;
        }

        private void ActivateTakeDamageVFX()
        {
            var takeDamageVFXObj = Pool.Instance.Get<TakeDamageVFX>();
            takeDamageVFXObj.transform.position = transform.position;
            takeDamageVFXObj.transform.SetParent(transform);
        }

        private void InitializeComponents()
        {
            var waveDependentStatsDelta = (Data.Session.EnemiesWaveNumber - 1) * GameplaySettings.Game.EnemiesStatsDeltaPerWave;

            _health.Initialize(_settings.health + waveDependentStatsDelta);
            _damageDealer.Initialize(_settings.damage + waveDependentStatsDelta);
        }
    }
}
