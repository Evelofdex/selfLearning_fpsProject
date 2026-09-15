using System.Collections;
using System.Collections.Generic;
using TMPro.SpriteAssetUtilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class basicMove : MonoBehaviour
{
    //ref other script
    private GameObject gameSettings;
    private basicGameplaySettings cameraSensitivity;

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
    //run mechanic
    private float runMultiplier;





    void Start()
    {
        //game settings
        gameSettings = GameObject.FindGameObjectWithTag("gameSettings");
        cameraSensitivity = gameSettings.GetComponent<basicGameplaySettings>();

        rb = GetComponent<Rigidbody>();
        //jump mechanic
        isGrounded = true;
        fallMultiplier = 4f;

        Cursor.lockState = CursorLockMode.Locked;

        runMultiplier = 1f;
    }   
    

    Vector3 rotateDirection = Vector3.zero; 
    void FixedUpdate()
    {
        Vector3 direction = Vector3.zero;


        //player rotation
        float mouseY = Input.GetAxis("Mouse X");
        rotateDirection.y += mouseY * cameraSensitivity.CameraMoveSensitivity;
        transform.rotation = Quaternion.Euler(rotateDirection);

        //basic movement
        if(Input.GetKey(KeyCode.W)) direction += transform.forward;
        if(Input.GetKey(KeyCode.S)) direction += transform.forward * -1;
        if(Input.GetKey(KeyCode.A)) direction += transform.right * -1;
        if(Input.GetKey(KeyCode.D)) direction += transform.right;
        //run
        if(Input.GetKey(KeyCode.LeftShift)) runMultiplier = 2f;
        else runMultiplier = 1f;

        if (direction != Vector3.zero) direction = direction.normalized;
        
        if (!isDashing)
        rb.velocity = new Vector3(direction.x * (spd * runMultiplier), rb.velocity.y, direction.z * spd);

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
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isCooldown_dash)
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

        rb.AddForce(transform.forward * dashSpd, ForceMode.Impulse);
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
