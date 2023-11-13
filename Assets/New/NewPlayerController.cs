using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerController : MonoBehaviour
{
    public int moveSpeed;
    public int jumpForce;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();   
    }

    public bool onGround;
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
            onGround = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
            onGround = false;
    }

    

    private void FixedUpdate()
    {
        //do stuff
        float horizontal = Input.GetAxis("Horizontal");
        float jump = Input.GetAxisRaw("Jump");
        float vertical = Input.GetAxisRaw("Vertical");
        

        Vector2 movement = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        //Debug.Log(Input.GetMouseButton(0));
        
        
        

        if (vertical > 0.1f || jump > 0.1f)
        {
            if (onGround)
                movement.y = jumpForce;
        }

        rb.velocity = movement;
    }
}
