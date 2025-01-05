using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Oroopie’s movement speed
    private bool facingRight = true; // Track if Oroopie is facing right

    void Update()
    {
        float moveX = 0f;

        // Get input for movement
        if (Input.GetKey("a")) // Move left (flip left)
        {
            moveX = -moveSpeed * Time.deltaTime;
            if (facingRight)
            {
                Flip(); // Flip to face left
            }
        }
        else if (Input.GetKey("d")) // Move right (flip right)
        {
            moveX = moveSpeed * Time.deltaTime;
            if (!facingRight)
            {
                Flip(); // Flip to face right
            }
        }

        // Move Oroopie based on input
        transform.Translate(new Vector3(moveX, 0, 0));
    }

    // Flip the character horizontally
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1; // Flip the x-axis scale
        transform.localScale = theScale;
    }
}

