using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poutine : Weapon
{
    public Poutine() : base(23, 0, 0, 1, Vector2.zero, Resources.Load<Sprite>("WeaponSprites/Pearl"), null)
    {

    }

    override public void Shoot(Vector2 position, Vector2 shootDirection)
    {
        GameManager.instance.PopupText(GameManager.instance.GetActivePlayer().transform.position,2,new Color(0.65f,0.16f,0.16f), "MIAM!");
        GameManager.instance.GetActivePlayer().Heal(50);
        AmmoUsed();
        gunShotEvent.Invoke(this);
        
        //TODO SFX Poutine
        
    }
}
