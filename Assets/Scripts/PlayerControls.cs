using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Processors;


public enum PlayerAction
{
    Waiting,
    Moving,
    PrepAttack,
}

public class PlayerControls : Damagable
{
    private Manette manette;
    private Rigidbody2D rb;
    private BoxCollider2D characterCollider;
    private Animator animator;
    private Vector2 movementDelta;

    private bool alive = true;

    //Turn
    public float currentStamina = 100;

    //movement setup
    [SerializeField] private PlayerAction state = PlayerAction.Waiting;
    private int fallCount = 0;

    [Header("Player Stats")] public float moveSpeed = 1;
    public float jumpHeight = 1;
    public float gravityScale = 10;
    public float staminaMax = 100;
    public float airControl = 100;
    public int maxHP = 100;

    //change to Weapon class when its created
    private List<Weapon> weapons = new List<Weapon>();
    private Weapon currentWeapon = null;

    private Vector2 storedVelocityBeforeShooting = Vector2.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        characterCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        currentStamina = staminaMax;

        rb.gravityScale = gravityScale;

        //debug
        state = PlayerAction.Waiting;
        
        //Init
        Init(maxHP);
    }

    public void GetPlayerGamepad(int index)
    {
        manette = PlayerInputs.GetPlayerController(index);
    }

    private void Update()
    {
        CheckInputs();

        UpdateAnimValues();
    }

    void UpdateAnimValues()
    {
        
        animator.SetBool("IsGrounded",IsGrounded());
        animator.SetFloat("XVelocity", Math.Abs(rb.velocity.x));
        
    }
    
    void CheckInputs()
    {
        //movement inputs

        switch (state)
        {
            case PlayerAction.Moving:

                if (manette.aButton.wasPressedThisFrame) Jump();
                movementDelta = manette.leftStick;

                if (manette.bButton.wasPressedThisFrame)
                {
                    //TODO Confirmation
                    GameManager.instance.NextPlayerTurn();
                }
                else if (manette.xButton.wasPressedThisFrame)
                {
                    GameManager.instance.setCurrentPlayerState(PlayerAction.PrepAttack);
                    storedVelocityBeforeShooting = rb.velocity;
                    movementDelta = Vector2.zero;
                }
                else if (manette.yButton.wasPressedThisFrame)
                {
                    //TODO peut pas bouger utilise la stamina
                    Weapon gunToAdd = GameManager.instance.getRandomWeapon();
                    weapons.Add(gunToAdd);
                    
                    GameManager.instance.NextPlayerTurn();
                }

                break;
            case PlayerAction.Waiting:
                //Physics guide le joueur :P
                break;
            case PlayerAction.PrepAttack:
                movementDelta = Vector2.zero;
                if (manette.xButton.wasPressedThisFrame)
                {
                    GameManager.instance.setCurrentPlayerState(PlayerAction.Moving);
                    rb.velocity = storedVelocityBeforeShooting;
                    storedVelocityBeforeShooting = Vector2.zero;
                }
                else if (manette.aButton.wasPressedThisFrame)
                {
                    rb.velocity = storedVelocityBeforeShooting;
                    storedVelocityBeforeShooting = Vector2.zero;
                    currentWeapon.Shoot();
                }else if (manette.rightShoulder.wasPressedThisFrame)
                {
                    NextGun();
                }else if (manette.leftShoulder.wasPressedThisFrame)
                {
                    PreviousGun();
                }

                break;
        }
    }

    void CheckStaminaState()
    {
        //Debug.Log("STAMINA: " + currentStamina);
        if (currentStamina <= 0)
        {
            state = PlayerAction.Waiting;
        }
    }

    private void FixedUpdate()
    {
        if (state == PlayerAction.Moving) MovePlayer();
    }

    void MovePlayer()
    {

        var position = transform.position;
        Vector2 delta = (movementDelta * moveSpeed);
        if (IsGrounded())
        {
            
            rb.velocity = new Vector2(delta.x, rb.velocity.y);
            currentStamina -= Math.Abs((delta * Time.deltaTime).x);
            
        }
        else
        {

            Debug.Log("AICONTROL");
            Vector2 di = delta * airControl;
            rb.AddForce(di*Time.deltaTime);

        }

        CheckStaminaState();
    }

    void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);   
        }
        //Debug.Log("JUMP");
    }

    bool IsGrounded()
    {
        bool ret = Physics2D.OverlapBox(characterCollider.bounds.center + (Vector3.down * 0.2f),
            characterCollider.bounds.extents*2f,0, 1 << LayerMask.NameToLayer("Ground")) != null;
        return ret;
    }

    private void OnDrawGizmos()
    {
        //characterCollider = GetComponent<BoxCollider2D>();
        //Gizmos.DrawCube(characterCollider.bounds.center + (Vector3.down * 0.2f), characterCollider.bounds.extents * 2f);
    }

    

    private void EndTurn()
    {
        GameManager.instance.NextPlayerTurn();
    }
    protected override void OnDeath()
    {
        alive = false;
        GameManager.instance.PlayerDied(gameObject);
    }
    public PlayerAction getState()
    {
        return state;
    }
    public void setState(PlayerAction stateToSet)
    {
        state = stateToSet;
    }
    public bool isAlive()
    {
        return alive;
    }
    private void NextGun()
    {
        
    }
    private void PreviousGun()
    {
        
    }
}