using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterX : MonoBehaviour
{

    private float delay;

    private void Start()
    {

        delay = Math.Max(0, delay);
        Invoke(nameof(Destroy),delay);

    }

    private void Destroy()
    {
        
        Destroy(gameObject);
        
    }
    
}
