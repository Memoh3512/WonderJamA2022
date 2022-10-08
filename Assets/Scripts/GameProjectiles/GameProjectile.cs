using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProjectile 
{
    int damage;
    float gravityScale;
    float speed;
    Weapon parent;

    public GameProjectile(int damage, float gravityScale, float speed, Weapon parent)
    {
        this.damage = damage;
        this.gravityScale = gravityScale;
        this.speed = speed;
        this.parent = parent;
    }

    private void OnHit()
    {

    }
}
