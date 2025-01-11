using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;

    private void Start() {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0) {
            Die();
        }
    }

    private void Die() {
        Debug.Log("Player has died");
    }

    public void Heal(float amount) {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log($"Player healed by {amount}. Current health: {currentHealth}");
    }
}
