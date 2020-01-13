using DataLayer;
using UnityEngine;

namespace Gameplay
{
    internal sealed class Player : MonoBehaviour
    {
        [SerializeField]
        private Health _health;
        [SerializeField]
        private CollisionDamageDealer _damageDealer;

        private GameplaySettingsDB.PlayerSettings _settings;

        private void Awake()
        {
            _settings = GameplaySettings.Player;

            _health.Died += OnDied;

            SessionsManager.Instance.SessionStarted += OnSessionStarted;
        }

        private void OnDestroy()
        {
            _health.Died -= OnDied;

            if (SessionsManager.Instance != null)
            {
                SessionsManager.Instance.SessionStarted -= OnSessionStarted;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var damageDealer = other.GetComponent<CollisionDamageDealer>();

            if (damageDealer != null)
            {
                _health.TakeDamage(damageDealer.Damage);
                Data.Session.PlayerHealth.Set(_health.Current);
            }
        }

        private void OnDied()
        {
            SessionsManager.Instance.FinishSession();
        }

        private void OnSessionStarted()
        {
            _health.Initialize(_settings.Health);
            _damageDealer.Initialize(int.MaxValue);
        }
    }
}
