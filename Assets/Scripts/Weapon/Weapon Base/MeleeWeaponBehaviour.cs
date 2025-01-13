using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// base script for melee weapon behaviours
/// </summary>


public class MeleeWeaponBehaviour : MonoBehaviour
{

    public float destroyAfterSeconds;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

 
}
