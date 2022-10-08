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

public struct Character
{
    public Sprite spr;
    public string name;

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

    public static List<Character> characters = new List<Character>()
    {
        new Character()
        {
            spr = Resources.Load<Sprite>("Characters/Coccinelle"),
            name = "Ladybug"
        },
        new Character()
        {
            spr = Resources.Load<Sprite>("Characters/Sauterelle"),
            name = "Grasshopper"
        },
    };

    [Header("GO References")] public CinemachineVirtualCamera followCam;

    [Header("UI")] 
    public GameObject movingControls;
    public GameObject prepAttackControls;
    public GameObject turnReminder;
    
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
        movingControls.SetActive(false);
        prepAttackControls.SetActive(false);
        //players[currentPlayerIndex].ShowUI();
        setUI(movingControls);
    }
    
    public void GameStart()
    {
        //RESET LES VALEURS <3
        turn = 1;

        foreach (var player in players)
        {
            player.setState(PlayerAction.Waiting);
            player.ShowUI(false);
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
        foreach (var player in players)
        {
            player.ShowUI(false);
        }
        players[currentPlayerIndex].setState(PlayerAction.Waiting);

        // Get le next alive et rollover le playerIndex 

        do
        {
            currentPlayerIndex++;
            currentPlayerIndex %= players.Count;
            if (currentPlayerIndex == 0) NextTurn();

        } while (!players[currentPlayerIndex].isAlive());

        FocusOnPlayer(players[currentPlayerIndex]);
        players[currentPlayerIndex].currentStamina = players[currentPlayerIndex].maxStamina;

        nextPlayerEvent.Invoke();
    }

    public void FocusOnPlayer(PlayerControls player)
    {
        player.ShowUI();
        setUI(movingControls);
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
        
        if (players[currentPlayerIndex] == deadPlayer.GetComponent<PlayerControls>())
        {
            NextPlayerTurn();
        }
    }
    public void GameEnd()
    {
        
    }
    public void setCurrentPlayerState(PlayerAction state)
    {
        Debug.Log("Current state switched : "+state);
        players[currentPlayerIndex].setState(state);
        switch (state)
        {
            case PlayerAction.Moving:
                setUI(instance.movingControls);
                break;
            case PlayerAction.PrepAttack:
                setUI(instance.prepAttackControls);
                break;
        }
    }
    public Weapon getRandomWeapon()
    {
        return allWeapons[Random.Range(0, allWeapons.Count)];
    }

    public void AddPlayer(PlayerControls player)
    {
        players.Add(player);
    }

    public void setUI(GameObject ui)
    {
        movingControls.SetActive(false);
        prepAttackControls.SetActive(false);
        turnReminder.SetActive(false);

        ui.SetActive(true);
    }
}
