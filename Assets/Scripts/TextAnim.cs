using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextAnim : MonoBehaviour
{
    // Start is called before the first frame update
    public float lifeSpan = 0;
    float Scaleup;
    float startingSize;
    float speed;
    float timeStep;
    Vector2 spawnDirection;
    RectTransform RT;
    TextMeshPro tm;
    void Start()
    {
        timeStep = 0.01f;
        if(lifeSpan == 0)
        lifeSpan = 1.5f;
        Scaleup = 2;
        speed = 0.5f;
        
        float angle = Random.Range(0, 360)*Mathf.Deg2Rad;
        spawnDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        spawnDirection *= Random.Range(0.5f, 1);
        transform.position += new Vector3(spawnDirection.x,spawnDirection.y);
        RT = GetComponent<RectTransform>();
        tm = GetComponent<TextMeshPro>();

        startingSize = RT.localScale.x;
        StartCoroutine(Animation());
    }

    IEnumerator Animation()
    {
        float totaltime = 0;
        float diffSize = (startingSize * Scaleup) - startingSize;
        float steps = startingSize / (lifeSpan / timeStep);
        while (RT.localScale.x < startingSize * Scaleup)
        {
            yield return new WaitForSeconds(timeStep);
            totaltime += timeStep;
            transform.position += new Vector3(spawnDirection.normalized.x * speed * timeStep, spawnDirection.normalized.y * speed * timeStep);
            tm.alpha = (lifeSpan - totaltime) / lifeSpan;
            RT.localScale += Vector3.one * steps;
        }

        Destroy(gameObject);
        yield return null;

    }
}
