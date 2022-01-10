using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerup : Powerup
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

            _ownerPlaneController.ToggleShield(isActivated:true);
        }
    }

    public override void OnExpiry()
    {
        base.OnExpiry();

        _ownerPlaneController.ToggleShield(isActivated:false);
    }
}
