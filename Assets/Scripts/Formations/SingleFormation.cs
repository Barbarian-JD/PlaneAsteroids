using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A set of loner units. Working individually.
/// </summary>
public class SingleFormation : Formation
{
    protected override void SetupUnits(List<AIPlaneController> units, Vector2 basePosition)
    {
        base.SetupUnits(units, basePosition);

        for (int i=0; i<units.Count; i++)
        {
            Transform currTransform = units[i].transform;
            currTransform.position = basePosition;
        }
    }

    //protected override void SetFormationType()
    //{
    //    _formationType = FormationType.LINEAR;
    //}

    
}
