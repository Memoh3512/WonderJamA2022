using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Explosion : MonoBehaviour
{
    int damage;
    float pushForce;

    public void Init(int damage, float radius,float pushForce)
    {
        this.damage = damage;
        this.pushForce = pushForce;
        CircleCollider2D cc = GetComponent<CircleCollider2D>();
        cc.radius = radius;
        cc.enabled = true;
        GameObject text = GameObject.Instantiate(Resources.Load<GameObject>("PopupText"), transform.position, Quaternion.identity);
        text.GetComponent<TextMeshPro>().text = "BOOM!";
        text.GetComponent<TextMeshPro>().color = Color.red;
        text.transform.localScale *= 4;
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(0.2f);
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
