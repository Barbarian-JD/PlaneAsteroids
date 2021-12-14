using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FormationType
{
    LINEAR = 0,
    BOX = 1,
    SINGLE = 2 // Loner unit.
}

public abstract class Formation
{
    protected FormationType _formationType;

    //protected abstract void SetFormationType();

    public abstract void SetupUnits(List<Transform> units, Vector2 basePosition);
}
