using UnityEngine;

namespace Gameplay
{
    internal sealed class Weapon
    {
        private const int SecondsInMinute = 60;

        private float _nextShotTime;
        private float _cooldownSec;
        private int _damage;

        public bool IsReady()
        {
            return Time.time >= _nextShotTime;
        }

        public void Initialize(int shotsPerMinute, int damage)
        {
            _cooldownSec = (float)SecondsInMinute / shotsPerMinute;
            _damage = damage;
        }

        public void Shot(EnemyBase target)
        {
            target.TakeDamage(_damage);

            _nextShotTime = Time.time + _cooldownSec;
        }
    }
}
