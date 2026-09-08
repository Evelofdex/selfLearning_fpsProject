using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class basicMove : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float spd;

    //skills
    [SerializeField] private float jumpForce;
    private bool isGrounded;

    private float dashEnd;
    private float dash;
    private bool isCooldown_dash;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 5f;
        spd = 10f;
        jumpForce = 10f;
        dashEnd = 2f;
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
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isCooldown_dash)
        {
            t_dash += Time.deltaTime/1f;
            dash = Mathf.SmoothStep(dashEnd, 0f, t_dash);
            Debug.Log("is Dashing: " + dash);
            StartCoroutine(dashCooldown());
            if (dash <= dashEnd)
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
        Debug.Log("grounded");
    }
    
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) 
        isGrounded = false;
    }

    IEnumerator dashCooldown()
    {
        isCooldown_dash = true;
        Debug.Log("stopped dashing");
        yield return new WaitForSeconds(3f);
        isCooldown_dash = false;
    }
}
