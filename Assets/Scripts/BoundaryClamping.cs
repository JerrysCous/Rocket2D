using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net.Mime;
using System.Security.Cryptography;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class BoundaryClamping : MonoBehaviour

{
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Main Camera reference
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            UnityEngine.Debug.LogError("No main camera found. Please ensure a camera is tagged as MainCamera.");
            return;
        }

        // Calculate Screen Bounds in World Space
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        // Get the Collider2D from the child object (the one with the Collider component)
        Collider2D collider = GetComponentInChildren<Collider2D>();
        if (collider == null)
        {
            UnityEngine.Debug.LogError("No Collider2D found in the GameObject or its children.");
            return;
        }

        // Calculate object size based on the Collider2D's bounds
        objectWidth = collider.bounds.extents.x; // Half width
        objectHeight = collider.bounds.extents.y; // Half height

        Debug.Log("screenBounds: " + screenBounds);
        Debug.Log("objectWidth: " + objectWidth + ", objectHeight: " + objectHeight);

    }

    // Update is called once per frame
    void Update()
    {
        if (screenBounds == Vector2.zero) return;

        // Get the current position of the GameObject
        Vector3 position = transform.position;

        // Clamp the position within the screen bounds
        position.x = Mathf.Clamp(position.x, -screenBounds.x + objectWidth, screenBounds.x - objectWidth);
        position.y = Mathf.Clamp(position.y, (-screenBounds.y + 1) + objectHeight, (screenBounds.y - 1) - objectHeight);

        

        // Apply the clamped position to the GameObject
        transform.position = position;
    }
}
