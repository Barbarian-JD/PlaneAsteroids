using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The enum types can be replace with appropriate aeroplane types.
// The dev has no knowledge about aeroplane types.
public enum PlaneType
{
    PLANE_TYPE_1 = 0, // Normal plane - Medium HP, low attack, medium speed
    PLANE_TYPE_2 = 1, // Fast plane - Low HP, high attack, high speed
    PLANE_TYPE_3 = 2, // Bigger plane - High HP, medium attack, low speed
    PLANE_TYPE_4 = 3, // Boss plane - High HP, high attack, low speed

}

[CreateAssetMenu(menuName = "ScriptableObjects/New Plane")]
public class PlaneSO : ScriptableObject
{
    [SerializeField] private int _maxHP = 100;
    [SerializeField] private int _baseAttack = 5;
    [SerializeField] private Vector2 _speed = new Vector2(5, 5);
    [SerializeField] private PlaneType _planeType = PlaneType.PLANE_TYPE_1;

    [Space(10)]
    [SerializeField][TextArea(1, 3)] private string _description; // Currently for config readability

    [Space(10)]
    [SerializeField] private List<WeaponSO> _weapons;

    public Vector2 GetSpeed() => _speed;
}
