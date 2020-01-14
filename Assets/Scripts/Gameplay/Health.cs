using System;
using UnityEngine;

namespace Gameplay
{
    internal sealed class Health : MonoBehaviour
    {
        public event Action Died;

        public int Current { get; private set; }

        public void Initialize(int health)
        {
            Current = health;
        }

        public void TakeDamage(int damage)
        {
            Current = Mathf.Max(0, Current - damage);

            if (Current <= 0)
            {
                Died?.Invoke();
            }
        }
    }
}