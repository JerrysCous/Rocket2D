
using System;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    [SerializeField] private EnemyMovementConfig movementConfig;
    public int AssignedPositionIndex { get; private set; }
    private Vector2 targetPosition;
    private float moveSpeed;
    private EnemyManager manager;

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
