using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlaneView))]
public class AIPlaneController : PlaneController
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("MoveInYDirection");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    private IEnumerator MoveInYDirection()
    {
        while (gameObject != null && GetComponent<PlaneView>().IsAlive)
        {
            transform.position -= Time.deltaTime * new Vector3(0, PlaneConfig.GetSpeed().y, 0);
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
}
