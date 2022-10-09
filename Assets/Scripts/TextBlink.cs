using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextBlink : MonoBehaviour
{
    float step;
    TextMeshPro sr;
    void Start()
    {
        sr = GetComponent<TextMeshPro>();
        StartCoroutine(Blink());

    }

    IEnumerator Blink()
    {
        step = 0.001f;
        while (sr.color.a > 0.1f)
        {
            yield return new WaitForSeconds(0.01f);
            step += step * 0.05f;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - step);
        }

        while (sr.color.a < 1)
        {
            yield return new WaitForSeconds(0.01f);
            step += step * 0.05f;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a + step);
        }

        StartCoroutine(Blink());
    }
}
