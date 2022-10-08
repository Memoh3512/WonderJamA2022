using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericBar : MonoBehaviour
{
    public enum Stat
    {
        Health,
        Stamina
    }
    public enum CostType
    {
        Mask,
        None
    }
    
    public float delay;
    public float speed;
    public Stat statType;
    public CostType costType = CostType.None;
    PlayerControls pc;
    SpriteRenderer sr;
    float baseScale;
    private float baseXPos;
    float targetValue;
    float currentValue;
    private float maxStat;
    
    private void Start()
    {
        baseXPos = transform.localPosition.x;
        baseScale = transform.localScale.x;
        sr = gameObject.GetComponent<SpriteRenderer>();
        pc = transform.parent.gameObject.GetComponent<PlayerControls>();
        maxStat = getMaxPlayerStat();
        
        if (costType==CostType.None)
        {
            switch (statType)
            {
                case Stat.Health:
                    currentValue = getMaxPlayerStat();
                    pc.damageTakenEvent.AddListener(OnValueRemove);
                    break;
                case Stat.Stamina:
                    currentValue = getMaxPlayerStat();
                    pc.staminaTakenEvent.AddListener(OnValueRemove);
                    break;
            }
        }
        else if(costType==CostType.Mask)
        {
            pc.changeGunEvent.AddListener(OnGunChanged);
        }
    }

    IEnumerator valueTaken()
    {
        yield return new WaitForSeconds(delay);
        while(targetValue < currentValue)
        {
            yield return new WaitForSeconds(0.001f);
            currentValue -= 0.001f * speed;
            if (currentValue < targetValue) currentValue = targetValue;
            Vector3 scale = new Vector3(baseScale * (currentValue / maxStat), transform.localScale.y, transform.localScale.z);
            transform.localScale = scale;
        }
    }

    void OnValueRemove(float value)
    {
        if (value >= 0)
        {
            if (delay!=0f)
            {
                targetValue = value;
                StopCoroutine(valueTaken());
                StartCoroutine(valueTaken());
            }
            else
            {
                transform.localScale = new Vector3( baseScale*(value / maxStat),transform.localScale.y,transform.localScale.z);
            }
        }
    }

    void ShowCost(float value,float currentValueX)
    {
        if (costType == CostType.Mask)
        {
            Debug.Log("ShowCost");
            transform.localScale = new Vector3( baseScale*((currentValueX-(value)) / maxStat),transform.localScale.y,transform.localScale.z);
        }
    }
    private float getMaxPlayerStat()
    {
        float stat = -1f;
        switch (statType)
        {
            case Stat.Health:
                stat = pc.maxHP;
                break;
            case Stat.Stamina:
                stat = pc.maxStamina;
                break;
        }
        return stat;
    }
    private void OnGunChanged(Weapon wep,Weapon oldWeapon)
    {
        UpdateWeaponStaminaCost(wep);
        if (oldWeapon!=null)
        {
            oldWeapon.gunShotEvent.RemoveListener(OnGunShot);
        }
        wep.gunShotEvent.AddListener(OnGunShot);
    }
    private void OnGunShot(Weapon wep)
    {
        UpdateWeaponStaminaCost(wep);
    }
    private void UpdateWeaponStaminaCost(Weapon wep)
    {
        if (pc.CanShootWeapon())
        {
            ShowCost(wep.getStaminaCost(),pc.currentStamina);
        }
    }
}


