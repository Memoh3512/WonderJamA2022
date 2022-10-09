using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    float distanceBetweenArrows;
    public Bow() : base(10, 0, 0, 9, Vector2.zero, Resources.Load<Sprite>("WeaponSprites/arc"), Resources.Load<GameObject>("ProjectilePrefabs/ArcBullet"))
    {
        distanceBetweenArrows = 0.5f;
    }

    override public void Shoot(Vector2 position, Vector2 shootDirection)
    {
        Vector2 normal = Vector2.Perpendicular(shootDirection);
        for(int i = -1; i < 2; i++)
        {           
            base.Shoot(position + normal * distanceBetweenArrows * i, shootDirection );
            lastProjectile.GetComponent<GameProjectile>().Init(this, shootDirection);
        }

        //SFX bow;
    }
}
