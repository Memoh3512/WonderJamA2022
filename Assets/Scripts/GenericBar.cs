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
    
    public float delay;
    public float speed;
    public Stat statType;
    public bool costShower;
    PlayerControls pc;
    SpriteRenderer sr;
    float baseScale;
    float targetValue;
    float currentValue;
    private float maxStat;
    
    private void Start()
    {
        baseScale = transform.localScale.x;
        sr = gameObject.GetComponent<SpriteRenderer>();
        pc = transform.parent.gameObject.GetComponent<PlayerControls>();
        maxStat = getMaxPlayerStat();
        
        if (!costShower)
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
        else
        {
            Invis();
        }
    }
    void Invis()
    {
        transform.localScale = new Vector2(0,transform.localScale.y);
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
            Debug.Log("Scale + "+scale);
            transform.localScale = scale;
        }
    }

    void OnValueRemove(float value)
    {
        if (value >= 0)
        {
            Debug.Log(value);
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

    void ShowCost(int value)
    {
        if (costShower)
        {
            transform.localScale = new Vector3( baseScale*((value / maxStat)+((currentValue-value)/maxStat)),transform.localScale.y,transform.localScale.z);
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
}


