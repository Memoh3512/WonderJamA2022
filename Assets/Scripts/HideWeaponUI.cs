using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HideWeaponUI : MonoBehaviour
{
    WeaponSwapUI wsUI;
    void Start()
    {
        wsUI = transform.Find("WeaponSprite").gameObject.GetComponent<WeaponSwapUI>();
        transform.parent.GetComponent<PlayerControls>().changeGunEvent.AddListener(new UnityAction<Weapon, Weapon>(OnWeaponChange));
    }
    private void OnWeaponChange(Weapon weapon, Weapon previousWeapon)
    {
        SetActiveChildren(true);
        wsUI.OnWeaponChange(weapon);
        StopAllCoroutines();
        StartCoroutine(HideCooldown());

    }
    IEnumerator HideCooldown()
    {
        yield return new WaitForSeconds(4);
        SetActiveChildren(false);
    }


    private void SetActiveChildren(bool active)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(active);
        }
    }
}
