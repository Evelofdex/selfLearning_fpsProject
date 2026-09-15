using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollowPlayer : MonoBehaviour
{
    private GameObject player;
    private float offsetX = 1f;
    private float offsetY = 1f;
    private float offsetZ = -3f;

    private float offsetRotationX = 5f;

    [SerializeField] private float playerOrbitRotate;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerOrbitRotate = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float mouseRotationX = Input.GetAxis("Mouse X");

        transform.position = player.transform.position + new Vector3(offsetX, offsetY, offsetZ);
        transform.rotation = Quaternion.Euler(offsetRotationX, 0, 0);
        playerOrbitRotate += mouseRotationX;
        transform.RotateAround(player.transform.position, Vector3.up, playerOrbitRotate);
    }
}
