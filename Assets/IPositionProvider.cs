using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPositionProvider {
    Vector2 GetPosition(int index, int totalPositions);
    void DrawGizmos();
    void UpdateRadius(float newRadius);
}