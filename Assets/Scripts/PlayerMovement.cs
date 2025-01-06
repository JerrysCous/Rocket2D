using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public float moveSpeed; // Roopies movement speed
    [HideInInspector]
    public float lastHorizontalVector;// for tracking the last horizontal vector for the animator
    [HideInInspector]
    public float lastVerticalVector;// for tracking the last horizontal vector for the animator
    Rigidbody2D rb;
    [HideInInspector]
    public Vector2 moveDir;

    void Update()
    {
        InputManagement();
    }

    void FixedUpdate()
    {
        Move(); //specifically in fixed update to make sure its not tied to fps and is consistent on all machines
    }

    void InputManagement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        //legacy movement is easier to implement and functions fine in this instance
        moveDir = new Vector2(moveX, moveY).normalized;
        //tracks which way roopie is moving before only moving on the Y axis. keeps roopie facing a direction when moving up and down
        if (moveDir.x != 0)
        {
            lastHorizontalVector = moveDir.x;
        }
        if (moveDir.y != 0)
        {
            lastVerticalVector = moveDir.y;
        }
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);
    }

    
}