using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/LevelConfig")]
public class LevelConfigSO : ScriptableObject
{
    public int TotalEnemiesToBeSpawned = 30;

    [Space(10)]
    public List<PlaneType> EnemyPlaneTypes;
    public List<float> PlaneProbabilityWeights;

    [Space(5)]
    public PlaneType BossPlaneType;

    [Space(10)]
    public List<FormationType> FormationTypes;
    public List<float> FormationProbabilityWeights;

    [Space(10)]
    public float InterSpawnDelayInSec = 2;


    public PlaneType PickWeightedRandomPlaneType()
    {
        int randomWeightedIndex = PlaneProbabilityWeights.PickWeightedElementIndex();

        return EnemyPlaneTypes[randomWeightedIndex];
    }

    public FormationType PickWeightedRandomFormationType()
    {
        int randomWeightedIndex = FormationProbabilityWeights.PickWeightedElementIndex();

        return FormationTypes[randomWeightedIndex];
    }
}
