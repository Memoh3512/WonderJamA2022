using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Processors;
using UnityEngine.Serialization;


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
    private GameObject gunHolder;
    
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
    public float maxStamina = 100;
    public float airControl = 10;
    public float maxAirVelocity = 10;
    public int maxHP = 100;
    public float GunHolderDistance = 2f;

    private List<Weapon> weapons = new List<Weapon>();
    private Weapon currentWeapon = null;

    private Vector2 storedVelocityBeforeShooting = Vector2.zero;
    
    public UnityEvent<float> staminaTakenEvent = new UnityEvent<float>();

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        characterCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        gunHolder = transform.Find("GunHolder").gameObject;
        
        currentStamina = maxStamina;

        rb.gravityScale = gravityScale;

        //debug
        weapons.Add(new Sniper());
        weapons.Add(new SMG());
        weapons.Add(new Sniper());
        weapons.Add(new Sniper());
        currentWeapon = weapons[0];
        gunHolder.GetComponent<SpriteRenderer>().sprite = currentWeapon.weaponSprite;
        
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
                UpdateGunHolderPosition();

                if (manette.bButton.wasPressedThisFrame)
                {
                    //TODO Confirmation
                    GameManager.instance.NextPlayerTurn();
                }
                else if (manette.xButton.wasPressedThisFrame)
                {
                    GameManager.instance.setCurrentPlayerState(PlayerAction.PrepAttack);
                    storedVelocityBeforeShooting = rb.velocity;
                    rb.velocity = Vector2.zero;
                    rb.gravityScale = 0;
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
                
                UpdateGunHolderPosition();
                
                if (manette.xButton.wasPressedThisFrame)
                {
                    GameManager.instance.setCurrentPlayerState(PlayerAction.Moving);
                    rb.velocity = storedVelocityBeforeShooting;
                    storedVelocityBeforeShooting = Vector2.zero;
                    rb.gravityScale = gravityScale;
                }
                else if (manette.rightTrigger.wasPressedThisFrame)
                {
                    rb.velocity = storedVelocityBeforeShooting;
                    storedVelocityBeforeShooting = Vector2.zero;
                    rb.gravityScale = gravityScale;

                    Vector2 dir = gunHolder.transform.position - transform.position;
                    
                    currentWeapon.Shoot(gunHolder.transform.position, dir.normalized);
                }else if (manette.dpRight.wasPressedThisFrame)
                {
                    NextGun();
                }else if (manette.dpLeft.wasPressedThisFrame)
                {
                    NextGun(true);
                }

                break;
        }
    }

    void UpdateGunHolderPosition()
    {

        if (manette.rightStick.magnitude > 0.1)
        {
         
            Vector2 angle = manette.rightStick.normalized;

            Vector2 pos = ((Vector2)transform.position) + (angle * GunHolderDistance);

            gunHolder.transform.position = pos;
            gunHolder.transform.eulerAngles = new Vector3(0,0,Mathf.Rad2Deg * Mathf.Atan2(angle.y, angle.x));
            
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
    void RemoveStamina(float toRemove)
    {
        if (toRemove > 0.01)
        {
            currentStamina -= toRemove;
            staminaTakenEvent?.Invoke(currentStamina);
        }
    }
    void MovePlayer()
    {

        var position = transform.position;
        Vector2 delta = (movementDelta * moveSpeed);
        if (IsGrounded())
        {
            rb.velocity = new Vector2(delta.x, rb.velocity.y);
            RemoveStamina(Math.Abs((delta * Time.deltaTime).x));
        }
        else
        {
            //air control
            float di = delta.x * airControl;
            rb.velocity = new Vector2(
                Mathf.Clamp(rb.velocity.x + (di*Time.deltaTime), -maxAirVelocity, maxAirVelocity),
                rb.velocity.y);
            RemoveStamina(Math.Abs(di * Time.deltaTime));
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

        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = LayerMask.GetMask("Ground", "Player");

        Collider2D[] colls = new Collider2D[2];
        bool ret = Physics2D.OverlapBox(characterCollider.bounds.center + (Vector3.down * 0.2f),
            characterCollider.bounds.extents*2f,0,filter,colls) > 1;
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
    private void NextGun(bool reverse = false)
    {
        int currIndex = weapons.IndexOf(currentWeapon);
        
        if (reverse)
        {
            currIndex--;
        }
        else
        {
            currIndex++;
        }
        if (currIndex < 0)
        {
            currIndex = weapons.Count - 1;
        }else if (currIndex >= weapons.Count)
        {
            currIndex = 0;
        }
        currentWeapon = weapons[currIndex];
        Debug.Log("Changing to :"+currIndex);
        gunHolder.GetComponent<SpriteRenderer>().sprite = currentWeapon.weaponSprite;
    }
}