using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shotgun : Weapon
{
    public Shotgun() : base(20, 0, 5, 5, Vector2.zero, Resources.Load<Sprite>("WeaponSprites/Shotgun"), Resources.Load<GameObject>("ProjectilePrefabs/ShotgunBullet"),"",false)
    {

    }

    override public void Shoot(Vector2 position, Vector2 shootDirection)
    {
        
          for(int i = 0; i < 15; i++)
          {
            shootDirection = Quaternion.AngleAxis(Random.Range(-40, 40), Vector2.up) * shootDirection;
            lastProjectile = GameObject.Instantiate(projectilePrefab, position + shootDirection.normalized * Random.Range(0.1f,0.5f)*offset, Quaternion.identity);
            lastProjectile.transform.position += new Vector3(shootingOffset.x, shootingOffset.y);
            lastProjectile.GetComponent<GameProjectile>().Init(this, shootDirection);
          }


        GameManager.instance.GetActivePlayer().GetComponent<Rigidbody2D>().velocity += ((-shootDirection).normalized * knockback);
        GameObject text = GameObject.Instantiate(Resources.Load<GameObject>("PopupText"), lastProjectile.transform.position, Quaternion.identity);
        text.GetComponent<TextMeshPro>().text = "POW!";
        text.transform.localScale *= 2;
        AmmoUsed();
        gunShotEvent.Invoke(this);
        
        
        SoundPlayer.instance.PlaySFX(Resources.Load<AudioClip>("Sound/SFX/Shotgun_V01"));

    }

}
