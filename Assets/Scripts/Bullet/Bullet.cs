using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Bullet : MonoBehaviour
{
    public WeaponController OwnerWeaponController { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsFiredFromPlayerPlane()
    {
        return OwnerWeaponController ? OwnerWeaponController.IsWeaponOnPlayerController() : false;
    }
}
