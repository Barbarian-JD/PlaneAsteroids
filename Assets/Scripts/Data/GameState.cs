using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : Singleton<GameState>
{
    private PlayerData _playerData = new PlayerData();

    public PlayerData GetPlayerData() => _playerData;

    public void Save()
    {
        _playerData.Save();
    }

    public void Load()
    {
        _playerData.Load();
    }
}
