using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    public PlaneSO PlaneConfig;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public void MovePlaneByVector(Vector2 deltaPosition)
    {
        Vector3 viewPortPos = Camera.main.WorldToViewportPoint(transform.position + (Vector3)deltaPosition);
        if (viewPortPos.x > 0 && viewPortPos.x < 1
            && viewPortPos.y > 0 && viewPortPos.y < 1)
        {
            transform.position += (Vector3)deltaPosition;
        }
    }
}
