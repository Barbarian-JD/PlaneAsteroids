using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour
{
    [SerializeField] protected PowerupSO _powerupConfig;

    protected PlaneController _ownerPlaneController = null;
    protected bool isUsedAlready = false;

    public long ExpiryTime { get; set; }

    private void Start()
    {
        StartCoroutine("MoveInYDirection");

        Initialize();
    }

    public void Initialize()
    {
        
    }

    public PowerupType GetPowerupType() => _powerupConfig != null ? _powerupConfig.GetPowerupType() : PowerupType.NONE;
    public long GetPowerupDuration() => _powerupConfig != null ? _powerupConfig.GetPowerupDuration() : 0;
    public float GetPowerupMultiplier() => _powerupConfig != null ? _powerupConfig.GetPowerupMultiplier() : 1;

    public bool CanUsePowerup()
    {
        return !isUsedAlready;
    }

    public virtual void ApplyPowerup(PlaneController planeController)
    {
        _ownerPlaneController = planeController;
        ExpiryTime = TimeManager.Now + _powerupConfig.GetPowerupDuration();
        if (_ownerPlaneController.ApplyPowerup(this))
        {
            UsePowerup();
        }
        if (gameObject)
        {
            Destroy(gameObject);
        }
    }

    public virtual void UsePowerup()
    {
        isUsedAlready = true;
    }

    public virtual void OnExpiry()
    {
        if (this && gameObject)
        {
            Destroy(gameObject);
        }
    }

    public bool IsExpired()
    {
        return (TimeManager.Now > ExpiryTime);
    }

    private IEnumerator MoveInYDirection()
    {
        while (gameObject != null && !isUsedAlready)
        {
            transform.position -= Time.deltaTime * new Vector3(0, _powerupConfig.GetPowerupMovementSpeed(), 0);
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
}
