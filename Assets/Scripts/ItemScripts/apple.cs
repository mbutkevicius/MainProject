using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class apple : MonoBehaviour, IThrowable
{
    public ItemSO itemData;
    public Sprite sprite;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("damage: " + itemData.damage);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Throw(bool facingRight)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        // throw right if facing right
        float force = facingRight ? 1f: -1f;
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(Vector2.right * force * 10f, ForceMode2D.Impulse);
        }
    }

    public void Drop(){
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        {
            rb.isKinematic = false;
        }
    }
}