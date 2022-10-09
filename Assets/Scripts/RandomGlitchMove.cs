using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGlitchMove : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(TP());
    }


    IEnumerator TP()
    {
        yield return new WaitForSeconds(15);
        if(Random.Range(0,10) == 5)
        {
            GameManager.instance.Glitch(GlitchType.Screen);
            yield return new WaitForSeconds(0.5f);
            transform.position = new Vector3(transform.position.x + Random.Range(-10, 10), transform.position.y + Random.Range(-10, 10), transform.position.z);
        }
        StartCoroutine(TP());
    }

}
