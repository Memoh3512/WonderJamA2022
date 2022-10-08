using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Weapon
{
    public Sprite weaponSprite;
    GameObject projectilePrefab;
    float staminaCost;
    float fireRate;
    float knockback;
    Vector2 shootingOffset;
    int projectileCount;
    string weaponName;
    
    public UnityEvent<Weapon> gunShotEvent = new UnityEvent<Weapon>();

    protected GameObject lastProjectile;
    public UnityEvent ammoUsedEvent = new UnityEvent();

    public Weapon(float staminaCost, float fireRate, float knockback, int projectileCount,Vector2 shootingOffset, Sprite weaponSprite, GameObject projectilePrefab, string weaponName = "")
    {
        this.staminaCost = staminaCost;
        this.fireRate = fireRate;
        this.knockback = knockback;
        this.projectileCount = projectileCount;
        this.shootingOffset = shootingOffset;
        this.weaponSprite = weaponSprite;
        this.projectilePrefab = projectilePrefab;
        if (weaponName == "")
            this.weaponName = this.GetType().ToString();
        else this.weaponName = weaponName;
    }

    virtual public void Shoot(Vector2 position, Vector2 shootDirection)
    {
        lastProjectile = GameObject.Instantiate(projectilePrefab, position, Quaternion.identity);
        lastProjectile.transform.position += new Vector3(shootingOffset.x,shootingOffset.y);
        AmmoUsed();
        GameObject text = GameObject.Instantiate(Resources.Load<GameObject>("PopupText"),lastProjectile.transform.position,Quaternion.identity);
        text.GetComponent<TextMeshPro>().text = "PEW!";
        gunShotEvent.Invoke(this);
    }

    public float getStaminaCost()
    {
        return staminaCost;
    }
    
    public void AmmoUsed()
    {
        projectileCount--;
        ammoUsedEvent?.Invoke();
    }
    virtual public void WeaponEmpty()
    {

    }

    public string getWeaponName()
    {
        return weaponName;
    }

    public int getProjectileCount()
    {
        return projectileCount;
    }

}
