using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
enum GameState
{
    Playing,
    Stopped
}
public class GameManager : MonoBehaviour
{
    private List<PlayerControls> players;
    private List<Weapon> allWeapons;
    private int turn;
    private GameState gameState;
    private UnityEvent nextTurnEvent;
    private UnityEvent nextPlayerEvent;
    private int currentPlayerIndex;

    
    // Singleton
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        GameStart();
    }
    
    void GameStart()
    {
        //RESET LES VALEURS <3
        turn = 1;

        foreach (var player in players)
        {
            player.setStateWaiting();
        }
        
        // Start la game
        currentPlayerIndex = 0;
        FocusOnPlayer(players[0]);
    }
    
    private void NextTurn()
    {
        turn += 1;
        
        nextTurnEvent.Invoke();
    }
    public void NextPlayerTurn()
    {
        players[currentPlayerIndex].setStateWaiting();

        // Get le next alive et rollover le playerIndex 
        do
        {
            currentPlayerIndex++;
            if (players.Count > currentPlayerIndex)
            {
                currentPlayerIndex = 0;
                NextTurn();
            }
        } while (!players[currentPlayerIndex].isAlive());
        

        FocusOnPlayer(players[currentPlayerIndex]);
        
        nextPlayerEvent.Invoke();
    }

    public void FocusOnPlayer(PlayerControls player)
    {
        //TODO set la camera ui etc
        
    }
    

    

    private PlayerControls GetActivePlayer()
    {
        return players[currentPlayerIndex];
    }

    public void PlayerDied(GameObject deadPlayer)
    {
        //TODO call ca
        int count=0;
        foreach (var p in players)
        {
            if (p.isAlive())
            {
                count++;
            }
        }
        if (count <= 1)
        {
            GameEnd();
        }
    }
    public void GameEnd()
    {
        
    }
}
