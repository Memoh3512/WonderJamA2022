using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerJoinTester : MonoBehaviour
{
    private PlayerInput playerInput { get; set; }
    public Animator pressStartText;
    [Range(1,4)]
    public int playerNb = 1;

    private void Start()
    {

        if (playerNb <= PlayerInputs.MAX_PLAYERS)
        {
            
            pressStartText.SetTrigger("Appear");
            
        }
        else
        {
            
            Debug.Log("DELETE");
            gameObject.SetActive(false);

        }

    }

    private void Update()
    {

        if (PlayerExists(playerNb-1))
        {
            
            if (PlayerInputs.GetPlayerController(playerNb-1).gp.enabled)
            {
            
                Color col = GetComponent<Image>().color;
                float h,s,v;
                Color.RGBToHSV(col, out h, out s, out v);
                col = Color.HSVToRGB(h, s, 1);
                GetComponent<Image>().color = col;
                
                GameObject.Find("BlinkText").GetComponent<Animator>().SetTrigger("Appear");
                if (pressStartText != null) pressStartText.gameObject.SetActive(false);

            }   
            
        }

        if (PlayerExists(0))
        {

            if (PlayerInputs.GetPlayerController(0).selectButton.wasPressedThisFrame)
            {

                for (int i = 0; i < PlayerInputs.MAX_PLAYERS; i++)
                {

                    if (PlayerInputs.GetPlayerController(i) != null)
                    {
                        
                        PlayerInputs.GetPlayerController(i).Rumble(0.1f);
                        
                    }
                    
                }
                
                SceneChanger.ChangeScene(SceneTypes.GameplayScene, TransitionTypes.CoolTransition);

            }
            
        }

    }

    private bool PlayerExists(int player)
    {

        //Debug.Log("Pla");
        return PlayerInputs.GetPlayerController(player) != null;

    }
}
