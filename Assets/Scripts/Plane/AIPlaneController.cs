using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlaneView))]
public class AIPlaneController : PlaneController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        StartCoroutine("MoveInYDirection");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void Initialize()
    {
        base.Initialize();

        transform.Rotate(transform.forward, 180f);
    }

    private IEnumerator MoveInYDirection()
    {
        while (gameObject != null && IsAlive)
        {
            transform.position -= Time.deltaTime * new Vector3(0, PlaneConfig.GetSpeed().y, 0);
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    protected override bool ShouldTakeDamageFromBullet(Bullet bullet)
    {
        return bullet && bullet.IsFiredFromPlayerPlane();
    }
}
