using DataLayer;
using UnityEngine;

namespace Gameplay
{
    internal sealed class Player : MonoBehaviour
    {
        [SerializeField]
        private Health _health;
        [SerializeField]
        private DamageDealer _damageDealer;

        private GameplaySettingsDB.PlayerSettings _settings;

        private void Awake()
        {
            _settings = GameplaySettings.Player;

            _health.Died += OnDied;
            _health.TookDamage += OnTookDamage;

            SessionsManager.Instance.SessionStarted += OnSessionStarted;
        }

        private void OnDestroy()
        {
            _health.Died -= OnDied;
            _health.TookDamage -= OnTookDamage;

            if (SessionsManager.Instance != null)
            {
                SessionsManager.Instance.SessionStarted -= OnSessionStarted;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var damageDealer = other.GetComponent<DamageDealer>();

            if (damageDealer != null)
            {
                _health.TakeDamage(damageDealer.Damage);
            }
        }

        private void OnTookDamage(int health)
        {
            Data.Session.PlayerHealth.Set(health);
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
