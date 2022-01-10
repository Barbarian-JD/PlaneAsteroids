using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/GameConfig")]
public class GameConfigSO : ScriptableObject
{
    [SerializeField] private float _gameBGSpeed = 0.2f;

    public float GetGameBGSpeed() => _gameBGSpeed;
}
