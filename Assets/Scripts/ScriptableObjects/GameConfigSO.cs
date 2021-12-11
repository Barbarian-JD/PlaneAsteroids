using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/GameConfig")]
public class GameConfigSO : ScriptableObject
{
    [SerializeField] private int _totalLevels = 10;

    public int GetTotalLevels() => _totalLevels;
}
