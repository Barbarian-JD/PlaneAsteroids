using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    SINGLE_GUN = 0,
    MULTI_GUN = 1,
    SINGLE_BOMB = 2,
    MISSILE = 3
}

[CreateAssetMenu(menuName = "ScriptableObjects/New Weapon")]
public class WeaponSO : ScriptableObject
{
    [SerializeField] private int _baseAttack = 5;
    [SerializeField] private float _attackSpeed = 1;
    [SerializeField] private float _numBullets = 1; // Number of bullets fired simultaneously
    [SerializeField] private WeaponType _weaponType = WeaponType.SINGLE_GUN;
}
