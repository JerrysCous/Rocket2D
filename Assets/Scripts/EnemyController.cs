using System;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    [SerializeField] private EnemyMovementConfig movementConfig;
    [SerializeField] private float attackRange = 1f;  // Range at which the enemy can attack
    [SerializeField] private float cooldownTime = 1f;  // Time between attacks (in seconds)
    [SerializeField] private float minAttackDamage = 5f;  // Minimum attack damage
    [SerializeField] private float maxAttackDamage = 10f;  // Maximum attack damage
    public int AssignedPositionIndex { get; private set; }
    private Vector2 targetPosition;
    private float moveSpeed;
    private EnemyManager manager;
    private PlayerHealthBar playerHealth;
    private float lastAttackTime;  // Time of the last attack


    private void OnEnable() {
        // Notify LevelManager when this enemy spawns
        LevelManager.Instance.RegisterEnemy(this.transform);
    }

    private void OnDisable() {
        // Notify LevelManager when this enemy is destroyed
        if (LevelManager.Instance != null)
            LevelManager.Instance.UnregisterEnemy(this.transform);
    }

    private void Start() {
        moveSpeed = UnityEngine.Random.Range(
            movementConfig.MinMoveSpeed,
            movementConfig.MaxMoveSpeed
        );

        manager = FindObjectOfType<EnemyManager>();
        if (manager == null) {
            Debug.LogError("EnemyManager not found");
            return;
        }

        playerHealth = FindObjectOfType<PlayerHealthBar>();
        if (playerHealth == null) {
            Debug.LogError("PlayerHealth not found");
            return;
        }

        manager.RegisterEnemy(this);
    }

    private void Update() {
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget() {
        if (Vector2.Distance(transform.position, targetPosition) > 0.1f) {
            Vector2 direction = ((Vector2)targetPosition - (Vector2)transform.position).normalized;
            transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
        }
        else {
            TryAttackPlayer();  // Attempt to attack when close enough
        }
    }

    private void TryAttackPlayer() {
        if (Vector2.Distance(transform.position, playerHealth.transform.position) <= attackRange) {
            // Attack only if enough time has passed since the last attack
            if (Time.time - lastAttackTime >= cooldownTime) {
                AttackPlayer();
            }
        }
    }

    private void AttackPlayer() {
        // Random damage between min and max attack damage
        float damage = UnityEngine.Random.Range(minAttackDamage, maxAttackDamage);
        Debug.Log($"Enemy attacked the player for {damage} damage!");
        playerHealth.TakeDamage(damage);  // Apply damage to the player
        lastAttackTime = Time.time;  // Update the last attack time
    }

    public void AssignPosition(int index, Vector2 position) {
        AssignedPositionIndex = index;
        targetPosition = position;
    }

    private void OnDestroy() {
        if (manager != null) {
            manager.UnregisterEnemy(this);
        }
    }

    private void OnDrawGizmos() {
        if (UnityEditor.Selection.activeGameObject == gameObject) {
            UnityEngine.Gizmos.color = Color.green;
            UnityEngine.Gizmos.DrawLine(transform.position, targetPosition);
        }
    }

    [Serializable]
    public class EnemyMovementConfig {
        public float MinMoveSpeed = 2f;
        public float MaxMoveSpeed = 5f;

        public void Validate() {
            MinMoveSpeed = Mathf.Max(0.1f, MinMoveSpeed);
            MaxMoveSpeed = Mathf.Max(MinMoveSpeed, MaxMoveSpeed);
        }
    }
}
