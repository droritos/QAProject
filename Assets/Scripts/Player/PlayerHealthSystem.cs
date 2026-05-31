using System;

public class PlayerHealthSystem
{
    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }

    public PlayerHealthSystem(float maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }

    // Function 1 - deal damage, clamp to 0
    public void TakeDamage(float amount)
    {
        if (amount < 0) return;
        CurrentHealth = Math.Max(0, CurrentHealth - amount);
    }

    // Function 2 - heal, clamp to max
    public void Heal(float amount)
    {
        if (amount < 0) return;
        CurrentHealth = Math.Min(MaxHealth, CurrentHealth + amount);
    }

    // Function 3 - is the player dead?
    public bool IsDead()
    {
        return CurrentHealth <= 0f;
    }

    // Function 4 - health as 0.0 to 1.0
    public float GetHealthPercentage()
    {
        if (MaxHealth <= 0) return 0f;
        return CurrentHealth / MaxHealth;
    }

    // Function 5 - fill to max
    public void RestoreFullHealth()
    {
        CurrentHealth = MaxHealth;
    }

    // Function 6 - change max health, clamp current if needed
    public void SetMaxHealth(float newMax)
    {
        if (newMax <= 0) return;
        MaxHealth = newMax;
        CurrentHealth = Math.Min(CurrentHealth, MaxHealth);
    }

    // Function 7 - is player alive and above a health threshold?
    public bool IsHealthy(float threshold = 0.5f)
    {
        return GetHealthPercentage() >= threshold;
    }
}
