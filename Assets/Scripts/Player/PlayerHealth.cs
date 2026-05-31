using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    private PlayerHealthSystem _healthSystem;

    public event Action<float> OnHealthChanged;
    public event Action OnDied;

    public float CurrentHealth => _healthSystem.CurrentHealth;
    public float MaxHealth => _healthSystem.MaxHealth;

    void Awake()
    {
        _healthSystem = new PlayerHealthSystem(maxHealth);
    }

    public void TakeDamage(float amount)
    {
        bool wasAlive = !_healthSystem.IsDead();
        _healthSystem.TakeDamage(amount);
        OnHealthChanged?.Invoke(_healthSystem.CurrentHealth);

        if (wasAlive && _healthSystem.IsDead())
            OnDied?.Invoke();
    }

    public void Heal(float amount)
    {
        _healthSystem.Heal(amount);
        OnHealthChanged?.Invoke(_healthSystem.CurrentHealth);
    }

    public bool IsDead() => _healthSystem.IsDead();
    public float GetHealthPercentage() => _healthSystem.GetHealthPercentage();
    public void RestoreFullHealth() => _healthSystem.RestoreFullHealth();
}
