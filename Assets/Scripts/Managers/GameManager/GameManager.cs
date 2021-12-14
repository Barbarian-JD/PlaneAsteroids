using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public LevelConfigSO[] LevelConfigs;

    public static class Tags
    {
        public static readonly string BULLET_TAG = "BULLET";
    }

    public GameConfigSO GameConfig;

    protected override void Awake()
    {
        if (Instance == null)
        {
            base.Awake();
            DontDestroyOnLoad(base.gameObject);

            if (GameState.Instance != null)
            {
                GameState.Instance.Load();
            }
        }
        else
        {
            Destroy(base.gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.name == "GameScene")
        {
            LevelGenerator.Instance.LevelWin += OnLevelWin;
        }
        else if(scene.name == "MenuScene")
        {
            if (LevelGenerator.Instance)
            {
                LevelGenerator.Instance.LevelWin -= OnLevelWin;
            }
        }
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void OnLevelWin(object sender, int levelCompleted)
    {
        GameState.Instance.GetPlayerData().OnLevelCompleted(levelCompleted);
    }
}
