using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/GameConfig")]
public class GameConfigSO : ScriptableObject
{
    [SerializeField] private int _totalLevels = 10;
    [SerializeField] private float _gameBGSpeed = 5f;

    public int GetTotalLevels() => _totalLevels;
}
