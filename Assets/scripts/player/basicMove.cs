using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class basicMove : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float spd;

    //skills
    private float jumpForce;
    private bool isGrounded;

    private float dashEnd;
    private float dash;
    private bool isCooldown_dash;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spd = 10f;
        jumpForce = 10f;
        dashEnd = 3f;
        dash = 0f;
        isCooldown_dash = false;
        isGrounded = true;
    }

    private float t_dash;
    void FixedUpdate()
    {
        Vector3 direction = Vector3.zero;

        if(Input.GetKey(KeyCode.W)) direction += Vector3.forward;
        if(Input.GetKey(KeyCode.S)) direction += Vector3.back;
        if(Input.GetKey(KeyCode.A)) direction += Vector3.left;
        if(Input.GetKey(KeyCode.D)) direction += Vector3.right;

        //jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
        //dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && isCooldown_dash)
        {
            t_dash += Time.deltaTime/1f;
            dash = Mathf.SmoothStep(1f, dashEnd, t_dash);
            if (dash >= dashEnd)
            {
                dash = 0f;
                StartCoroutine(dashCooldown());
            }
        }

        if(direction != Vector3.zero) direction.Normalize();

        rb.velocity = direction * spd + rb.velocity * dash;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) 
        isGrounded = true;
    }
    
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) 
        isGrounded = false;
    }

    IEnumerator dashCooldown()
    {
        
    }
}
