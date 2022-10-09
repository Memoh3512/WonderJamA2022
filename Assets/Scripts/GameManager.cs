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

public enum GlitchType
{
    Player,
    Screen
}

public struct Character
{
    public Sprite spr;
    public string name;

}

public class GameManager : MonoBehaviour
{
    private List<PlayerControls> players = new List<PlayerControls>();
    private int turn = 0;
    private GameState gameState;
    private UnityEvent nextTurnEvent = new UnityEvent();
    private UnityEvent nextPlayerEvent = new UnityEvent();
    private int currentPlayerIndex;

    public static List<Character> characters;

    [Header("Camera")] public CinemachineVirtualCamera followCam;
    public float GlobalCamShowTime = 3f;

    [Header("UI")] 
    public GameObject movingControls;
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
        setUI(movingControls);
    }
    
    public void GameStart()
    {
        //RESET LES VALEURS <3
        foreach (var player in players)
        {
            player.setState(PlayerAction.Waiting);
        }
        NextPlayerTurn(true);
    }
    public void TurnShowText()
    {
        turnReminder.SetActive(true);
        TextMeshProUGUI t = turnReminder.GetComponentInChildren<TextMeshProUGUI>();
        t.color = new Color(t.color.r, t.color.g, t.color.b, 0);
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
    public void NextPlayerTurn(bool start = false)
    {
        
        Time.timeScale = 1;

        setCurrentPlayerState(PlayerAction.Waiting);

        if (start)
        {
            currentPlayerIndex = 0;
            turn = 0;
        }
        else
        {
            do
            {
                currentPlayerIndex++;
                currentPlayerIndex %= players.Count;

            } while (!GetActivePlayer().isAlive());
        }
        

        if (currentPlayerIndex == 0)
        {
            NextTurn();
            //delay call pareil que le else en dessous sauf que ca montre la global cam
            StartCoroutine(GlobalCamCor());
            GetActivePlayer().currentStamina = GetActivePlayer().maxStamina;

            
            SoundPlayer.instance.PlaySFX(Resources.Load<AudioClip>("Sound/SFX/Nextturn_V01"));
            nextPlayerEvent.Invoke();
            if (!start)
            {
                SwapGlitch();    
            }
            
        }
        else
        {
            FocusOnPlayer(GetActivePlayer());
            GetActivePlayer().currentStamina = GetActivePlayer().maxStamina;

            nextPlayerEvent.Invoke();
            if (!start)
            {
                SwapGlitch();    
            }
        }
        
    }

    private IEnumerator GlobalCamCor()
    {
        followCam.gameObject.SetActive(false);
        yield return new WaitForSeconds(GlobalCamShowTime);
        followCam.gameObject.SetActive(true);
        FocusOnPlayer(GetActivePlayer());
    }

    private void SwapGlitch()
    {
        //Glitch à la fin du tour d'un player 2 players swap de place cuz why not
        if (players.Count > 1)
        {
            if (Random.Range(0, 10) == 1)
            {
                int playersAlive = 0;
                foreach(PlayerControls p in players)
                {
                    if (p.isAlive()) playersAlive++;
                }
                if (playersAlive < 2) return;
                PlayerControls player1 = GetRandomAlivePlayer();
                PlayerControls player2;
                do
                {
                    player2 = GetRandomAlivePlayer();

                } while (player2 == player1);

                Glitch(GlitchType.Player, player1);
                Glitch(GlitchType.Player, player2);

                Vector3 position = player1.transform.position;
                player1.transform.position = player2.transform.position;
                player2.transform.position = position;

            }
        }


    }

    public PlayerControls GetRandomAlivePlayer()
    {
        PlayerControls player;
        do
        {
            player = players[Random.Range(0, players.Count)];

        }
        while (!player.isAlive());
        return player;
    }

    public void FocusOnPlayer(PlayerControls player)
    {
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
        SoundPlayer.instance.SetMusic(Songs.Crickets, 1f, TransitionBehavior.Stop);
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
        }
    }
    public Weapon getRandomWeapon()
    {
        Weapon rWeapon = new SMG();
        switch (Random.Range(0, 7))
        {
            case 0: rWeapon =  new Sniper();
                break;
            case 1: rWeapon = new SMG();
                break;
            case 2: rWeapon = new Rocket();
                break;
            case 3: rWeapon = new Shotgun();
                break;
            case 4: rWeapon = new Pearl();
                break;
            case 5: rWeapon = new Poutine();
                break;
            case 6: rWeapon = new Bow();
                break;


        }
        return rWeapon;
    }

    public void AddPlayer(PlayerControls player)
    {
        players.Add(player);
    }

    public void setUI(GameObject ui)
    {
        movingControls.SetActive(false);

        ui.SetActive(true);
    }

    public void Glitch(GlitchType glitchType)
    {
        Glitch(glitchType, GetActivePlayer());
    }

    public void Glitch(GlitchType glitchType, PlayerControls player)
    {
        SoundPlayer.instance.PlaySFX(Resources.Load<AudioClip>("Sound/SFX/Glitch01_V01"));

        switch (glitchType)
        {
            case GlitchType.Player:
                GameObject.Instantiate(Resources.Load<GameObject>("GlitchSmall"), player.gameObject.transform);
                break;
            case GlitchType.Screen:
                GameObject.Instantiate(Resources.Load<GameObject>("GlitchBig"), GameObject.FindGameObjectWithTag("MainCamera").transform);
                break;
        }
    }

    public void PopupText(Vector3 position, float scale, Color color, String text,float lifeSpan = 1.5f)
    {
        GameObject textpop = Instantiate(Resources.Load<GameObject>("PopupText"), position, Quaternion.identity);
        textpop.transform.localScale *= scale;
        textpop.GetComponent<TextMeshPro>().color = color;
        textpop.GetComponent<TextMeshPro>().text = text;
        textpop.GetComponent<TextAnim>().lifeSpan = lifeSpan;
    }
}
