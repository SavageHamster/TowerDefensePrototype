using System;
using UnityEngine;

internal sealed class Health : MonoBehaviour
{
    public event Action Died;
    public event Action<int> TookDamage;

    private int _health;

    public void Initialize(int health)
    {
        _health = health;
    }

    public void TakeDamage(int damage)
    {
        _health = Mathf.Max(0, _health - damage);

        TookDamage?.Invoke(_health);

        if (_health <= 0)
        {
            Died?.Invoke();
        }
    }
}