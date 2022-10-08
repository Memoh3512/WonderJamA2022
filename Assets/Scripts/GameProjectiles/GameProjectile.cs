using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProjectile : MonoBehaviour
{
    int damage;
    float gravityScale;
    float speed;
    Weapon parent;
    Vector2 shootDirection;
    public void Init(int damage, float gravityScale, float speed, Weapon parent,Vector2 shootDirection)
    {
        this.damage = damage;
        this.gravityScale = gravityScale;
        this.speed = speed;
        this.parent = parent;
        this.shootDirection = shootDirection;

        GetComponent<Rigidbody2D>().gravityScale = gravityScale;
        GetComponent<Rigidbody2D>().velocity = shootDirection.normalized * speed;
    }
    private void OnHit()
    {
    
        

    }
}
