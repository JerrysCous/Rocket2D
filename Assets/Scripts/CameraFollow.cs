using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed = 2f;   // Smoothness of camera movement
    public float yOffset = 1f;       // Vertical offset to position the camera above the character
    public float zOffset = -10f;     // Camera's fixed Z position for 2D
    public Transform target;         // Reference to the player object (Oroopie)

    void LateUpdate()
    {
        if (target == null) return;

        // Set target position based on Oroopie’s position
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y + yOffset, zOffset);

        // Smoothly move the camera to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}