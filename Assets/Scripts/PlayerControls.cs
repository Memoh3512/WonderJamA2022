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

public class PlayerControls : MonoBehaviour
{
    private Manette manette;
    private Rigidbody2D rb;
    private BoxCollider2D characterCollider;
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

    //change to Weapon class when its created
    private List<Weapon> weapons = new List<Weapon>();
    private Weapon currentWeapon = null;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        characterCollider = GetComponent<BoxCollider2D>();
        currentStamina = staminaMax;

        rb.gravityScale = gravityScale;

        //debug
        state = PlayerAction.Waiting;
    }

    public void GetPlayerGamepad(int index)
    {
        manette = PlayerInputs.GetPlayerController(index);
    }

    private void Update()
    {
        CheckInputs();
    }

    void CheckInputs()
    {
        //movement inputs

        switch (state)
        {
            case PlayerAction.Moving:

                if (manette.aButton.wasPressedThisFrame) Jump();
                movementDelta = manette.leftStick;

                break;
            case PlayerAction.Waiting:

                movementDelta = Vector2.zero;

                break;
            case PlayerAction.PrepAttack:

                movementDelta = Vector2.zero;

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
        Vector2 delta = (manette.leftStick * moveSpeed);
        rb.velocity = new Vector2(delta.x, rb.velocity.y);
        currentStamina -= Math.Abs((delta * Time.deltaTime).x);

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
        Debug.Log(ret);
        return ret;
    }

    private void OnDrawGizmos()
    {
        characterCollider = GetComponent<BoxCollider2D>();
        Gizmos.DrawCube(characterCollider.bounds.center + (Vector3.down * 0.2f), characterCollider.bounds.extents * 2f);
    }

    public PlayerAction getPlayerState()
    {
        return state;
    }

    private void EndTurn()
    {
        GameManager.instance.NextPlayerTurn();
    }
    private void OnDeath()
    {
        alive = false;
    }
    public void setStateWaiting()
    {
        state = PlayerAction.Waiting;
    }
    public bool isAlive()
    {
        return alive;
    }
}