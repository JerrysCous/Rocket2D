
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    // References
    Animator am;
    PlayerMovement pm;

    void Start() {
        am = GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();
    }

    void Update() {
        if (pm.moveDir != Vector2.zero) // Player is moving
        {
            am.SetBool("Move", true);
            UpdateBlendTree(pm.moveDir);
        }
        else // Player is idle
        {
            am.SetBool("Move", false);
            SetIdleDirection();
        }
    }

    void UpdateBlendTree(Vector2 moveDirection) {
        // Update the blend tree parameters
        am.SetFloat("MoveX", moveDirection.x);
        am.SetFloat("MoveY", moveDirection.y);
    }

    void SetIdleDirection() {
        // Use the last moved vector for the idle animation
        am.SetFloat("MoveX", pm.lastMovedVector.x);
        am.SetFloat("MoveY", pm.lastMovedVector.y);
    }
}
