using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapon
{
    public Sniper() : base(50,0,10,2,Vector2.zero,Resources.Load<Sprite>("WeaponSprites/Sniper"), Resources.Load<GameObject>("ProjectilePrefabs/SniperBullet"))
    {
        
    }

    override public void Shoot(Vector2 position, Vector2 shootDirection)
    {
        base.Shoot(position, shootDirection);
        lastProjectile.GetComponent<SniperBullet>().Init(this, shootDirection);

    }


}
