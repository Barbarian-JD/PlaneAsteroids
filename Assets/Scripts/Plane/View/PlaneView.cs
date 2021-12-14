using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneView : MonoBehaviour
{
    public TMPro.TMP_Text HealthText;

    // Assuming one trigger point for all weapons for simplicity.
    public Transform WeaponTriggerPoint;

    // List of all the weapons attached
    private List<WeaponController> weapons = new List<WeaponController>();

    private void Start()
    {
        FlipHPTextIfNeeded();
    }

    void OnEnable()
    {
        GetComponent<PlaneController>().PlaneDamaged += OnPlaneDamaged;
    }

    void OnDisable()
    {
        GetComponent<PlaneController>().PlaneDamaged += OnPlaneDamaged;
    }

    private void FlipHPTextIfNeeded()
    {
        if (GetComponent<PlaneController>() is AIPlaneController)
        {
            HealthText.transform.Rotate(transform.forward, 180);
        }
    }

    public void AttachWeapon(WeaponController weaponView)
    {
        weapons.Add(weaponView);
    }

    private void OnPlaneDamaged(object sender, int damageTaken)
    {
        if(HealthText)
        {
            HealthText.text = string.Format("HP: {0}", GetComponent<PlaneController>().CurrHealth);
        }
    }
}
