using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;


public class WeaponSwapUI : MonoBehaviour
{

    SpriteRenderer sr;
    private UnityEvent ammoUsedEvent;
    Weapon currentWeapon;


    public void OnWeaponChange(Weapon weapon)
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();

        currentWeapon = weapon;
        OnAmmoUsed();
        sr.sprite = weapon.weaponSprite;
        transform.Find("WeaponName").gameObject.GetComponent<TextMeshPro>().text = weapon.getWeaponName();
        this.ammoUsedEvent?.RemoveListener(OnAmmoUsed);
        this.ammoUsedEvent = weapon.ammoUsedEvent;
        this.ammoUsedEvent.AddListener(OnAmmoUsed);

    }



    private void OnAmmoUsed()
    {
        transform.Find("AmmoCount").gameObject.GetComponent<TextMeshPro>().text = "x" + currentWeapon.getProjectileCount();
    }

}
