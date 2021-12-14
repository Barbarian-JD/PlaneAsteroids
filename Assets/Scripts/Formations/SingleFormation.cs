using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A set of loner units. Working individually.
/// </summary>
public class SingleFormation : Formation
{
    public override void SetupUnits(List<Transform> units, Vector2 basePosition)
    {
        for (int i=0; i<units.Count; i++)
        {
            Transform currTransform = units[i];
            currTransform.position = basePosition;
        }
    }

    //protected override void SetFormationType()
    //{
    //    _formationType = FormationType.LINEAR;
    //}

    
}
