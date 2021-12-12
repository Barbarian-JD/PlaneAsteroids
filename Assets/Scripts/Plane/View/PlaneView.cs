using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlaneController))]
public class PlaneView : MonoBehaviour
{
    // Assuming one trigger point for all weapons for simplicity.
    public Transform WeaponTriggerPoint;

    private List<WeaponController> weapons = new List<WeaponController>();


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    

    public void AttachWeapon(WeaponController weaponView)
    {
        weapons.Add(weaponView);
    }

    
}
