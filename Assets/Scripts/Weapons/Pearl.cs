using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pearl : Weapon
{
    public Pearl() : base(20, 0, 0, 3, Vector2.zero, Resources.Load<Sprite>("WeaponSprites/Pearl"), Resources.Load<GameObject>("ProjectilePrefabs/PearlBullet"))
    {

    }

    // ReSharper disable Unity.PerformanceAnalysis
    override public void Shoot(Vector2 position, Vector2 shootDirection)
    {
        base.Shoot(position, shootDirection);
        lastProjectile.GetComponent<PearlBullet>().Init(this, shootDirection,GameManager.instance.GetActivePlayer());
    }
}
