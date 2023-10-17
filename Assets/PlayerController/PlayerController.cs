using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public bool onGround;

    public float horizontal;
    private SpriteRenderer SpriteRenderer;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Ground"))
            onGround = true;
    }
   
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Ground"))
            onGround = false;
    }
    private void FixedUpdate()
    {
        //do stuff
        horizontal = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
    }
        
}
