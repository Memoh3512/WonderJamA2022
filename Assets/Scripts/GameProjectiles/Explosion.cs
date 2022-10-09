using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Explosion : MonoBehaviour
{
    int damage;
    float pushForce;
    public float steps = 30;
    private float lifeSpan;
    private float maxRadius;
    private LineRenderer lr;
    public int pointsCount = 100;
    CircleCollider2D cc;

    public void Init(int damage, float radius,float pushForce,float lifeSpan = 0.2f)
    {
        this.lr = GetComponent<LineRenderer>();
        this.lifeSpan = lifeSpan;
        this.damage = damage;
        this.pushForce = pushForce;
        cc = GetComponent<CircleCollider2D>();
        maxRadius = radius;
        cc.enabled = true;
        GameObject text = GameObject.Instantiate(Resources.Load<GameObject>("PopupText"), transform.position, Quaternion.identity);
        text.GetComponent<TextMeshPro>().text = "BOOM!";
        text.GetComponent<TextMeshPro>().color = Color.red;
        text.transform.localScale *= 3;
        text.GetComponent<TextAnim>().random = false;
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        lr.positionCount = pointsCount;
        float timePassed = 0;
        float timeStep = lifeSpan / steps;
        float currentRadius = 0;
        while (timePassed < lifeSpan)
        {
            yield return new WaitForSeconds(timeStep);
            timePassed += timeStep;
            Vector3[] points = new Vector3[pointsCount];
            currentRadius = maxRadius * (timePassed / lifeSpan);
            cc.radius = currentRadius;
            for (int i = 0; i < pointsCount; i++)
            {
                float angle = (360f / pointsCount) * i * Mathf.Deg2Rad;
                points[i] = transform.position + (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)))*currentRadius;
            }
            points[points.Length-1] = points[0];
            lr.SetPositions(points);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if(angle < 100 && angle > 80)
            {
                GameManager.instance.Glitch(GlitchType.Player, collision.GetComponent<PlayerControls>());
                collision.gameObject.GetComponent<Rigidbody2D>().velocity += (direction * pushForce*2);
            }
            else
            {
                collision.gameObject.GetComponent<PlayerControls>().TakeDamage(damage);
                collision.gameObject.GetComponent<Rigidbody2D>().velocity += (direction * pushForce);

            }

        }
    }
}
