using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    public Transform player;  // The player's transform
    public Vector3 offset;    // Offset between the player and camera

    void Start()
    {
        // Calculate the initial offset
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // Set the position of the camera to the player's position plus the offset
        transform.position = player.position + offset;
    }
}
