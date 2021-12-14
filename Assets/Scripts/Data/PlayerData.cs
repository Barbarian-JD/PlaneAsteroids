using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int MaxLevelUnlocked { get; private set; } = 1;
    public int HighScore { get; private set; } = 0;

    public struct SerializationKeys
    {
        public static readonly string MAX_LEVEL_UNLOCKED_KEY = "MAX_LEVEL_UNLOCKED_KEY";
        public static readonly string HIGH_SCORE_KEY = "HIGH_SCORE_KEY";
    }

    public void Save()
    {
        PlayerPrefs.SetInt(SerializationKeys.MAX_LEVEL_UNLOCKED_KEY, MaxLevelUnlocked);
        PlayerPrefs.SetInt(SerializationKeys.HIGH_SCORE_KEY, HighScore);
    }

    public void Load()
    {
        MaxLevelUnlocked = PlayerPrefs.GetInt(SerializationKeys.MAX_LEVEL_UNLOCKED_KEY, 1);
        HighScore = PlayerPrefs.GetInt(SerializationKeys.HIGH_SCORE_KEY, 0);
    }

    public void OnLevelCompleted(int levelCompleted)
    {
        // TODO: Add hack checks, if levelNum-MaxLevelUnlocked > 0
        if(levelCompleted > MaxLevelUnlocked - 1)
        {
            MaxLevelUnlocked = levelCompleted + 1;
            Save();
        }
    }
}
