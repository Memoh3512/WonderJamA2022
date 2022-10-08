using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
enum GameState
{
    Playing,
    Stopped
}
public class NewBehaviourScript : MonoBehaviour
{
    private List<PlayerControls> players;
    private List<Weapon> allWeapons;
    private int turn;
    private GameState gameState;
    private UnityEvent nextTurnEvent;
    private UnityEvent nextPlayerEvent;


    public void NextTurn()
    {
        nextTurnEvent.Invoke();
    }
    public void NextPlayer()
    {
        
        nextPlayerEvent.Invoke();
    }
    
    void Start()
    {
        GameStart();
    }

    void GameStart()
    {
        //RESET LES VALEURS <3
        turn = 1;
    }

    private PlayerControls GetActivePlayer()
    {
        foreach (var player in players)
        {
            if (player.getPlayerState() != PlayerAction.Waiting)
            {
                return player;
            }
        }
        Debug.LogError("Returning default player none where not waiting");
        return players[0];
    }
}
