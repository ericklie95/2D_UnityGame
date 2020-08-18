using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayerMask;

    public float moveSpeed = 5f;
    public float jumpForce = 1f;

    private float movementInput = 0f;
    private Rigidbody2D rb;
    private BoxCollider2D boxColl;

    public bool onGround = true;
    public bool touchEnemy = false;

    public float explosionForce = .1f;
    public float radius = 5f;

    public float startDebuff = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // So character won't be rotated
        rb.gravityScale = 2; // Gravity value that pulls down character

        boxColl = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Move();

        // Counting down the debuff time.
        if(startDebuff > 0)
        {
            startDebuff -= Time.deltaTime;
            if(startDebuff <= 0)
            {
                // Debuff finishes. Going back to initial value.
                rb.gravityScale = 2;
            }
        }
    }

    void Move()
    {
        movementInput = Input.GetAxis("Horizontal");

        // rb.velocity.y because it is possible to jump and move at the same time.
        rb.velocity = new Vector2(movementInput * moveSpeed, rb.velocity.y);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") || Input.GetMouseButton(0))
        {
            rb.velocity = Vector2.up * jumpForce;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision" + collision.collider.tag);
        // Find collision based on tag.
        if (collision.collider.tag == "Ground")
        {
            onGround = true;
        }
        
        // Find collision based on layer
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //destroyEnemy();
            hitByEnemy();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            onGround = false;
        }
    }

    void destroyEnemy()
    {
        // Get all colliders surrounding enemies.
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, radius, 1 << LayerMask.NameToLayer("Enemy"));
        
        foreach (Collider2D enemy in enemies)
        {
            // Get direction of where the enemy is going to be hit to
            Vector2 direction = enemy.transform.position - transform.position;
            enemy.GetComponent<Rigidbody2D>().AddForce(direction * explosionForce);
            Destroy(enemy.gameObject, 2); // Destroy the enemy after 2 seconds.
        }
    }

    void hitByEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, radius, 1 << LayerMask.NameToLayer("Enemy"));
        foreach (Collider2D enemy in enemies)
        {
            if(enemy.gameObject.tag == "Ice")
            {
                rb.gravityScale = 5;
                startDebuff = 3;
            }
        }
    }
    
}
