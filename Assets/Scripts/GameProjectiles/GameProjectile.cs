using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProjectile : MonoBehaviour
{
    int damage;
    float gravityScale;
    float speed;
    Weapon parent;

    public void Init(int damage, float gravityScale, float speed, Weapon parent)
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
