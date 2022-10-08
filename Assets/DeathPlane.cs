using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    private float levelTop = 0;
    // Start is called before the first frame update
    void Start()
    {

        GameObject top = GameObject.FindGameObjectWithTag("LevelTop");
        if (top != null)
        {

            levelTop = top.transform.position.y;

        }

    }

    private void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.CompareTag("Player"))
        {

            var player = col.gameObject.GetComponent<PlayerControls>();
            player.Fall(levelTop);

        }
        
    }
}
