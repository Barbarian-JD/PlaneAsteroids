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

    private List<Powerup> _activePowerups = new List<Powerup>();

    public bool IsShieldActive { get; private set; }

    public float PlaneAttackMultiplier { get; private set; } = 1f;
    public float PlaneWeaponCooldownMultiplier { get; private set; } = 1f;

    [SerializeField] protected Collider _collider;

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

        PlaneDestroyed += OnPlaneDestroyed;

        StartCoroutine("CheckForPowerupsExpiry");
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

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Bullet>() != null
            && ShouldTakeDamageFromBullet(other.gameObject.GetComponent<Bullet>()))
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            if (bullet.OwnerWeaponController
                && bullet.OwnerWeaponController.Weapon)
            {
                int damage = (int)(bullet.OwnerWeaponController.Weapon.GetBaseAttack()
                                    * PlaneAttackMultiplier);

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

    /// <summary>
    /// If the powerup already exists, increases the time duration, else applies a fresh powerup.
    ///  returns true if its applying a fresh powerup
    /// </summary>
    /// <param name="powerup">Powerup.</param>
    public bool ApplyPowerup(Powerup powerup)
    {
        if(powerup.GetPowerupType() == PowerupType.NONE)
        {
            Debug.LogErrorFormat("PowerupType can't be NONE, Powerup: {0}", powerup);
            return false;
        }

        foreach (Powerup currPowerup in _activePowerups)
        {
            if (currPowerup.GetPowerupType() == powerup.GetPowerupType())
            {
                currPowerup.ExpiryTime += powerup.GetPowerupDuration();
                return false;
            }
        }
        _activePowerups.Add(powerup);
        return true;
    }

    private IEnumerator CheckForPowerupsExpiry()
    {
        while (true)
        {
            if (_activePowerups != null && _activePowerups.Count > 0)
            {
                for (int i = _activePowerups.Count - 1; i >= 0; i--)
                {
                    Powerup powerup = _activePowerups[i];
                    if (powerup.IsExpired())
                    {
                        RemovePowerupAt(i);
                        powerup.OnExpiry();
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void RemovePowerup(Powerup powerup)
    {
        _activePowerups.Remove(powerup);
    }

    public void RemovePowerupAt(int powerupIndex)
    {
        _activePowerups.RemoveAt(powerupIndex);
    }

    public void ToggleShield(bool isActivated)
    {
        IsShieldActive = isActivated;
    }

    public void SetPlaneAttackMultiplier(float multiplier) => PlaneAttackMultiplier = multiplier;

    public void SetPlaneWeaponCooldownMultiplier(float multiplier) => PlaneWeaponCooldownMultiplier = multiplier;

    private void OnPlaneDestroyed(object sender, EventArgs args)
    {
        PlaneDestroyed -= OnPlaneDestroyed;

        TrySpawnPowerup();
    }

    private void TrySpawnPowerup()
    {
        List<PowerupType> powerupKeys = PlaneConfig.PowerupWeights.GetDictionaryKeys();
        List<int> powerupWeights = PlaneConfig.PowerupWeights.GetDictionaryValues();

        int pickedIndexPowerup = powerupWeights.PickWeightedElementIndex();

        if (pickedIndexPowerup >= 0 && pickedIndexPowerup < powerupKeys.Count)
        {
            PowerupType pickedPowerupType = powerupKeys[pickedIndexPowerup];

            if(pickedPowerupType != PowerupType.NONE)
            {
                LevelGenerator.Instance.SpawnPowerup(pickedPowerupType, transform.position);
            }
        }
    }
}
