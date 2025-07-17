using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crow2 : MonoBehaviour
{
    public Transform pointA, pointB;
    public float speed = 2f;
    private Transform target;

    private void Start()
    {
        target = pointB;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            target = target == pointA ? pointB : pointA;
            Flip();
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Ambil arah knockback (dari crow ke player)
            Vector2 knockDirection = collision.transform.position - transform.position;

            // Ambil script PlayerMovement
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage(1, knockDirection);
            }
        }
    }

}

