namespace Gameplay
{
    internal sealed class SphericalEnemy : EnemyBase
    {
        protected override void Release()
        {
            Pool.Instance.Release<SphericalEnemy>(gameObject);
        }
    }
}
