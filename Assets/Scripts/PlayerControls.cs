using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;


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
    private Animator animator;
    private Vector2 movementDelta;

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

    //change to Weapon class when its created
    private int currentWeapon = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        characterCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        currentStamina = staminaMax;

        rb.gravityScale = gravityScale;

        //debug
        state = PlayerAction.Moving;
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
}