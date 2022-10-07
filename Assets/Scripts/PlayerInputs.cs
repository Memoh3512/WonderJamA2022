using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public static class PlayerInputs
{
    
    public const int MAX_PLAYERS = 4;

    public static Manette[] pControllers = new Manette[MAX_PLAYERS];
    private static int playerAdded = 0;
    public static List<Manette> gamepads = new List<Manette>();
    
    public static void SetPlayerController(Manette device, int player)
    {

        if (player >= 0 && player < MAX_PLAYERS)
        {

            pControllers[player] = device;

        }
        
    }

    public static Manette GetPlayerController(int player)
    {

        if (player >= 0 && player < MAX_PLAYERS) return pControllers[player];
        else return null;

    }

    public static void AddPlayerController(Manette ctrl)
    {

        if (playerAdded < MAX_PLAYERS)
        {
            
            pControllers[playerAdded] = ctrl;
            playerAdded++;
            Debug.Log("New player added to game: ");
            
        }

    }
    
    
}
