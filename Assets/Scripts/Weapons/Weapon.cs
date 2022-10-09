using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Weapon
{
    public Sprite weaponSprite;
    protected GameObject projectilePrefab;
    protected float staminaCost;
    protected float fireRate;
    protected float knockback;
    protected Vector2 shootingOffset;
    protected int projectileCount;
    protected string weaponName;
    private LineRenderer lr;
    protected float offset;
    private float gravityScale;
    private bool lrActive;
    private float speed;
    private float drag;

    public UnityEvent<Weapon> gunShotEvent = new UnityEvent<Weapon>();

    protected GameObject lastProjectile;
    public UnityEvent ammoUsedEvent = new UnityEvent();

    public float cooldown = 0;

    public Weapon(float staminaCost, float fireRate, float knockback, int projectileCount, Vector2 shootingOffset, Sprite weaponSprite, GameObject projectilePrefab, string weaponName = "", float offset = 0f, bool lrActive = true)
    {
        this.staminaCost = staminaCost;
        this.fireRate = fireRate;
        this.knockback = knockback;
        this.projectileCount = projectileCount;
        this.shootingOffset = shootingOffset;
        this.weaponSprite = weaponSprite;
        this.projectilePrefab = projectilePrefab;
        this.offset = offset;
        this.lrActive = lrActive;

        if (weaponName == "")
            this.weaponName = this.GetType().ToString();
        else this.weaponName = weaponName;

        lr = GameObject.Find("PreviewRenderer").GetComponent<LineRenderer>();
        if(projectilePrefab != null)
        {
            gravityScale = projectilePrefab.GetComponent<GameProjectile>().gravityScale;
            speed = projectilePrefab.GetComponent<GameProjectile>().speed;
            drag = projectilePrefab.GetComponent<Rigidbody2D>().drag;
        }

    }

    public void OnGunHolderMove(Vector2 position, Vector2 shootDirection)
    {

        if (projectilePrefab == null || !lrActive)
        {
            lr.positionCount = 0;
            return;
        }
        position += shootDirection.normalized * offset;
        int steps = Mathf.RoundToInt(2500f/speed);
        List<Vector3> points = new List<Vector3>();
        float timeStep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        Vector2 gravity = Physics2D.gravity * gravityScale * timeStep * timeStep;
        float drg = 1f - timeStep * drag;
        Vector2 moveStep = shootDirection.normalized * speed * timeStep;

        for(int i = 0; i < steps; i++)
        {
            moveStep += gravity;
            moveStep *= drg;
            position += moveStep;
            points.Add(position);

        }
        for(int i = 0; i < Mathf.RoundToInt(steps*0.1f); i++)
        {
            points.RemoveAt(0);
        }
        Vector3[] finalPoints = new Vector3[points.Count];
        for(int i = 0; i < points.Count; i++)
        {
            finalPoints[i] = points[i];
        }
        lr.positionCount = finalPoints.Length;
        lr.SetPositions(finalPoints);
    }

    public void turnOffLineRenderer()
    {
        lr.positionCount = 0;
    }

    virtual public void Shoot(Vector2 position, Vector2 shootDirection)
    {
        cooldown =  60/fireRate;
        Debug.Log(shootDirection);
        lastProjectile = GameObject.Instantiate(projectilePrefab, position + shootDirection* offset, Quaternion.identity);
        lastProjectile.transform.position += new Vector3(shootingOffset.x,shootingOffset.y);
        GameManager.instance.GetActivePlayer().GetComponent<Rigidbody2D>().velocity += ((-shootDirection).normalized * knockback);
        AmmoUsed();
        GameObject text = GameObject.Instantiate(Resources.Load<GameObject>("PopupText"),lastProjectile.transform.position,Quaternion.identity);
        text.GetComponent<TextMeshPro>().text = Random.Range(1,100) == 2 ? "Poutine!" : "PEW!";
        gunShotEvent.Invoke(this);
    }


    public float getStaminaCost()
    {
        return staminaCost;
    }
    
    public void AmmoUsed()
    {
        projectileCount--;
        ammoUsedEvent?.Invoke();
        if (projectileCount <= 0) WeaponEmpty();
    }
    virtual public void WeaponEmpty()
    {
        GameObject text = GameObject.Instantiate(Resources.Load<GameObject>("PopupText"), GameManager.instance.GetActivePlayer().transform.position, Quaternion.identity);
        text.GetComponent<TextMeshPro>().text = weaponName +  " Out of ammo!";
        text.transform.localScale *= 3;
        text.GetComponent<TextMeshPro>().color = Color.red;
        text.GetComponent<TextAnim>().lifeSpan = 4;
        GameManager.instance.GetActivePlayer().RemoveWeapon(this);
        
    }

    public string getWeaponName()
    {
        return weaponName;
    }

    public int getProjectileCount()
    {
        return projectileCount;
    }
    public float getFirerate()
    {
        return fireRate;
    }

    public bool OutOfCooldown()
    {
        if (fireRate == 0)
        {
            return true;
        }
        if (fireRate>0)
        {
            if (cooldown <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

}
