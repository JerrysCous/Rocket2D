using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour {
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

    public float GetCurrentHealth() {
        return currentHealth;
    }

    public void SetHealth(float health) {
        currentHealth = health;
        UpdateHealthBar();
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log($"Player healed by {amount}. Current health: {currentHealth}");
        UpdateHealthBar();
    }

    private void Die()
    {
        Debug.Log("Player has died");
        gameObject.SetActive(false);
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;  // Normalize health for the slider
        }
    }
}