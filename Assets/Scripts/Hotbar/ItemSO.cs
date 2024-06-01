using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

/*
ItemSO should contain all variable data shared between items. Still tbd
if I want to add in things like like the sprite. See Item.cs for reason
why methods aren't contained here
*/

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class ItemSO : ScriptableObject
{
    [Header("Properties")]
    //public ItemType itemType;
    public int damage;

    //public abstract void Throw(bool direction);


    // may use ItemTypes in future so keeping this here
    /*
    public enum ItemType {
        Throwable,
        Consumable,
    }
    */
}
