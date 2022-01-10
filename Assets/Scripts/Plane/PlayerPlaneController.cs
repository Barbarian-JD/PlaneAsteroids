using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlaneController : PlaneController
{
    public EventHandler<PowerupType> PowerupApplied;

    protected override void Start()
    {
        base.Start();

        if(InputManager.Instance != null)
        {
            InputManager.Instance.PlayerInputHappening += OnPlayerInputReceived;
        }
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.PlayerInputHappening -= OnPlayerInputReceived;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    private void OnPlayerInputReceived(object sender, Vector2 deltaInputPos)
    {
        MovePlaneByVector(deltaInputPos);
    }

    protected override bool ShouldTakeDamageFromBullet(Bullet bullet)
    {
        return !IsShieldActive && bullet && !bullet.IsFiredFromPlayerPlane();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.gameObject.GetComponent<Powerup>() != null)
        {
            Powerup powerup = other.gameObject.GetComponent<Powerup>();
            if (powerup.CanUsePowerup())
            {
                powerup.ApplyPowerup(this);
                PowerupApplied?.Invoke(this, powerup.GetPowerupType());
            }
        }
    }
}
