using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Configuration Parameters

    //movement
    [Header("Movement")]
    [SerializeField] float speed = 1f;
    [SerializeField] BoxCollider2D patrolBoundry;

    //cache
    Rigidbody2D rb;
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Move()
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PatrolBoundry"))
        {
            SwitchDirection();
        }
    }

    void SwitchDirection()
    {
        speed = -speed;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
