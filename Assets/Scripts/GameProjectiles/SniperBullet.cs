using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : GameProjectile
{
  

    public void Init(Weapon parent, Vector2 shootDirection)
    {
        base.Init(20,0.1f,20,parent,shootDirection);
    }
}
