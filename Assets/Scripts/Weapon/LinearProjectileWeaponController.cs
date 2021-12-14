using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearProjectileWeaponController : WeaponController
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void Fire()
    {
        if (CanFire())
        {
            if (Weapon && Weapon.BulletPrefab)
            {
                int numBulletsToFire = Weapon.GetNumBulletToFire();

                float bulletSeparation = 0.2f;
                bool isOddNumOfBullets = numBulletsToFire % 2 == 1;

                for (int i = 1; i <= numBulletsToFire; i++)
                {
                    GameObject bulletObject = Instantiate(Weapon.BulletPrefab);
                    if (bulletObject)
                    {
                        float xDeltaIndex;
                        if(isOddNumOfBullets)
                        {
                            xDeltaIndex = i - (numBulletsToFire / 2 + 1);
                        }
                        else
                        {
                            xDeltaIndex = i - (numBulletsToFire / 2) - 0.5f;
                        }

                        bulletObject.transform.position
                            = _planeController.GetComponent<PlaneView>().WeaponTriggerPoint.position
                                + new Vector3(bulletSeparation * xDeltaIndex, 0, 0);

                        Rigidbody rb = bulletObject.transform.GetComponent<Rigidbody>();
                        rb.velocity = GetShootingDirection() * Weapon.GetBulletSpeed();

                        bulletObject.GetComponent<Bullet>().OwnerWeaponController = this;
                        bulletObject.tag = GameManager.Tags.BULLET_TAG;
                    }
                }
            }
        }
    }

}
