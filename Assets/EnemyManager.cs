
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    [SerializeField] private EnemyFormationConfig config;
    [SerializeField] private Transform player;

    private float lastRadius;
    private IPositionProvider positionProvider;
    private Dictionary<int, EnemyController> positionAssignments;
    private HashSet<EnemyController> activeEnemies;
    private float lastReassignTime;

    private void OnValidate() {
        if (config != null) {
            config.Validate();
            if (positionProvider != null && !Mathf.Approximately(lastRadius, config.Radius)) {
                UpdateRadius();
            }
        }
    }

    private void UpdateRadius() {
        lastRadius = config.Radius;
        positionProvider.UpdateRadius(config.Radius);
        foreach (var enemy in activeEnemies) {
            if (enemy != null) {
                AssignNewPosition(enemy);
            }
        }
    }

    private void OnDrawGizmos() {
        if (positionProvider != null) {
            positionProvider.DrawGizmos();
        }
        else if (!Application.isPlaying && player != null) {
            UnityEngine.Gizmos.color = Color.yellow;
            UnityEngine.Gizmos.DrawWireSphere(player.position, config.Radius);
        }
    }

    private void Awake() {
        config.Validate();
        InitializeManager();
    }

    private void InitializeManager() {
        if (player == null) {
            Debug.LogError("Player transform not assigned");
            return;
        }

        lastRadius = config.Radius;
        positionProvider = new CircularPositionProvider(player, config.Radius);
        positionAssignments = new Dictionary<int, EnemyController>();
        activeEnemies = new HashSet<EnemyController>();

        for (int i = 0; i < config.MaxEnemies; i++) {
            positionProvider.GetPosition(i, config.MaxEnemies);
        }
    }

    private void Update() {
        if (!Mathf.Approximately(lastRadius, config.Radius)) {
            UpdateRadius();
        }

        if (Time.time - lastReassignTime < config.ReassignmentDelay) return;
        CheckAndReassignPositions();
        lastReassignTime = Time.time;
    }

    private void CheckAndReassignPositions() {
        foreach (var enemy in activeEnemies) {
            if (enemy == null) continue;

            if (IsEnemyTooFarFromPosition(enemy)) {
                AssignNewPosition(enemy);
            }
        }
    }

    private bool IsEnemyTooFarFromPosition(EnemyController enemy) {
        var currentPosition = enemy.transform.position;
        var assignedPosition = GetPositionByIndex(enemy.AssignedPositionIndex);
        return Vector2.Distance(currentPosition, assignedPosition) > config.MaxDistanceForReassignment;
    }

    public void RegisterEnemy(EnemyController enemy) {
        if (enemy == null) return;

        activeEnemies.Add(enemy);
        AssignNewPosition(enemy);
    }

    public void UnregisterEnemy(EnemyController enemy) {
        if (enemy == null) return;

        activeEnemies.Remove(enemy);
        if (positionAssignments.ContainsValue(enemy)) {
            var index = GetAssignmentIndex(enemy);
            if (index.HasValue) {
                positionAssignments.Remove(index.Value);
            }
        }
    }

    private int? GetAssignmentIndex(EnemyController enemy) {
        foreach (var kvp in positionAssignments) {
            if (kvp.Value == enemy)
                return kvp.Key;
        }
        return null;
    }

    private void AssignNewPosition(EnemyController enemy) {
        var currentIndex = GetAssignmentIndex(enemy);
        if (currentIndex.HasValue) {
            positionAssignments.Remove(currentIndex.Value);
        }

        int newIndex = FindClosestAvailablePositionIndex(enemy.transform.position);
        positionAssignments[newIndex] = enemy;
        enemy.AssignPosition(newIndex, GetPositionByIndex(newIndex));
    }

    private int FindClosestAvailablePositionIndex(Vector2 fromPosition) {
        float closestDistance = float.MaxValue;
        int closestIndex = 0;

        for (int i = 0; i < config.MaxEnemies; i++) {
            if (positionAssignments.ContainsKey(i)) continue;

            Vector2 position = GetPositionByIndex(i);
            float distance = Vector2.Distance(fromPosition, position);

            if (distance < closestDistance) {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    private Vector2 GetPositionByIndex(int index) {
        return positionProvider.GetPosition(index, config.MaxEnemies);
    }
}