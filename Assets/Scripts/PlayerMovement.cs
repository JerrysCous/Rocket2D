using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed; // Roopies movement speed
    [HideInInspector]
    public float lastHorizontalVector; // for tracking the last horizontal vector for the animator
    [HideInInspector]
    public float lastVerticalVector; // for tracking the last horizontal vector for the animator
    Rigidbody2D rb;
    [HideInInspector]
    public Vector2 moveDir;
    [HideInInspector]
    public Vector2 lastMovedVector;

    public bool canMove = true; // Controls whether the player can move

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastMovedVector = new Vector2(1, 0f); // Knife won't move at game start if the player hasn't moved
    }

    void Update()
    {
        if (canMove) // Only process input if the player can move
        {
            InputManagement();
        }
        else
        {
            moveDir = Vector2.zero; // Stop movement when movement is disabled
        }
    }

    public float GetCurrentSpeed() {
        return moveSpeed;
    }

    void FixedUpdate()
    {
        if (canMove) // Only move if the player can move
        {
            Move(); // Specifically in FixedUpdate to ensure consistency across machines
        }
    }

    void InputManagement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Legacy movement
        moveDir = new Vector2(moveX, moveY).normalized;

        // Tracks last direction for animator purposes
        if (moveDir.x != 0)
        {
            lastHorizontalVector = moveDir.x;
            lastMovedVector = new Vector2(lastHorizontalVector, 0f);
        }
        if (moveDir.y != 0)
        {
            lastVerticalVector = moveDir.y;
            lastMovedVector = new Vector2(0f, lastVerticalVector);
        }

        if (moveDir.x != 0 && moveDir.y != 0)
        {
            lastMovedVector = new Vector2(lastHorizontalVector, lastVerticalVector);
        }
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);
    }
}
