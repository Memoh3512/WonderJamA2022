using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shotgun : Weapon
{
    public Shotgun() : base(20, 0, 5, 5, Vector2.zero, Resources.Load<Sprite>("WeaponSprites/Shotgun"), Resources.Load<GameObject>("ProjectilePrefabs/ShotgunBullet"),"",0.5f,false)
    {

    }

    override public void Shoot(Vector2 position, Vector2 shootDirection)
    {
        
          for(int i = 0; i < 15; i++)
          {
            shootDirection = Quaternion.AngleAxis(Random.Range(-60, 60), Vector2.up) * shootDirection;
            lastProjectile = GameObject.Instantiate(projectilePrefab, position + shootDirection.normalized * Random.Range(0.1f,0.5f), Quaternion.identity);
            lastProjectile.transform.position += new Vector3(shootingOffset.x, shootingOffset.y);
            lastProjectile.GetComponent<ShotgunBullet>().Init(this, shootDirection);
          }


        GameManager.instance.GetActivePlayer().GetComponent<Rigidbody2D>().velocity += ((-shootDirection).normalized * knockback);
        GameObject text = GameObject.Instantiate(Resources.Load<GameObject>("PopupText"), lastProjectile.transform.position, Quaternion.identity);
        text.GetComponent<TextMeshPro>().text = "POW!";
        text.transform.localScale *= 2;
        AmmoUsed();
        gunShotEvent.Invoke(this);
        
        //TODO SFX SHOTGUN
        

    }

}
