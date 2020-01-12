namespace Gameplay
{
    internal sealed class CapsularEnemy : EnemyBase
    {
        protected override void Release()
        {
            Pool.Instance.Release<CapsularEnemy>(gameObject);
        }
    }
}
