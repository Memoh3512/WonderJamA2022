using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapon
{
    public Sniper() : base(50,0,10,2,Vector2.zero,null,null)
    {
        
    }

    override public void Shoot(Vector2 shootDirection)
    {
        base.Shoot(shootDirection);
    }
}
