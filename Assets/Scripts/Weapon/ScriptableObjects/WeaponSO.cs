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
    [SerializeField] private float _baseAttack = 5;
    [SerializeField] private float _cooldownTime = 1;
    [SerializeField] private float _bulletSpeed = 5;
    [SerializeField] private int _numBullets = 1; // Number of bullets fired simultaneously
    [SerializeField] private WeaponType _weaponType = WeaponType.SINGLE_GUN;

    public GameObject BulletPrefab;

    public float GetBaseAttack() => _baseAttack;
    public float GetCooldownTime() => _cooldownTime;
    public float GetBulletSpeed() => _bulletSpeed;
    public int GetNumBulletToFire() => _numBullets;
}
