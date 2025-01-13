using UnityEngine;

public interface IPowerUp {
    string Name { get; }
    string Description { get; }
    void ApplyEffect(GameObject player);
}
