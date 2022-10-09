using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damagable : MonoBehaviour
{
    protected int hp;

    public UnityEvent<float> damageTakenEvent = new UnityEvent<float>();
    public Damagable(int hp = 10)
    {
        this.hp = hp;
    }

    public void Init(int hp)
    {
        this.hp = hp;
    }

    /// <summary>
    /// TAKEDAMAGE
    /// </summary>
    /// <param name="damage"></param>
    /// <returns>true if dead</returns>
    public bool TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            OnDeath();
        } else if (gameObject.CompareTag("Player"))
        {
            
            
            SoundPlayer.instance.PlaySFX(Resources.Load<AudioClip>("Sound/SFX/Hurt_V01"));
            
        }
        damageTakenEvent?.Invoke(hp);
        return hp <= 0;
    }

 
    protected virtual void OnDeath()
    {
        Destroy(gameObject);
    }


}
