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
    void Update()
    {
        
    }

    private void OnPlayerInputReceived(object sender, Vector2 deltaInputPos)
    {
        MovePlaneByVector(deltaInputPos);
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
}
