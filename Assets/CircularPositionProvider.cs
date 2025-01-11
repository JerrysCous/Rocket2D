using UnityEngine;
using System;
using System.Collections.Generic;


public class CircularPositionProvider : IPositionProvider {
    private readonly Transform center;
    private float radius;
    private Vector2[] currentPositions;

    public CircularPositionProvider(Transform center, float radius) {
        this.center = center ?? throw new ArgumentNullException(nameof(center));
        this.radius = Mathf.Max(0.1f, radius);
    }

    public void UpdateRadius(float newRadius) {
        radius = Mathf.Max(0.1f, newRadius);
    }

    public Vector2 GetPosition(int index, int totalPositions) {
        float angleInRadians = (index * 2f * Mathf.PI) / totalPositions;
        float xOffset = Mathf.Cos(angleInRadians) * radius;
        float yOffset = Mathf.Sin(angleInRadians) * radius;
        Vector2 centerPos = center.position;
        Vector2 position = centerPos + new Vector2(xOffset, yOffset);

        if (currentPositions == null || currentPositions.Length != totalPositions) {
            currentPositions = new Vector2[totalPositions];
        }
        currentPositions[index] = position;

        return position;
    }

    public void DrawGizmos() {
        if (center == null || currentPositions == null) return;

        UnityEngine.Gizmos.color = Color.yellow;
        UnityEngine.Gizmos.DrawWireSphere(center.position, radius);

        UnityEngine.Gizmos.color = Color.red;
        foreach (var position in currentPositions) {
            UnityEngine.Gizmos.DrawWireSphere(position, 0.2f);
        }
    }
}

[Serializable]
public class EnemyFormationConfig {
    [SerializeField] private int maxEnemies = 8;
    [SerializeField] private float radius = 2f;
    [SerializeField] private float maxDistanceForReassignment = 5f;
    [SerializeField] private float reassignmentDelay = 2f;
    public int MaxEnemies {
        get => maxEnemies;
        set => maxEnemies = Mathf.Max(1, value);
    }

    public float Radius {
        get => radius;
        set => radius = Mathf.Max(0.1f, value);
    }

    public float MaxDistanceForReassignment {
        get => maxDistanceForReassignment;
        set => maxDistanceForReassignment = Mathf.Max(0.1f, value);
    }

    public float ReassignmentDelay {
        get => reassignmentDelay;
        set => reassignmentDelay = Mathf.Max(0f, value);
    }

    public void Validate() {
        MaxEnemies = maxEnemies;
        Radius = radius;
        MaxDistanceForReassignment = maxDistanceForReassignment;
        ReassignmentDelay = reassignmentDelay;
    }
}