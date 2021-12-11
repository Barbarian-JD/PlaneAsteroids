using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonMonoBehaviour<InputManager>
{
    private Vector2 _touchStartPos;
    private Vector2 _touchDeltaPos;

    public EventHandler<Vector2> PlayerInputHappening;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

        Input.multiTouchEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        DetectInput();
    }

    public void DetectInput()
    {

#if UNITY_EDITOR
        // Start the input process
        if (Input.GetMouseButtonDown(0))
        {
            _touchStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        // Keep gathering the delta touch position
        else if(Input.GetMouseButton(0))
        {
            Vector2 touchNowPos = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition));
            _touchDeltaPos = touchNowPos - _touchStartPos;
            _touchStartPos = touchNowPos;

            if (_touchDeltaPos != Vector2.zero)
            {
                PlayerInputHappening?.Invoke(this, _touchDeltaPos);
            }
        }
        // Stop the touch input process
        else if(Input.GetMouseButtonUp(0))
        {
            _touchStartPos = Vector2.zero;
            _touchDeltaPos = Vector2.zero;
        }
#endif
    }
}
