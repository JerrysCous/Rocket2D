using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
    [SerializeField] private float maxHealth = 100f;  // Maximum health
    [SerializeField] private Slider healthBar;        // Reference to the health bar slider

    private float currentHealth;  // Player's current health

    private void Start()
    {
        currentHealth = maxHealth;  // Initialize health at max
        UpdateHealthBar();          // Ensure the health bar reflects full health
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;  // Reduce health
        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            currentHealth = 0;   // Prevent health from going below zero
            Die();
        }

        UpdateHealthBar();  // Update the health bar after taking damage
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);  // Heal but clamp to maxHealth
        Debug.Log($"Player healed by {amount}. Current health: {currentHealth}");
        UpdateHealthBar();  // Update the health bar after healing
    }

    private void Die()
    {
        Debug.Log("Player has died");
        // Add your death logic here (e.g., respawn, game over, etc.)
        gameObject.SetActive(false);  // Example: Disable the player object
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;  // Normalize health for the slider
        }
    }
}