using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;
    public UIHealthBar uiBar;

    void Start()
    {
        currentHealth = maxHealth;
        if (uiBar != null) uiBar.SetMax(maxHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        if (uiBar != null) uiBar.Set(currentHealth);
    }

    // Heal the player by amount (clamped to maxHealth)
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        if (uiBar != null) uiBar.Set(currentHealth);
    }

    void Die()
    {
        Debug.Log("Player muerto");
        // implementar respawn o game over
    }
}
