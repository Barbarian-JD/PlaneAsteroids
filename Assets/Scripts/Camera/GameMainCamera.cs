using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainCamera : SingletonMonoBehaviour<GameMainCamera>
{
    public Camera MainCamera;

    protected override void Awake()
    {
        base.Awake();

        if(MainCamera == null)
        {
            MainCamera = GetComponent<Camera>();
        }
    }
}
