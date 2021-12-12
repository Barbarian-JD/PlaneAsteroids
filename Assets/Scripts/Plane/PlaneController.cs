using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlaneView))]
public class PlaneController : MonoBehaviour
{
    public PlaneSO PlaneConfig;

    public bool IsAlive { get; private set; } = true;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        IsAlive = true;
        Initialize();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CheckForLifeSpan();
    }

    public virtual void Initialize()
    {
        List<GameObject> weaponPrefabs = PlaneConfig.GetWeaponPrefabs();
        if (weaponPrefabs == null || weaponPrefabs.Count == 0)
        {
            Debug.LogErrorFormat("No weapon attached to the plane, {0}", gameObject.ToString());
            return;
        }

        // Attach weapons on the plane
        foreach (GameObject prefab in weaponPrefabs)
        {
            GameObject weapon = Instantiate(prefab);
            if (weapon && weapon.GetComponent<WeaponController>() != null)
            {
                // Attach weapon to the plane.
                GetComponent<PlaneView>().AttachWeapon(weapon.GetComponent<WeaponController>());

                weapon.GetComponent<WeaponController>().Initialize(this);
                weapon.transform.SetParent(transform);
                weapon.GetComponent<WeaponController>().StartFiring();
            }
        }
    }

    private void CheckForLifeSpan()
    {
        Vector3 viewPortPos = Camera.main.WorldToViewportPoint(transform.position);
        if (IsAlive && viewPortPos.y < -0.1f)
        {
            IsAlive = false;

            StopAllCoroutines();

            Destroy(gameObject);
        }
    }

    public void MovePlaneByVector(Vector2 deltaPosition)
    {
        Vector3 viewPortPos = Camera.main.WorldToViewportPoint(transform.position + (Vector3)deltaPosition);
        if (viewPortPos.x > 0 && viewPortPos.x < 1
            && viewPortPos.y > 0 && viewPortPos.y < 1)
        {
            transform.position += (Vector3)deltaPosition;
        }
    }

    /// <summary>
    /// Can contain conditions when the plane itself isn't able to fire. Eg. the plane is stunned.
    /// </summary>
    /// <returns></returns>
    protected bool CanFire()
    {
        return true;
    }

    /// <summary>
    /// Checks and fires the weapons.
    /// </summary>
    protected void Fire()
    {
        if(CanFire())
        {

        }
    }
}
