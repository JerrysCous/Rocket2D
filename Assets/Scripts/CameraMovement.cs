using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;  // Reference to the target (Oroopie)
    public Vector3 offset;    // Offset from the target (adjust to position behind)

    public float smoothSpeed = 0.125f;  // Speed of camera smoothing

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;  // Calculate desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);  // Smooth the transition
        transform.position = smoothedPosition;  // Set the camera position
    }
}
