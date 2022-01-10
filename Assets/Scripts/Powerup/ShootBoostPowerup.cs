using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBoostPowerup : Powerup
{
    public override void ApplyPowerup(PlaneController plane)
    {
        base.ApplyPowerup(plane);
    }

    public override void UsePowerup()
    {
        if (!IsExpired())
        {
            base.UsePowerup();
            _ownerPlaneController.SetPlaneAttackMultiplier(_powerupConfig.GetPowerupMultiplier());
            _ownerPlaneController.SetPlaneWeaponCooldownMultiplier(1f/_powerupConfig.GetPowerupMultiplier());
        }
    }

    public override void OnExpiry()
    {
        base.OnExpiry();

        _ownerPlaneController.SetPlaneAttackMultiplier(1f);
        _ownerPlaneController.SetPlaneWeaponCooldownMultiplier(1f);
    }
}
