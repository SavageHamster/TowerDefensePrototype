using UnityEngine;

namespace Gameplay
{
    internal sealed class SphericalEnemy : EnemyBase
    {
        protected override void OnDied()
        {
            base.OnDied();

            Pool.Instance.Release<SphericalEnemy>(gameObject);
        }
    }
}
