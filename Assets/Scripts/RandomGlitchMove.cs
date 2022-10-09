using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGlitchMove : MonoBehaviour
{
    public float scaleBouger = 1;

    private void Start()
    {
        StartCoroutine(TP());
    }


    IEnumerator TP()
    {
        yield return new WaitForSeconds(5);
        if(Random.Range(0,20) == 5)
        {
            GameManager.instance.Glitch(GlitchType.Screen);
            yield return new WaitForSeconds(0.5f);
            float delta = 10 * scaleBouger;
            transform.position = new Vector3(transform.position.x + Random.Range(-delta, delta), transform.position.y + Random.Range(-delta, delta), transform.position.z);
        }
        StartCoroutine(TP());
    }

}
