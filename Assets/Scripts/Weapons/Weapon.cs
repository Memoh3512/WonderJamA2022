using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    Sprite weaponSprite;
    GameObject projectilePrefab;
    float staminaCost;
    float fireRate;
    float knockback;
    Vector2 shootingOffset;
    int projectileCount;


    public Weapon(float staminaCost, float fireRate, float knockback, int projectileCount,Vector2 shootingOffset, Sprite weaponSprite, GameObject projectilePrefab)
    {
        this.staminaCost = staminaCost;
        this.fireRate = fireRate;
        this.knockback = knockback;
        this.projectileCount = projectileCount;
        this.shootingOffset = shootingOffset;
        this.weaponSprite = weaponSprite;
        this.projectilePrefab = projectilePrefab;
    }

    virtual public void Shoot(Vector2 shootDirection)
    {
        GameObject projectile = GameObject.Instantiate(projectilePrefab);
        projectile.transform.position += new Vector3(shootingOffset.x,shootingOffset.y);
        projectile.GetComponent<SniperBullet>().Init(this, shootDirection);
    }


    virtual public void WeaponEmpty()
    {

    }
}
