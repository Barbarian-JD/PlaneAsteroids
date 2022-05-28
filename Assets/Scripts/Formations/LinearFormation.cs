using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearFormation : Formation
{
    private float _unitsSeparationX = 1f;


    protected override void SetupUnits(List<AIPlaneController> units, Vector2 basePosition)
    {
        base.SetupUnits(units, basePosition);

        int numUnits = units.Count;

        bool isOddNumOfUnits = numUnits % 2 == 1;

        for (int i=0; i<units.Count; i++)
        {
            Transform currTransform = units[i].transform;
            Vector2 pos = new Vector2(basePosition.x, basePosition.y);
            float xDeltaIndex;

            if (isOddNumOfUnits)
            {
                xDeltaIndex = i - (numUnits / 2 + 1);
            }
            else
            {
                xDeltaIndex = i - (numUnits / 2) - 0.5f;
            }

            pos.x = basePosition.x + _unitsSeparationX * xDeltaIndex;

            currTransform.transform.position = pos;
        }
    }

    //protected override void SetFormationType()
    //{
    //    _formationType = FormationType.LINEAR;
    //}

    
}
