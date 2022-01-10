using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PowerupManager : SingletonMonoBehaviour<GameManager>
{
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
            LevelGenerator.Instance.ScoreChanged += OnScoreChanged;
        }
        else if(scene.name == "MenuScene")
        {
            if (LevelGenerator.Instance)
            {
                LevelGenerator.Instance.LevelWin -= OnLevelWin;
                LevelGenerator.Instance.ScoreChanged -= OnScoreChanged;
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

    private void OnScoreChanged(object sender, int updatedScore)
    {
        int highScore = GameState.Instance.GetPlayerData().HighScore;

        if(updatedScore > highScore)
        {
            GameState.Instance.GetPlayerData().OnHighScoreChanged(updatedScore);
        }
    }

    /// <summary>
    /// Returns if the provided world coordinates position lies inside the (camera-view + 10% buffer)
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static bool CheckIfInsideTheCameraView(Vector2 position)
    {
        Vector3 viewPortPos = Camera.main.WorldToViewportPoint(position);
        if (viewPortPos.x > -0.1f && viewPortPos.x < 1.1f
            && viewPortPos.y > -0.1f && viewPortPos.y < 1.1f)
        {
            return true;
        }

        return false;
    }

}
