using DataLayer;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay
{
    internal class EnemyBase : MonoBehaviour
    {
        [SerializeField]
        private NavMeshAgent _navMeshAgent;
        [SerializeField]
        private Health _health;
        [SerializeField]
        private DamageDealer _damageDealer;

        private EnemyData _settings;

        private void OnEnable()
        {
            _settings = GameplaySettings.Enemies[this.GetType()];

            _health.Initialize(_settings.health);
            _health.Died += OnDied;

            _damageDealer.Initialize(_settings.damage);
        }

        private void OnDisable()
        {
            _health.Died -= OnDied;
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

        public void InitializePositions(Vector3 startingPosition, Vector3 targetPosition)
        {
            _navMeshAgent.Warp(startingPosition);
            _navMeshAgent.destination = targetPosition;
        }

        protected virtual void OnDied()
        {
            ActivateDeathVFX();
        }

        private void ActivateDeathVFX()
        {
            // TODO
            //var explosion = Pool.Instance.Get<ExplosionVFX>();
            //explosion.transform.position = transform.position;
        }
    }
}
