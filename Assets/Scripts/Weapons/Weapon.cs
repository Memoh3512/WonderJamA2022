using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    float staminaCost;
    float fireRate;
    float knockback;
    int projectileCount;
    GameObject projectilePrefap;


    public Weapon(float staminaCost, float fireRate, float knockback, int projectileCount)
    {
        this.staminaCost = staminaCost;
        this.fireRate = fireRate;
        this.knockback = knockback;
        this.projectileCount = projectileCount;
    }

    public void Shoot()
    {
        //pew pew
    }


    public void WeaponEmpty()
    {

    }
}
