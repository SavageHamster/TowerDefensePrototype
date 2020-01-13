namespace VFX
{
    internal sealed class TakeDamageVFX : VFXBase
    {
        protected override void Release()
        {
            Pool.Instance.Release<TakeDamageVFX>(gameObject);
        }
    }
}
