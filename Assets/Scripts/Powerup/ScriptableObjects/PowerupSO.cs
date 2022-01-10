using UnityEngine;

[System.Serializable]
public enum PowerupType
{
    NONE = 0,
    SHIELD = 1,
    SHOOT_BOOST = 2
}

[CreateAssetMenu(menuName = "ScriptableObjects/New Powerup")]
public class PowerupSO : ScriptableObject
{
    [SerializeField] private PowerupType _powerupType;
    [SerializeField] private PowerupParams _powerupParams;
    [SerializeField] private float _powerupMovementSpeed;

    public PowerupType GetPowerupType() => _powerupType;
    public float GetPowerupMultiplier() => _powerupParams != null ? _powerupParams._multiplier : 1f;
    public long GetPowerupDuration() => _powerupParams != null ? _powerupParams._duration : 1;
    public float GetPowerupMovementSpeed() => _powerupMovementSpeed;
}

[System.Serializable]
public class PowerupParams
{
    public float _multiplier;
    public long _duration;
}