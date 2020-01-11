namespace Gameplay
{
    internal sealed class CapsularEnemy : EnemyBase
    {
        protected override void OnDied()
        {
            base.OnDied();

            Pool.Instance.Release<CapsularEnemy>(gameObject);
        }
    }
}
