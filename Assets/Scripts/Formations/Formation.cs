using System;
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

    private List<AIPlaneController> _cohortUnits = null;

    //protected abstract void SetFormationType();
    public virtual void Initialize(List<AIPlaneController> units, Vector2 basePosition)
    {
        SetupUnits(units, basePosition);

        AIPlaneController.PlaneHitScreenEdge += OnPlaneHitScreenEdge;

        if(_cohortUnits != null && _cohortUnits.Count > 0)
        {
            foreach(var currUnit in _cohortUnits)
            {
                currUnit.PlaneDestroyed += OnPlaneDestroyed;
            }
        }
    }

    private void OnPlaneDestroyed(object sender, EventArgs e)
    {
        if(sender is AIPlaneController destroyedPlaneController
            && _cohortUnits != null && _cohortUnits.Count > 0)
        {
            // Remove the listener.
            destroyedPlaneController.PlaneDestroyed -= OnPlaneDestroyed;

            // Remove the destroyed plane from the formation.
            _cohortUnits.Remove(destroyedPlaneController);

            // If there's no plane left in formation, destroy the formation.
            if(_cohortUnits.Count == 0)
            {
                DestroyFormation();
            }
        }
    }

    protected virtual void SetupUnits(List<AIPlaneController> units, Vector2 basePosition)
    {
        _cohortUnits = units;
    }

    protected void OnPlaneHitScreenEdge(object sender, bool isMovingLeftUpdated)
    {
        if(sender is AIPlaneController aiController)
        {
            // Check if the sender plane is actually a part of the current formation.
            if(_cohortUnits != null && _cohortUnits.Count > 0 && _cohortUnits.Contains(aiController))
            {
                foreach(AIPlaneController currentUnit in _cohortUnits)
                {
                    currentUnit.ToggleDirection();
                }
            }
        }
    }

    protected void DestroyFormation()
    {
        AIPlaneController.PlaneHitScreenEdge -= OnPlaneHitScreenEdge;
    }
}
