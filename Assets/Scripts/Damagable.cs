using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    protected int hp;
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
        }
        
        return hp <= 0;
    }

    protected virtual void OnDeath()
    {
        Destroy(gameObject);
    }


}
