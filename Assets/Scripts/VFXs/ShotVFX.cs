namespace VFX
{
    internal sealed class ShotVFX : VFXBase
    {
        protected override void Release()
        {
            Pool.Instance.Release<ShotVFX>(gameObject);
        }
    }
}
