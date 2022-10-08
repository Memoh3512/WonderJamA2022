using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingBar : MonoBehaviour
{
    SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    IEnumerator Blink()
    {
        float step = 0;
        while(sr.color.a > 0)
        {
            yield return new WaitForSeconds(0.01f);

            
        }
    }
}
