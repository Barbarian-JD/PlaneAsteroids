using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlaneController : PlaneController
{
    private void Start()
    {
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

    
}
