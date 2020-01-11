using UnityEngine;

namespace Gameplay
{
    internal sealed class DamageDealer : MonoBehaviour
    {
        public int Damage { get; private set; }

        public void Initialize(int damage)
        {
            Damage = damage;
        }
    }
}
