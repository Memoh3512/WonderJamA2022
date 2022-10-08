using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAnim : MonoBehaviour
{
    // Start is called before the first frame update
    float lifeSpan;
    float Scaleup;
    float startingSize;
    float speed;
    float timeStep;
    Vector2 spawnDirection;
    TextMesh tm;
    void Start()
    {
        timeStep = 0.01f;
        lifeSpan = 0.5f;
        Scaleup = 2;
        speed = 2;
        
        float angle = Random.Range(0, 360)*Mathf.Deg2Rad;
        spawnDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        spawnDirection *= Random.Range(3, 5);
        transform.position += new Vector3(spawnDirection.x,spawnDirection.y);
        tm = GetComponent<TextMesh>();
        startingSize = tm.fontSize;
        StartCoroutine(Animation());
    }

    IEnumerator Animation()
    {
        float diffSize = (startingSize * Scaleup) - startingSize;
        float steps = startingSize / (lifeSpan / timeStep);
        float totalSizeBoost = 0;
        while (tm.fontSize < startingSize * Scaleup)
        {
            yield return new WaitForSeconds(timeStep);
            transform.position += new Vector3(spawnDirection.normalized.x * speed * timeStep, spawnDirection.normalized.y * speed * timeStep);
            totalSizeBoost += steps;
            tm.fontSize += Mathf.RoundToInt(totalSizeBoost);           
        }

        Destroy(gameObject);
        yield return null;

    }
}
