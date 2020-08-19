using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float knockbackPower = 5f;
    public float knockbackDuration = 3f;

    public Rigidbody2D rb;
    private Player player;

    private void Start()
    {
        rb.gravityScale = 0;
            
    }

    /* Collision Details:
     * When Enemy hit a player : Player will be knockbacked and start any debuff.
     * To apply debuff to player, send the coroutine to the player script.
     */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player") {
            player = Player.instance;

            player.knockbackCount = player.knockbackLength;

            if(collision.transform.position.x < transform.position.x)
            {
                player.knockFromRight = true;
            } else
            {
                player.knockFromRight = false;
            }

            player.hitByEnemy(this.gameObject.tag);
        }

        if(collision.gameObject.tag == "Bullet")
        {
            Debug.Log("Enemy is hit by a bullet!");
            Vector2 direction = collision.gameObject.transform.position - this.transform.position;
            //rb.AddForce(direction * knockback);
            rb.velocity = new Vector2(direction.x*knockbackPower / 2, direction.y*knockbackPower / 2);
            Destroy(collision.gameObject);
            Destroy(gameObject,2);
        }
    }
}
