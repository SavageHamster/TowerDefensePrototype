using DataLayer;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay
{
    internal abstract class EnemyBase : MonoBehaviour
    {
        [SerializeField]
        private NavMeshAgent _navMeshAgent;
        [SerializeField]
        private Health _health;
        [SerializeField]
        private DamageDealer _damageDealer;

        private static readonly List<EnemyBase> _enabledEnemies = new List<EnemyBase>();

        private EnemyData _settings;

        private void OnEnable()
        {
            _settings = GameplaySettings.Enemies[this.GetType()];

            _health.Initialize(_settings.health);
            _health.Died += OnDied;

            _damageDealer.Initialize(_settings.damage);

            _enabledEnemies.Add(this);
        }

        private void OnDisable()
        {
            _health.Died -= OnDied;

            _enabledEnemies.Remove(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            var damageDealer = other.GetComponent<DamageDealer>();

            if (damageDealer != null)
            {
                var player = other.GetComponent<Player>();

                if (player != null)
                {
                    _health.TakeDamage(damageDealer.Damage);
                }
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

        protected abstract void Release();

        private void OnDied()
        {
            ActivateDeathVFX();
            Release();
        }

        private void ActivateDeathVFX()
        {
            // TODO
            //var explosion = Pool.Instance.Get<ExplosionVFX>();
            //explosion.transform.position = transform.position;
        }
    }
}
