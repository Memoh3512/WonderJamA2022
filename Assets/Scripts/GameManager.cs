using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
enum GameState
{
    Playing,
    Stopped
}
public class GameManager : MonoBehaviour
{
    private List<PlayerControls> players = new List<PlayerControls>();
    private List<Weapon> allWeapons = new List<Weapon>();
    private int turn;
    private GameState gameState;
    private UnityEvent nextTurnEvent = new UnityEvent();
    private UnityEvent nextPlayerEvent = new UnityEvent();
    private int currentPlayerIndex;

    [Header("GO References")] public CinemachineVirtualCamera followCam;
    
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
        
    }
    
    public void GameStart()
    {
        //RESET LES VALEURS <3
        turn = 1;

        foreach (var player in players)
        {
            player.setState(PlayerAction.Waiting);
        }
        
        // Start la game
        currentPlayerIndex = 0;
        FocusOnPlayer(players[0]);
    }
    
    private void NextTurn()
    {
        turn++;
        
        nextTurnEvent.Invoke();
    }
    public void NextPlayerTurn()
    {
        players[currentPlayerIndex].setState(PlayerAction.Waiting);

        // Get le next alive et rollover le playerIndex 

        do
        {

            currentPlayerIndex++;
            currentPlayerIndex %= players.Count;
            if (currentPlayerIndex == 0) NextTurn();

        } while (!players[currentPlayerIndex].isAlive());

        FocusOnPlayer(players[currentPlayerIndex]);

        nextPlayerEvent.Invoke();
    }

    public void FocusOnPlayer(PlayerControls player)
    {
        //TODO set la camera ui etc
        players[currentPlayerIndex].setState(PlayerAction.Moving);

        followCam.Follow = players[currentPlayerIndex].transform;

    }

    public void SwitchToGlobalCam()
    {
        
        followCam.gameObject.SetActive(false);
        
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
    public void setCurrentPlayerState(PlayerAction state)
    {
        Debug.Log("Current state switched : "+state);
        players[currentPlayerIndex].setState(state);
    }
    public Weapon getRandomWeapon()
    {
        return allWeapons[Random.Range(0, allWeapons.Count)];
    }

    public void AddPlayer(PlayerControls player)
    {
        players.Add(player);
    }
}
