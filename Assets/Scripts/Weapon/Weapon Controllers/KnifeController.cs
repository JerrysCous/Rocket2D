using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KnifeController : WeaponController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedKnife = Instantiate(prefab); // instantiate knife
        spawnedKnife.transform.position = transform.position; // position is same as object, parented to player
        Vector2 targetDirection = GetTargetDirection();
        spawnedKnife.GetComponent<KnifeBehaviour>().DirectionChecker(targetDirection);
    }
}
