using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemSO itemData;

    void Start()
    {
        if (itemData != null)
        {
            InitializeItem();
        }
    }

    void InitializeItem()
    {
        
        // Set up the item using data from itemData
        // For example, you can set the sprite or model based on itemData
        //Debug.Log("Item initialized: " + itemData.itemName);
    }
}
