using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    private PlayerHealth _playerHealth;

    public bool PlayerHasDied { get; private set; } = false;
    public int DeathCount { get; private set; } = 0;

    void Awake()
    {
        _playerHealth = GetComponent<PlayerHealth>();
    }

    void OnEnable()
    {
        _playerHealth.OnDied += HandleDeath;
    }

    void OnDisable()
    {
        _playerHealth.OnDied -= HandleDeath;
    }

    private void HandleDeath()
    {
        PlayerHasDied = true;
        DeathCount++;
        Debug.Log("Player died!");
        // In a real game: trigger animation, disable controls, show game over UI...
    }
}
