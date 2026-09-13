using System.Collections;
using System.Collections.Generic;
using TMPro.SpriteAssetUtilities;
using UnityEngine;

public class basicMove : MonoBehaviour
{
    private Rigidbody rb;
    private float spd = 3f;
    private float jumpForce = 3f;

    //jump mechanic
    private bool isGrounded;
    [SerializeField] private float fallMultiplier;
    //dash mechanic
    [SerializeField] private float dashSpd = 10f;
    private float dashCooldownTime = 2f;
    private bool isCooldown_dash = false;
    private float dashDuration = 0.2f;
    private bool isDashing;
    //mouse rotation





    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //jump mechanic
        isGrounded = true;
        fallMultiplier = 4f;

        Cursor.lockState = CursorLockMode.Locked;
    }   
    

    void FixedUpdate()
    {
        Vector3 direction = Vector3.zero;

        //player rotation
        float mouseY = Input.GetAxis("Mouse X");
        transform.Rotate(new Vector3(0, mouseY, 0));
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.y, 0));

        if(Input.GetKey(KeyCode.W)) direction += Vector3.forward;
        if(Input.GetKey(KeyCode.S)) direction += Vector3.back;
        if(Input.GetKey(KeyCode.A)) direction += Vector3.left;
        if(Input.GetKey(KeyCode.D)) direction += Vector3.right;

        if (direction != Vector3.zero) direction = direction.normalized;
        
        if (!isDashing)
        rb.velocity = new Vector3(direction.x * spd, rb.velocity.y, direction.z * spd);

        //exit lockstate
        if (Input.GetKeyDown(KeyCode.Escape)) Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        //when jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) 
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //when falling down
        if (rb.velocity.y < 0)
        rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

        //when dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isCooldown_dash)
        {
            StartCoroutine(dashing());
            StartCoroutine(dashCooldown());
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Debug.Log("Grounded");
            isGrounded = true;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Debug.Log("Exited Ground");
            isGrounded = false;
        }
    }

    //dashing
    IEnumerator dashing()
    {
        isDashing = true;

        rb.AddForce(Vector3.forward * dashSpd, ForceMode.Impulse);
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z);
        isCooldown_dash = true;
        Debug.Log("dash cooldown");

        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }
    //dash cooldown
    IEnumerator dashCooldown()
    {
        yield return new WaitForSeconds(dashCooldownTime);
        isCooldown_dash = false;
        Debug.Log("dash cooldown ended");
    }

}
