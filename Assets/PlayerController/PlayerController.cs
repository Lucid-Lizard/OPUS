using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public bool onGround;
    public int PlayerDir;

    private Rigidbody2D rb;
    private Animator anim;

    public Camera MainCamera;
    public GameObject Player;

    private  float horizontal;
    public bool hit;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        PlayerDir = 1;
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
        float jump = Input.GetAxisRaw("Jump");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        Debug.Log(Input.GetMouseButton(0));
        hit = Input.GetMouseButton(0);

        if (horizontal > 0)
        {
            transform.localScale = new Vector3(10, 10, 1); 
            PlayerDir = 1;
        }
        else if (horizontal < 0)
        {
            transform.localScale = new Vector3(-10, 10, 1); 
            PlayerDir = -1;
        }

        if (vertical > 0.1f || jump > 0.1f)
        {
            if (onGround)
                movement.y = jumpForce;
        }

        rb.velocity = movement;
    }
        
    private void Update()
    {
        anim.SetFloat("horizontal", horizontal);
        anim.SetBool("hit", hit);
    }
}
