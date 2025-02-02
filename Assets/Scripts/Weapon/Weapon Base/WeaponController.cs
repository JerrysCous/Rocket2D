using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    float currentCooldown;
    public int pierce;
    [Header("Targeting")]
    [SerializeField] private float detectionRadius ;
   
    protected PlayerMovement pm;
    private Transform nearestEnemy;
    public float DetectionRadius { get => detectionRadius; private set => detectionRadius = value; }
    
    public Transform NearestEnemy { get => nearestEnemy; private set => nearestEnemy = value; }



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
         FindNearestEnemy();
        if (currentCooldown <= 0f)
        {
            Attack(); // attack when cooldown is 0
        }
    }

    protected virtual void Attack()
    {
        currentCooldown = cooldownDuration; // reset cooldown
    }
    private void FindNearestEnemy()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        if (hitColliders.Length > 0)
        {
            nearestEnemy = hitColliders
                .Where(collider => collider.CompareTag("Enemy"))
                .OrderBy(collider => Vector2.Distance(transform.position, collider.transform.position))
                .First()
                .transform;
        }
        else
        {
            nearestEnemy = null;
        }
    }

    protected Vector2 GetTargetDirection()
    {
        if (nearestEnemy != null)
        {
            return (nearestEnemy.position - transform.position).normalized;
        }

        return pm.LastMovedVector;
    }

}
