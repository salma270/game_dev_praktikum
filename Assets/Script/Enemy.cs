using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float walkSpeed = 2.5f;
    private bool facingLeft;
    public Transform detectPoint;
    public float distance = .5f;
    public LayerMask detectLayer;
    void Start()
    {
        facingLeft = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector2(-1f, 0f) * Time.deltaTime * walkSpeed);

        RaycastHit2D hitInfo = Physics2D.Raycast(detectPoint.position, new Vector2(0f, -1f), distance, detectLayer);

        if (hitInfo == false){
            if (facingLeft == true){
                transform.eulerAngles = new Vector3(0f, -180f, 0f);
                facingLeft = false;
            }
            else{
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                facingLeft = true;
            }
            Debug.Log("Flip Enemy");
        }
        //Physics2D.Raycast();
    }

    private void OnDrawGizmosSelected()
    {
        if (detectPoint == null){
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(detectPoint.position, Vector2.down * distance);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            if (player != null)
            {
                Vector2 hitDirection = collision.transform.position - transform.position;
                player.TakeDamage(1, hitDirection);
            }
        }
    }


}

