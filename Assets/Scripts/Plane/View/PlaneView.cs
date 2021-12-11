using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneView : MonoBehaviour
{
    public bool IsAlive { get; private set; } = true;

    // Start is called before the first frame update
    void Start()
    {
        IsAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForLifeSpan();
    }

    private void CheckForLifeSpan()
    {
        Vector3 viewPortPos = Camera.main.WorldToViewportPoint(transform.position);
        if (IsAlive && viewPortPos.y < -0.1f)
        {
            IsAlive = false;

            StopAllCoroutines();

            Destroy(gameObject);
        }
    }
}
