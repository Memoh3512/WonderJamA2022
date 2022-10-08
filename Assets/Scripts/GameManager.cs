using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
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

        //GetActivePlayer().ShowUI();
        setUI(movingControls);
    }
    
    public void GameStart()
    {
        //RESET LES VALEURS <3
        turn = 1;
        TurnShowText();

        foreach (var player in players)
        {
            player.setState(PlayerAction.Waiting);
            player.PlayerTargetted(false);
        }
        
        // Start la game
        currentPlayerIndex = 0;
        FocusOnPlayer(players[0]);
    }
    public void TurnShowText()
    {
        turnReminder.SetActive(true);
        TextMeshProUGUI t = turnReminder.GetComponentInChildren<TextMeshProUGUI>();
        t.text = "Turn " + turn;
        StartCoroutine(FadeTextToFullAlpha(2f,t));
    }
    public IEnumerator FadeTextToFullAlpha(float t, TextMeshProUGUI i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            if (i.color.a >= 1f)
            {
                yield return StartCoroutine(FadeTextToZeroAlpha(t,i));
                break;
            }
            yield return null;
        }
    }
 
    public IEnumerator FadeTextToZeroAlpha(float t, TextMeshProUGUI i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
    
    private void NextTurn()
    {
        turn++;
        TurnShowText();
        
        nextTurnEvent.Invoke();
    }
    public void NextPlayerTurn()
    {
        foreach (var player in players)
        {
            player.PlayerTargetted(false);
        }
        setCurrentPlayerState(PlayerAction.Waiting);

        do
        {
            currentPlayerIndex++;
            currentPlayerIndex %= players.Count;
            if (currentPlayerIndex == 0) NextTurn();

        } while (!GetActivePlayer().isAlive());

        FocusOnPlayer(GetActivePlayer());
        GetActivePlayer().currentStamina = GetActivePlayer().maxStamina;

        nextPlayerEvent.Invoke();
    }

    public void FocusOnPlayer(PlayerControls player)
    {
        player.PlayerTargetted();
        setCurrentPlayerState(PlayerAction.Moving);

        followCam.Follow = GetActivePlayer().transform;
    }

    public void SwitchToGlobalCam()
    {
        
        followCam.gameObject.SetActive(false);
        
    }

    public PlayerControls GetActivePlayer()
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
        }else if (GetActivePlayer() == deadPlayer.GetComponent<PlayerControls>())
        {
            NextPlayerTurn();
        }
    }
    public void GameEnd()
    {
        GetActivePlayer().Won();
        SceneChanger.ChangeScene(SceneTypes.EndScreen);
    }
    public void setCurrentPlayerState(PlayerAction state)
    {
        Debug.Log("Current state switched : "+state);
        GetActivePlayer().setState(state);
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

        ui.SetActive(true);
    }
}
