using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShoot : MonoBehaviour
{
    public float speed = 20f;
    private float knockback = 1f;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.gravityScale = 0;
        rb.velocity = transform.right * speed;

        // Destroy bullet after 3 seconds.
        Destroy(gameObject,3f);
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision detected:"+collision.collider.name);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Debug.Log("Collision finished:" + collision.collider.name);
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected:" + collision.name);
    }
    */
}
