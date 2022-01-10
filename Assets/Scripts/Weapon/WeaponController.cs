using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    [HideInInspector] public WeaponSO Weapon;
    protected PlaneController _planeController;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize(PlaneController planeController, WeaponSO weaponConfig)
    {
        _planeController = planeController;
        Weapon = weaponConfig;
    }

    public void StartFiring()
    {
        StartCoroutine("FiringCoroutine");
    }

    protected IEnumerator FiringCoroutine()
    {
        while(this != null && gameObject != null)
        {
            if(CanFire())
            {
                Fire();
            }

            yield return new WaitForSeconds(GetWeaponCooldownTime());
        }
    }

    protected float GetWeaponCooldownTime()
    {
        return _planeController.PlaneWeaponCooldownMultiplier * Weapon.GetCooldownTime();
    }

    protected bool CanFire()
    {
        return true;
    }

    protected abstract void Fire();

    public bool IsWeaponOnPlayerController() => _planeController is PlayerPlaneController;

    protected virtual Vector2 GetShootingDirection()
    {
        return transform.up;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
