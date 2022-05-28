using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlaneView))]
public class AIPlaneController : PlaneController
{
    public bool IsBoss { get; set; } = false;

    public bool IsMovingLeft { get; private set; } = false;

    public static EventHandler<bool> PlaneHitScreenEdge;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        StartCoroutine(DoMovement());
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

    private IEnumerator DoMovement()
    {
        while (gameObject != null && IsAlive)
        {
            // Calculate X-Direction
            int xDirection = IsMovingLeft ? -1 : 1;

            // Y-Movement
            transform.position += Time.deltaTime * new Vector3(xDirection * PlaneConfig.GetSpeed().x
                                                                , -PlaneConfig.GetSpeed().y
                                                                , 0);

            if(CheckIfIntersectingWithHorizontalScreenEdge())
            {
                PlaneHitScreenEdge?.Invoke(this, IsMovingLeft);
            }

            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    protected override bool ShouldTakeDamageFromBullet(Bullet bullet)
    {
        return !IsShieldActive && bullet && bullet.IsFiredFromPlayerPlane();
    }

    private bool CheckIfIntersectingWithHorizontalScreenEdge()
    {
        // returns if the plane intersects with any of the screen edges
        if(GameMainCamera.Instance == null || GameMainCamera.Instance.MainCamera == null)
        {
            Debug.LogError("Something wrong with GameMainCamera. This shouldn't happen.");
            return false;
        }

        float xDelta = 0f;

        if(_collider)
        {
            xDelta += _collider.bounds.extents.x/2;
        }

        float virtualBoundaryXLength = GameMainCamera.Instance.MainCamera.orthographicSize/2 - xDelta;

        return (transform.position.x < -virtualBoundaryXLength
                || transform.position.x > virtualBoundaryXLength);
    }

    public void ToggleDirection()
    {
        IsMovingLeft = !IsMovingLeft;
    }
}
