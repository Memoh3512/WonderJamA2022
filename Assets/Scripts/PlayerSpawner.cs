using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{

    public GameObject playerPrefab;

    public GameObject cursorPrefab;
    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < PlayerInputs.MAX_PLAYERS; i++)
        {

            if (PlayerInputs.pControllers[i] != null)
            {
                /*if (i == 0)
                {

                    GameObject mousePlayer = Instantiate(cursorPrefab, Vector3.zero, Quaternion.identity);
                    mousePlayer.GetComponent<MouseControls>().GetPlayerGamepad(i);

                }
                else
                {*/
                    
                    GameObject newPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);  
                    newPlayer.GetComponent<PlayerControls>().GetPlayerGamepad(i);
                    
                //}

                
            }

        }
        
    }

}
