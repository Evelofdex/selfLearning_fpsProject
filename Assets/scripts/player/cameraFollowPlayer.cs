using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollowPlayer : MonoBehaviour
{
    //game settings
    private GameObject gameSettings;
    private basicGameplaySettings camSensitivity;

    private GameObject player;

    private float offsetRotationX = 5f;

    private float distance = 5f;

    // Start is called before the first frame update
    void Start()
    {
        //game settings
        gameSettings = GameObject.FindGameObjectWithTag("gameSettings");
        camSensitivity = gameSettings.GetComponent<basicGameplaySettings>();

        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float mouseRotationX = Input.GetAxis("Mouse X") * camSensitivity.CameraMoveSensitivity;

        transform.RotateAround(player.transform.position, Vector3.up, mouseRotationX);
        
        Vector3 directionFromPlayer = (transform.position - player.transform.position).normalized;
        transform.position = player.transform.position + directionFromPlayer * distance;
        
        transform.LookAt(player.transform.position);
    }
}
