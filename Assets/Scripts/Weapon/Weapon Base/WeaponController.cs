using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Weapon controller Inheritance Script
/// </summary>
public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public GameObject prefab;
    public float damage;
    public float speed;
    public float cooldownDuration;
    public float currentCooldown;
    public int pierce;

    protected PlayerMovement pm;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        currentCooldown = cooldownDuration; // weapon wont fire when game starts / is collected
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime; 
        if (currentCooldown <= 0f)
        {
            Attack(); // attack when cooldown is 0
        }
    }

    protected virtual void Attack()
    {
        currentCooldown = cooldownDuration; // reset cooldown
    }

    public float GetCurrentCooldown() {
        return cooldownDuration;
    }

    public float GetCurrentSpeed() {
        return speed;
    }
}
