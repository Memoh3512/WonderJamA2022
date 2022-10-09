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
    [Range(1, 4)] public int playerNb = 1;

    private bool joined = false;

    private int currentCharacterIndex = 0;

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
        if (PlayerExists(playerNb - 1))
        {
            if (PlayerInputs.GetPlayerController(playerNb - 1).gp.enabled)
            {

                GameObject.Find("BlinkText").GetComponent<Animator>().SetTrigger("Appear");
                if (pressStartText != null) pressStartText.gameObject.SetActive(false);

                //character chooser setup

                if (joined)
                {
                    if (PlayerInputs.GetPlayerController(playerNb - 1).lJoyRight.wasPressedThisFrame)
                    {
                        currentCharacterIndex = (currentCharacterIndex + 1) % GameManager.characters.Count;
                        SetCharacter(GameManager.characters[currentCharacterIndex]);
                    } else if (PlayerInputs.GetPlayerController(playerNb - 1).lJoyLeft.wasPressedThisFrame)
                    {
                        currentCharacterIndex--;
                        if (currentCharacterIndex < 0) currentCharacterIndex = GameManager.characters.Count - 1;
                        SetCharacter(GameManager.characters[currentCharacterIndex]);
                    }
                }
                else
                {
                    SetCharacter(GameManager.characters[0]);
                    
                    
                    SoundPlayer.instance.PlaySFX(Resources.Load<AudioClip>("Sound/SFX/Join_V01"));
                    
                    joined = true;
                }

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
                SoundPlayer.instance.SetMusic(Songs.GameplaySong, 1, TransitionBehavior.Stop);
                
                
                SoundPlayer.instance.PlaySFX(Resources.Load<AudioClip>("Sound/SFX/Gamestart_V01"));
            }
        }
    }

    private void SetCharacter(Character c)
    {

        transform.Find("CharacterChoosed").GetComponent<Image>().sprite = c.spr;
        transform.Find("CharacterChoosed").GetComponent<Image>().color = new Color(1,1,1,1);
        transform.Find("CharacterName").GetComponent<TextMeshProUGUI>().text = c.name;

        PlayerInputs.GetPlayerController(playerNb - 1).character = c.name;

    }

    private bool PlayerExists(int player)
    {
        //Debug.Log("Pla");
        return PlayerInputs.GetPlayerController(player) != null;
    }
}