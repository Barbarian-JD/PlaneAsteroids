using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public static class Tags
    {
        public static readonly string BULLET_TAG = "BULLET";
    }

    public GameConfigSO GameConfig;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this);

        if(GameState.Instance != null)
        {
            GameState.Instance.Load();
        }
    }
}
