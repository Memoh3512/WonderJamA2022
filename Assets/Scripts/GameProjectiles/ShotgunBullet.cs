using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : GameProjectile
{
    public void Init(Weapon parent, Vector2 shootDirection)
    {      
        base.Init(2, 0.01f, 5, parent, shootDirection);
    }
}
