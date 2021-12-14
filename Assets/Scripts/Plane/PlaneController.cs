using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlaneView))]
public abstract class PlaneController : MonoBehaviour
{
    public PlaneSO PlaneConfig;

    public bool IsAlive { get; private set; } = true;

    protected int _currHealth;
    public virtual int CurrHealth
    {
        get { return _currHealth; }
        protected set
        {
            _currHealth = value;
            HealthChanged?.Invoke(this, _currHealth);

            IsAlive = _currHealth > 0;
            if(!IsAlive)
            {
                DestroyPlane();
            }
        }
    }

    public EventHandler<int> HealthChanged;

    public EventHandler PlaneDestroyed;

    public EventHandler<int> PlaneDamaged;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CheckForLifeSpan();
    }

    public virtual void Initialize()
    {
        CurrHealth = PlaneConfig.GetMaxHP();
        IsAlive = true;

        List<GameObject> weaponPrefabs = PlaneConfig.GetWeaponPrefabs();
        if (weaponPrefabs == null || weaponPrefabs.Count == 0)
        {
            Debug.LogErrorFormat("No weapon attached to the plane, {0}", gameObject.ToString());
            return;
        }

        // Attach weapons on the plane
        for (int i=0; i<weaponPrefabs.Count; i++)
        {
            GameObject prefab = weaponPrefabs[i];

            GameObject weapon = Instantiate(prefab);
            if (weapon && weapon.GetComponent<WeaponController>() != null)
            {
                // Attach weapon to the plane.
                GetComponent<PlaneView>().AttachWeapon(weapon.GetComponent<WeaponController>());

                weapon.GetComponent<WeaponController>().Initialize(this, PlaneConfig.GetWeaponConfigs()[i]);
                weapon.transform.SetParent(transform);
                weapon.GetComponent<WeaponController>().StartFiring();
            }
        }
    }

    private void CheckForLifeSpan()
    {
        //Vector3 viewPortPos = Camera.main.WorldToViewportPoint(transform.position);
        if (IsAlive && !GameManager.CheckIfInsideTheCameraView(transform.position))
        {
            IsAlive = false;

            StopAllCoroutines();

            DestroyPlane();
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

    protected abstract bool ShouldTakeDamageFromBullet(Bullet bullet);

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Bullet>() != null
            && ShouldTakeDamageFromBullet(other.gameObject.GetComponent<Bullet>()))
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            if (bullet.OwnerWeaponController
                && bullet.OwnerWeaponController.Weapon)
            {
                int damage = bullet.OwnerWeaponController.Weapon.GetBaseAttack();

                CurrHealth = Math.Max(0, CurrHealth - damage);
                PlaneDamaged?.Invoke(this, damage);

                bullet.DestroyBullet();
            }
        }
    }

    protected void DestroyPlane()
    {
        // Wrong way to do this, need to use Object Pooling instead.
        Destroy(gameObject);

        PlaneDestroyed?.Invoke(this, null);
    }
}
