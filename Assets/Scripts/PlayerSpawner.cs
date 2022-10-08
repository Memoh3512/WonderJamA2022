using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{

    public GameObject playerPrefab;

    public GameObject cursorPrefab;
    
    // Start is called before the first frame update
    void Start()
    {

        List<GameObject> spawnpoints = GameObject.FindGameObjectsWithTag("Spawnpoint").ToList();
        for (int i = 0; i < PlayerInputs.MAX_PLAYERS; i++)
        {

            if (PlayerInputs.pControllers[i] != null)
            {

                int spawnpointIndex = Random.Range(0, spawnpoints.Count);
                GameObject newPlayer = Instantiate(playerPrefab, 
                    spawnpoints[spawnpointIndex].transform.position,
                    Quaternion.identity);
                
                spawnpoints.RemoveAt(spawnpointIndex);
                
                newPlayer.GetComponent<PlayerControls>().GetPlayerGamepad(i);
                GameManager.instance.AddPlayer(newPlayer.GetComponent<PlayerControls>());

            }

        }
        
        //Start game
        GameManager.instance.GameStart();
        
    }

}
