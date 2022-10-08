using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMG : Weapon
{
    public SMG() : base(5, 600, 3, 30, Vector2.zero, Resources.Load<Sprite>("WeaponSprites/SMG"), Resources.Load<GameObject>("ProjectilePrefabs/SMGBullet"))
    {

    }

    // ReSharper disable Unity.PerformanceAnalysis
    override public void Shoot(Vector2 position, Vector2 shootDirection)
    {
        base.Shoot(position, shootDirection);
        lastProjectile.GetComponent<SMGBullet>().Init(this, shootDirection);
        

    }

}
