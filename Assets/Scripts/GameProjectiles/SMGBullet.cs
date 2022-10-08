using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMGBullet : GameProjectile
{
    public void Init(Weapon parent, Vector2 shootDirection)
    {
        base.Init(1, 1f, 5, parent, shootDirection);
    }
}
