using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int MaxLevelUnlocked { get; private set; } = 1;

    public struct SerializationKeys
    {
        public static readonly string MAX_LEVEL_UNLOCKED_KEY = "MAX_LEVEL_UNLOCKED_KEY";
    }

    public void Save()
    {
        PlayerPrefs.SetInt(SerializationKeys.MAX_LEVEL_UNLOCKED_KEY, MaxLevelUnlocked);
    }

    public void Load()
    {
        MaxLevelUnlocked = PlayerPrefs.GetInt(SerializationKeys.MAX_LEVEL_UNLOCKED_KEY, 1);
    }


}
