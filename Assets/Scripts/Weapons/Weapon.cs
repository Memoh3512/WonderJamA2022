using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon
{
    public Sprite weaponSprite;
    GameObject projectilePrefab;
    float staminaCost;
    float fireRate;
    float knockback;
    Vector2 shootingOffset;
    int projectileCount;

    protected GameObject lastProjectile;

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

    virtual public void Shoot(Vector2 position, Vector2 shootDirection)
    {
        lastProjectile = GameObject.Instantiate(projectilePrefab, position, Quaternion.identity);
        lastProjectile.transform.position += new Vector3(shootingOffset.x,shootingOffset.y);
        projectileCount--;
        GameObject text = GameObject.Instantiate(Resources.Load<GameObject>("PopupText"),lastProjectile.transform.position,Quaternion.identity);
        text.GetComponent<TextMeshPro>().text = "PEW";
    }


    virtual public void WeaponEmpty()
    {

    }


}
