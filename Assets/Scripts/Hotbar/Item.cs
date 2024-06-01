using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
Item contains methods that all items will inherit. Originally, I tried inheriting
ItemSO with apple and combining vars and methods but that caused an error.
I also need general Item class as a mediary for the hotbar so it knows which icons
to display for individual items. I'm not entirely sure why it wasn't working the way
I originally planned or why I have to assign a reference to ItemSO but it is functioning
if I assign it as reference here.
*/


public abstract class Item : MonoBehaviour
{
    // reference to SO containing variable data
    public ItemSO itemData;

    // abstract methods to call in HotBarScript.cs for specific items
    public abstract void Throw(bool direction);
    public abstract void Drop();

    // TODO: Implement life fruit
    /*
    public abstract void ApplyPassiveEffect(PlayerScript player);
    public abstract void UseItem(PlayerScript player);
    public abstract void RemovePassiveEffect(PlayerScript player);
    */
}
