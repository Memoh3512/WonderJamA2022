using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private GameObject cam;
    public float multiplier = 1;
    public bool x = true;
    public bool y = false;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 delta = cam.transform.position;
        delta = (-delta) * multiplier;
        if (!x) delta.x = 0;
        if (!y) delta.y = 0;
        transform.position = delta;


    }
}
