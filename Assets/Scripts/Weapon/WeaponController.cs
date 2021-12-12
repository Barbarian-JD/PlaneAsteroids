using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    public WeaponSO Weapon;
    protected PlaneController _planeController;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize(PlaneController planeController)
    {
        _planeController = planeController;
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

            yield return new WaitForSeconds(Weapon.GetCooldownTime());
        }
    }

    protected bool CanFire()
    {
        return true;
    }

    protected abstract void Fire();

    protected bool IsWeaponOnPlayerController() => _planeController is PlayerPlaneController;

    protected virtual Vector2 GetShootingDirection()
    {
        return IsWeaponOnPlayerController() ? new Vector2(0f, 1f) : new Vector2(0f, -1f);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
