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
        CheckForLifeSpan();
    }

    public bool IsFiredFromPlayerPlane()
    {
        return OwnerWeaponController ? OwnerWeaponController.IsWeaponOnPlayerController() : false;
    }

    private void CheckForLifeSpan()
    {
        //Vector3 viewPortPos = Camera.main.WorldToViewportPoint(transform.position);
        if (!GameManager.CheckIfInsideTheCameraView(transform.position))
        {
            DestroyBullet();
        }
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
