using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http.Headers;
using UnityEditor.Rendering;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayerMask;

    public float moveSpeed = 5f;
    public float jumpForce = 1f;

    public static Player instance;

    private float movementInput = 0f;
    private Rigidbody2D rb;
    private BoxCollider2D boxColl;
    public bool FacingRight = true;

    public bool onGround = true;
    public bool touchEnemy = false;

    public float radius = 5f;

    public float knockback=5f;
    public float knockbackLength=5/10f;
    public float knockbackCount=0;
    public bool knockFromRight = true;

    public float startDebuff = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // So character won't be rotated
        rb.gravityScale = 2; // Gravity value that pulls down character

        boxColl = GetComponent<BoxCollider2D>();
        instance = this;
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
        // Player can only move when they are not on knockbacked.
        if(knockbackCount <= 0)
        {         
            movementInput = Input.GetAxis("Horizontal");

            // If (player moves to the left and facing right) OR (moves to the right and facing left) then flip
            if((movementInput<0 & FacingRight) | (movementInput>0 & !FacingRight) )
            {
                this.Flip();
            }
            // rb.velocity.y because it is possible to jump and move at the same time.
            rb.velocity = new Vector2(movementInput * moveSpeed, rb.velocity.y);
        } else // When the user is knockbacked.
        {
            if (knockFromRight)
            {
                rb.velocity = new Vector2(-knockback, knockback/2);
            } else
            {
                rb.velocity = new Vector2(knockback, knockback/2);
            }
            knockbackCount -= Time.deltaTime;
        }
    }

    /* Function used to flip character when he is facing to the left or right
     */
    void Flip()
    {
        FacingRight = !FacingRight;
        this.transform.Rotate(0f, 180f, 0f);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    /*
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
    */

    /* Destroy Enemy only happens when the player shoots enemies.
     * In this case, the bullet must be used to detect collision.
     * 
     */
    void destroyEnemy()
    {
        // Raycast(current position, the line to make from current position);
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right);

        // If we hitting something
        if (hitInfo)
        {
            // Check if we are hitting enemy
            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                Vector2 direction = enemy.transform.position - transform.position;
                enemy.GetComponent<Rigidbody2D>().AddForce(direction * knockback);
                Destroy(enemy.gameObject, 2); // Destroy the enemy after 2 seconds.
            }
            // Else check if we are hitting platform
        }
        
    }

    /* This function is used when the enemy hits player and
     * will be called from Enemy script.
     * Param: enemyTag with type of string to check what enemy is colliding with player.
     */
    public void hitByEnemy(string enemyTag)
    {
        if (enemyTag == "Ice")
        {
            rb.gravityScale = 7;
            startDebuff = 3;
            Debug.Log("Enemy Tag is Ice. Start debuff on player");
        }

    }
}
