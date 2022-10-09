using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponPopup : MonoBehaviour
{

    public float timeAlive;
    public void Setup(Weapon weapon)
    {
        transform.Find("WeaponSprite").GetComponent<SpriteRenderer>().sprite = weapon.weaponSprite;
        transform.Find("WeaponName").GetComponent<TextMeshPro>().text = weapon.getWeaponName() + " Acquired!";
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(timeAlive);
        Destroy(gameObject);
    }
}
