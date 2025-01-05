using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    public float smoothTime = 0.1f; // Time to reach target velocity
    private Vector2 currentVelocity; // Used for smooth damping
    private Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input
        float xinput = Input.GetAxisRaw("Horizontal");
        float yinput = Input.GetAxisRaw("Vertical");

        // Calculate target velocity
        Vector2 targetVelocity = new Vector2(xinput, yinput).normalized * speed;

        // Smoothly transition to target velocity
        Vector2 smoothedVelocity = Vector2.SmoothDamp(
            body.velocity,
            targetVelocity,
            ref currentVelocity,
            smoothTime
        );

        body.velocity = smoothedVelocity;
    }
}