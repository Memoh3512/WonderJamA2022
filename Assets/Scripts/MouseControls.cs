using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControls : MonoBehaviour
{
    private Manette manette;

    public void GetPlayerGamepad(int index)
    {

        manette = PlayerInputs.GetPlayerController(index);

    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!(Camera.main is null))
        {
            
            transform.position = (Vector2) Camera.main.ScreenToWorldPoint(manette.mousePosition);   
            
        }
    }
}
