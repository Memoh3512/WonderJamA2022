using System;
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
    float targetValue;
    float currentValue;
    private float maxStat;
    private float cost;
    private void Awake()
    {
        
    }

    private void Start()
    {
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
                    pc.damageTakenEvent.AddListener(OnValueUpdated);
                    break;
                case Stat.Stamina:
                    currentValue = getMaxPlayerStat();
                    pc.staminaTakenEvent.AddListener(OnValueUpdated);
                    break;
            }
        }
        if(costType==CostType.Mask)
        {
            pc.staminaTakenEvent.AddListener(OnValueUpdated);
            pc.changeGunEvent.AddListener(OnGunChanged);
            pc.unShowCostEvent.AddListener(UnShowCost);
            if (pc.GetGun()!=null)
            {
                UpdateWeaponStaminaCost(pc.GetGun());
            }
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

    void OnValueUpdated(float value)
    {
        if (costType == CostType.Mask)
        {
            UpdateCost(value);
        }
        else
        {
            if (value < 0)
            {
                value = 0;
            }
        
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
            cost = value;
            transform.localScale = new Vector3( baseScale*((currentValueX-(value)) / maxStat),transform.localScale.y,transform.localScale.z);
        }
    }
    void UpdateCost(float currentValueX)
    {
        if (cost > 0)
        {
            transform.localScale = new Vector3( baseScale*((currentValueX-(cost)) / maxStat),transform.localScale.y,transform.localScale.z);
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
        UnShowCost();
        if (pc.CanShootWeaponStamina())
        {
            ShowCost(wep.getStaminaCost(),pc.currentStamina);
        }
    }
    public void UnShowCost()
    {
        if (costType == CostType.Mask)
        {
            transform.localScale = new Vector3( baseScale*maxStat,transform.localScale.y,transform.localScale.z);
        }
    }
}


