using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HPBar : MonoBehaviour
{
    public bool delayed;
    public float delay;
    public float speed;
    PlayerControls pc;
    SpriteRenderer sr;
    float baseScale;
    int targetHp;
    float currentHp;
    private void Start()
    {
        baseScale = transform.localScale.x;
        sr = gameObject.GetComponent<SpriteRenderer>();
        pc = transform.parent.gameObject.GetComponent<PlayerControls>();
        currentHp = pc.maxHP;
        pc.damageTakenEvent.AddListener(new UnityAction<int>(OnDamageTaken));
    }

    IEnumerator damageTaken()
    {
        yield return new WaitForSeconds(delay);
        while(targetHp < currentHp)
        {
            yield return new WaitForSeconds(0.001f);
            currentHp -= 0.001f * speed;
            if (currentHp < targetHp) currentHp = targetHp;
            transform.localScale = new Vector3(baseScale * (currentHp / (float)pc.maxHP), transform.localScale.y, transform.localScale.z);
        }
    }

    void OnDamageTaken(int hp)
    {
        if (delayed)
        {
            targetHp = hp;
            StopCoroutine(damageTaken());
            StartCoroutine(damageTaken());
        }
        else
        {
            transform.localScale = new Vector3( baseScale*(hp / (float)pc.maxHP),transform.localScale.y,transform.localScale.z);
        }
    }

}


