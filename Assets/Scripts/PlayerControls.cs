using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Processors;
using UnityEngine.Serialization;
using TMPro;
using Random = UnityEngine.Random;


public enum PlayerAction
{
    Waiting,
    Moving,
}

public class PlayerControls : Damagable
{
    private Manette manette;
    private Rigidbody2D rb;
    private Collider2D characterCollider;
    private Animator animator;
    private SpriteRenderer sprite;
    private GameObject gunHolder;
    
    private Vector2 movementDelta;

    private bool alive = true;

    //Turn
    public float currentStamina = 100;

    //movement setup
    [SerializeField] private PlayerAction state = PlayerAction.Waiting;
    private int fallCount = 0;
    private bool isSpriteFlipped = false;
    private bool isGunFlipped = false;

    [Header("Global Player Params")] public float flipStickDeadzone = 0.5f;
    public float flipAngleLeeway;
    public float zoomSpeed = 1;
    public float maxIKDistance = 1;
    public float startShootTimeScale = 0.05f;
    public float slomoTimeScale = 0.2f;
    public int fallDamage = 25;

    [Header("Player Stats")] public float moveSpeed = 1;
    public float jumpHeight = 1;
    public float gravityScale = 10;
    public float maxStamina = 100;
    public float airControl = 10;
    public float maxAirVelocity = 10;
    public int maxHP = 100;
    public float GunHolderDistance = 2f;
    public float staminaUsageMultiplier = 3f;

    [Header("Audio")] public AudioClip jumpSFX;
    public AudioClip footstepSFX;

    private List<Weapon> weapons = new List<Weapon>();
    private Weapon currentWeapon = null;
    
    public UnityEvent<float> staminaTakenEvent = new UnityEvent<float>();
    public UnityEvent<Weapon,Weapon> changeGunEvent = new UnityEvent<Weapon,Weapon>();
    public UnityEvent unShowCostEvent = new UnityEvent();
    
    public List<GameObject> toNotShowOnOthersTurn = new List<GameObject>();

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        characterCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        gunHolder = transform.Find("GunHolder").gameObject;
        sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        
        currentStamina = maxStamina;

        rb.gravityScale = gravityScale;

        //debug
        weapons.Add(new Sniper());
        weapons.Add(new SMG());
        weapons.Add(new Rocket());
        weapons.Add(new Shotgun());
        weapons.Add(new Pearl());
        weapons.Add(new Poutine());
        //ENSEMBLE {
        currentWeapon = weapons[0];
        
        //}
        
        gunHolder.GetComponent<SpriteRenderer>().sprite = currentWeapon.weaponSprite;
        gunHolder.transform.position = ((Vector2)transform.position) + (Vector2.right * GunHolderDistance);;
        
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

        if (currentWeapon != null)
        {

            if (currentWeapon.cooldown > 0)
            {
                currentWeapon.cooldown -= Time.deltaTime;
                if (currentWeapon.cooldown < 0)
                {
                    currentWeapon.cooldown = 0;
                }
            }
        }
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


                    if (!IsGrounded())
                    {
                        if (Math.Abs(Time.timeScale - 1f) < 0.01f)
                        {
                            
                            SoundPlayer.instance.PlaySFX(Resources.Load<AudioClip>("Sound/SFX/Slowmoin_V01"));
                            
                        }
                        Time.timeScale = startShootTimeScale;
                    }
                    
                }else if (manette.rightTrigger.wasPressedThisFrame)
                {


                    if (!IsGrounded())
                    {
                        
                        if (Time.timeScale < 1f)
                        {

                            Time.timeScale = slomoTimeScale;

                        }
                    }

                    if (currentWeapon != null)
                    {
                        
                        TryShoot(true);
                        if (currentWeapon != null)
                        {

                            if (currentWeapon.getFirerate() == 0)
                            {
                                ResetTime();
                            }

                        }
                        else ResetTime();

                    }

                }else if (manette.rightTrigger.isPressed)
                {
                    if (currentWeapon != null)
                    {
                        
                        if (currentWeapon.getFirerate() != 0) TryShoot();
                        if (IsGrounded() && Time.timeScale < 1f)
                        {
                            Debug.Log("SMGWATF GROUNDPRESSED");
                            ResetTime();
                        } 
                        
                    }
                }
                else if (manette.rightTrigger.wasReleasedThisFrame)
                {

                    if (Time.timeScale < 1f)
                    {

                        ResetTime();   
                        
                    }
                }
                else if (manette.dpRight.wasPressedThisFrame)
                {
                    NextGun();
                }else if (manette.dpLeft.wasPressedThisFrame)
                {
                    NextGun(true);
                }
                else if (manette.yButton.wasPressedThisFrame)
                {
                    GetWeapon();
                }

                break;
            case PlayerAction.Waiting:
                //Physics guide le joueur :P
                break;
        }
        //zoom
        if (state != PlayerAction.Waiting)
        {
            if (manette.rightShoulder.isPressed) FindObjectOfType<PlayerFollowCam>().Zoom(-zoomSpeed*Time.deltaTime);
            if (manette.leftShoulder.isPressed) FindObjectOfType<PlayerFollowCam>().Zoom(zoomSpeed*Time.deltaTime);
        }
    }

    void ResetTime()
    {

        if (Time.timeScale < 1f)
        {
            SoundPlayer.instance.PlaySFX(Resources.Load<AudioClip>("Sound/SFX/Slowmoout_V01"));
            Time.timeScale = 1f;
        }
    }
    
    void TryShoot(bool singlePress=false)
    {
        if (currentWeapon != null)
        {
            if (CanShootWeaponCooldown())
            {
                if (CanShootWeaponStamina())
                {
                    Vector2 dir = gunHolder.transform.position - transform.position;
                    RemoveStamina(currentWeapon.getStaminaCost());
                    currentWeapon.Shoot(gunHolder.transform.position, dir.normalized);
                    CheckStaminaState();
                }
                else if(singlePress)
                {
                    GameObject text = Instantiate(Resources.Load<GameObject>("PopupText"), transform.position, Quaternion.identity);
                    text.GetComponent<TextMeshPro>().text = "No stamina!";
                    text.transform.localScale *= 0.5f;
                    text.GetComponent<TextMeshPro>().color = Color.cyan;
                }
            }
        }
    }

    void UpdateGunHolderPosition()
    {
        if (manette.rightStick.magnitude > flipStickDeadzone)
        {
            Vector2 angle = manette.rightStick.normalized;

            Vector2 pos = ((Vector2)transform.position) + (angle * GunHolderDistance);

            gunHolder.transform.position = pos;
            gunHolder.transform.eulerAngles = new Vector3(0,0,Mathf.Rad2Deg * Mathf.Atan2(angle.y, angle.x));
        }
        currentWeapon?.OnGunHolderMove(gunHolder.transform.position, gunHolder.transform.position - transform.position);
    }

    public void Heal(int heal)
    {
        hp += heal;
        if (hp > maxHP)
        {
            GameManager.instance.Glitch(GlitchType.Player);
        }
        damageTakenEvent?.Invoke(hp);
    }

    void GetWeapon()
    {
        if(currentStamina >= 75)
        {
            RemoveStamina(75);
            Weapon gunToAdd = GameManager.instance.getRandomWeapon();
            if(Random.Range(1, 10) == 5)
            {
                gunToAdd.setWeaponName(gunToAdd.getWeaponName() + "?");
                Weapon weapon2;
                do
                {
                    weapon2 = GameManager.instance.getRandomWeapon();
                } while (weapon2.getProjectilePrefab() == null);
                gunToAdd.setProjectilePrefab(weapon2.getProjectilePrefab());
                GameManager.instance.Glitch(GlitchType.Player);
            }
            bool weaponAlreadyAcquired = false;
            foreach(Weapon w in weapons)
            {
                if(gunToAdd.getWeaponName() == w.getWeaponName())
                {
                    weaponAlreadyAcquired = true;
                    w.addAmmos(gunToAdd.getProjectileCount());
                    gunToAdd.setWeaponName(w.getWeaponName() + " Ammos x" + gunToAdd.getProjectileCount());
                }
            }
            if(!weaponAlreadyAcquired)
                weapons.Add(gunToAdd);
            GameObject popup = GameObject.Instantiate(Resources.Load<GameObject>("GetWeaponPopup"), transform);
            popup.transform.position += Vector3.up * 2;
            popup.GetComponent<WeaponPopup>().Setup(gunToAdd);
            if (weapons.Count == 1) NextGun();
        }
        else
        {
            GameObject text = Instantiate(Resources.Load<GameObject>("PopupText"), transform.position, Quaternion.identity);
            text.GetComponent<TextMeshPro>().text = "No Stamina!";
            text.GetComponent<TextMeshPro>().color = Color.cyan;
        }
    }

    public void RemoveWeapon(Weapon weapon)
    {
        weapons.Remove(weapon);
        NextGun();
        ResetTime();
    }

    void CheckStaminaState()
    {
        //Debug.Log("STAMINA: " + currentStamina);
        if (currentStamina <= 0)
        {
            GameManager.instance.NextPlayerTurn();
        }
    }

    private void FixedUpdate()
    {
        if (state == PlayerAction.Moving)
        {
            MovePlayer();
            UpdateSpriteIK();
        }
        if (state != PlayerAction.Waiting) UpdateSpriteFlip();
    }
    void RemoveStamina(float toRemove)
    {
        if (toRemove > 0)
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
            RemoveStamina(Math.Abs((delta * Time.deltaTime).x)*staminaUsageMultiplier);
        }
        else
        {
            //air control
            float di = delta.x * airControl;
            rb.velocity = new Vector2(
                Mathf.Clamp(rb.velocity.x + (di*Time.deltaTime), -maxAirVelocity, maxAirVelocity),
                rb.velocity.y);
            RemoveStamina(Math.Abs(rb.velocity.x * Time.deltaTime));
        }
        CheckStaminaState();
    }

    void UpdateSpriteFlip()
    {
        //gun sprite flip
        if (manette.rightStick.magnitude > flipStickDeadzone)
        {
            float angle = Mathf.Rad2Deg * Mathf.Atan2(manette.rightStick.y, manette.rightStick.x);
            if (!isGunFlipped && (angle > 90 + flipAngleLeeway || angle < -90 - flipAngleLeeway))
            {
                isSpriteFlipped = true;
                sprite.flipX = true;
                isGunFlipped = true;
                gunHolder.GetComponent<SpriteRenderer>().flipY = true;
            } 
            else if ( isGunFlipped && angle < 90 - flipAngleLeeway && angle > -90 + flipAngleLeeway)
            {
                isGunFlipped = false;
                gunHolder.GetComponent<SpriteRenderer>().flipY = false;
                isSpriteFlipped = false;
                sprite.flipX = false;
            }

        }
        else if (IsGrounded() && manette.leftStick.magnitude > flipStickDeadzone)
        {
            //player sprite flip
            float angle = Mathf.Rad2Deg*Mathf.Atan2(movementDelta.y, movementDelta.x);

            if (!isSpriteFlipped && (angle > 90 + flipAngleLeeway || angle < -90 - flipAngleLeeway))
            {
                isSpriteFlipped = true;
                sprite.flipX = true;
            } else if (isSpriteFlipped && angle < 90 - flipAngleLeeway && angle > -90 + flipAngleLeeway)
            {
                isSpriteFlipped = false;
                sprite.flipX = false;
            }
        }
    }

    void UpdateSpriteIK()
    {

        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = 1 << LayerMask.NameToLayer("Ground");

        RaycastHit2D[] hit = new RaycastHit2D[2];
        if (Physics2D.Raycast(transform.position, Vector2.down, filter, hit) > 1)
        {
            if (hit[1].distance <= maxIKDistance)
            {
                //Debug.Log("DIST: " + hit[1].distance);
                Vector2 normal = hit[1].normal;
                float angle = Mathf.Rad2Deg * Mathf.Atan2(normal.y, normal.x);
                angle -= 90; //tangent a la surface

                sprite.transform.eulerAngles = new Vector3(0,0,angle);   
            }
            else
            {
                sprite.transform.eulerAngles = Vector3.zero;
            }
        }
    }

    void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);

            if (jumpSFX != null)
            {
                SoundPlayer.instance.PlaySFX(jumpSFX);
            }
        }
        //Debug.Log("JUMP");
    }

    public void PlayFootstepSFX()
    {
        if (footstepSFX != null)
        {
            SoundPlayer.instance.PlaySFX(footstepSFX);
        }
    }

    bool IsGrounded()
    {

        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = LayerMask.GetMask("Ground", "Player");

        Collider2D[] colls = new Collider2D[2];
        bool ret = Physics2D.OverlapCircle(characterCollider.bounds.center + (Vector3.down * 0.1f),
            ((CircleCollider2D)characterCollider).radius,filter,colls) > 1;
        if (colls[1] != null)Debug.Log( colls[0].gameObject.name + " - " + colls[1].gameObject.name);
        return ret;
    }

    protected override void OnDeath()
    {
        Die();
    }
    public PlayerAction getState()
    {
        return state;
    }
    public void setState(PlayerAction stateToSet)
    {
        state = stateToSet;
        if (state == PlayerAction.Moving)
        {
            showUI(true);
            
        }
        if (state == PlayerAction.Waiting)
        {
            showUI(false);
        }
    }
    public bool isAlive()
    {
        return alive;
    }
    private void NextGun(bool reverse = false)
    {
        Weapon oldGun = currentWeapon;
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
        if (weapons.Count > 0)
        {
            currentWeapon = weapons[currIndex];
            gunHolder.GetComponent<SpriteRenderer>().sprite = currentWeapon.weaponSprite;
            changeGunEvent.Invoke(currentWeapon, oldGun);
        }
        else
        {
            gunHolder.GetComponent<SpriteRenderer>().sprite = null;
            currentWeapon = null;
            
            GameObject text = Instantiate(Resources.Load<GameObject>("PopupText"), transform.position, Quaternion.identity);
            text.GetComponent<TextMeshPro>().text = "No gun!";
            text.transform.localScale *= 0.7f;
            text.GetComponent<TextMeshPro>().color = Color.yellow;
        }
    }
    public bool CanShootWeaponStamina()
    {
        if (currentWeapon != null)
        {
            if (currentStamina >= currentWeapon.getStaminaCost())
            {
                return true;
            }   
        }
        return false;
    }
    public bool CanShootWeaponCooldown()
    {
        return currentWeapon.OutOfCooldown();
    }

    public void Fall(float top)
    {

        if (TakeDamage(fallDamage))
        {
            Die();
        }
        else
        {
            //respawn to a random spawnpoint
            GameObject[] spawnpoints = GameObject.FindGameObjectsWithTag("Spawnpoint");
            Vector3 pos = spawnpoints[Random.Range(0, spawnpoints.Length)].transform.position;
            transform.position = pos;
            
            
            SoundPlayer.instance.PlaySFX(Resources.Load<AudioClip>("Sound/SFX/Void_V01"));
        }
    }

    private void Die()
    {
        SoundPlayer.instance.PlaySFX(Resources.Load<AudioClip>("Sound/SFX/Die"));
        
        alive = false;
        gameObject.SetActive(false);
        GameManager.instance.PlayerDied(gameObject);
    }

    public void Won()
    {
        manette.Winner = true;
    }
    private void showUI(bool show = true)
    {
        foreach (var ui in toNotShowOnOthersTurn)
        {
            ui.SetActive(show);
        }
        if (show)
        {
            staminaTakenEvent?.Invoke(currentStamina);
        }
        else
        {
            currentWeapon?.turnOffLineRenderer();
        }
        changeGunEvent.Invoke(currentWeapon, null);
    }
    public Weapon GetGun()
    {
        return currentWeapon;
    }
}