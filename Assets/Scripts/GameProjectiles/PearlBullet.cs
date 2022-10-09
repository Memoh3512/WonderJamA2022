using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PearlBullet : GameProjectile
{
    bool collided = false;
    PlayerControls player;
    public override void Init(Weapon parent, Vector2 shootDirection)
    {
        this.player = GameManager.instance.GetActivePlayer();
        base.Init(parent, shootDirection);
    }

    public override void Colliding(Collision2D collision)
    {
        if(collision.gameObject.name == "DeathPlane")
        {
            OnHit();
        }
        else if (!collided)
        {
            collided = true;
            GameManager.instance.PopupText(transform.position, 3, Color.green, "FWOOMP!");
            player.transform.position = gameObject.transform.position+Vector3.up;
            OnHit();
        }
    }
}
