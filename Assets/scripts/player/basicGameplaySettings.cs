using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicGameplaySettings : MonoBehaviour
{
    public float CameraMoveSensitivity;
    
    void Start()
    {
        CameraMoveSensitivity = 2f;
    }
}
