using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class ItemSO : ScriptableObject
{
    [Header("Properties")]
    public ItemType itemType;
    public Sprite sprite;

    public enum ItemType {
        apple,
        banana,
        cherry,
        Throwable,
        Consumable,
    }
}
