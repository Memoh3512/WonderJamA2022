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
        text.transform.localScale *= 4;
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerControls>().TakeDamage(damage);
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * pushForce);
        }
    }
}
