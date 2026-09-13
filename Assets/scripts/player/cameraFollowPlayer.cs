using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollowPlayer : MonoBehaviour
{
    private GameObject player;
    private float offsetX = 1f;
    private float offsetY = 1f;
    private float offsetZ = -3f;

    private float rotationX = 5f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = player.transform.position + new Vector3(offsetX, offsetY, offsetZ);
        transform.rotation = Quaternion.Euler(rotationX, 0, 0);
    }
}
