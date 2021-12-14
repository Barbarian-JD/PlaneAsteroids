using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxFormation : Formation
{
    private float _unitsSeparation = 1f;

    public override void SetupUnits(List<Transform> units, Vector2 basePosition)
    {
        int numUnits = units.Count;

        bool isOddNumOfUnits = numUnits % 2 == 1;

        int boxSize = (int)Mathf.Sqrt(units.Count);

        for (int i=0; i<numUnits; i++)
        {
            Transform currTransform = units[i];
            Vector2 pos = new Vector2(basePosition.x, basePosition.y);
            float xDeltaIndex;
            float yDeltaIndex;

            xDeltaIndex = i % boxSize;
            yDeltaIndex = i / boxSize + 1;

            pos.x = basePosition.x + _unitsSeparation * xDeltaIndex;
            pos.y = basePosition.y + _unitsSeparation * yDeltaIndex;

            currTransform.transform.position = pos;
        }
    }

    //protected override void SetFormationType()
    //{
    //    _formationType = FormationType.LINEAR;
    //}

    
}
