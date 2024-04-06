using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePadScript : MonoBehaviour
{
    public BoxCollider2D platformCollider;

    void Start()
    {
        if (platformCollider == null)
        {
            Debug.LogError("Platform Collider is missing!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        // Disable collision with player when entering from below
        if (other.CompareTag("Player"))
        {
            Debug.Log("Touched sprite");
            //Debug.Log(other.GetComponent<PlayerScript>().isJumping);
            other.GetComponent<PlayerScript>().isJumping = false;
            other.GetComponent<PlayerScript>().bouncing = true;
            other.GetComponent<PlayerScript>().BounceJump();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Enable collision with player when leaving the platform
        if (other.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(other, platformCollider, false);
            other.GetComponent<PlayerScript>().bouncing = false;
        }
    }
}
