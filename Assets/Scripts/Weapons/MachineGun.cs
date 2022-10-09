using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : Weapon
{
    public MachineGun() : base(1, 1200, 1, 100, Vector2.zero, Resources.Load<Sprite>("WeaponSprites/PPSH"), Resources.Load<GameObject>("ProjectilePrefabs/PPSHBullet"),"Machine Gun")
    {

    }

    // ReSharper disable Unity.PerformanceAnalysis
    override public void Shoot(Vector2 position, Vector2 shootDirection)
    {
        shootDirection = Quaternion.AngleAxis(Random.Range(-80f, 80f), Vector2.up) * shootDirection;
        base.Shoot(position, shootDirection);
        lastProjectile.GetComponent<GameProjectile>().Init(this, shootDirection);
        SoundPlayer.instance.PlaySFX(Resources.Load<AudioClip>("Sound/SFX/Riffleshot_V01"));

    }
}
