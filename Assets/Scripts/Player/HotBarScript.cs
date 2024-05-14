using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotBarScript : MonoBehaviour
{
    public GameObject slotHolderUI;
    public GameObject[] slotsUI;
    public int hotbarSize = 5;
    public int currentSlot = 0;
    public Item[] hotbarSlots;
    // Start is called before the first frame update
    void Start()
    {
        slotsUI = new GameObject[slotHolderUI.transform.childCount];
        
        for (int i = 0; i < slotHolderUI.transform.childCount; i++){
            slotsUI[i] = slotHolderUI.transform.GetChild(i).gameObject;
        }
        //RefreshUI();
        
        
        // might not need the slotsUI and if so I'll use this one
        hotbarSlots = new Item[hotbarSize];
    }

    public void RefreshUI(){
        Debug.Log(hotbarSlots[0].GetComponent<SpriteRenderer>().sprite);
        for (int i = 0; i < slotsUI.Length; i++){
            if (hotbarSlots[i] != null){
                slotsUI[i].transform.GetChild(0).GetComponent<Image>().sprite = hotbarSlots[i].GetComponent<SpriteRenderer>().sprite;
                Debug.Log("Are we getting an image: " + hotbarSlots[i].GetComponent<SpriteRenderer>().sprite);
            }
        }
    }

    public void AddToHotbar(Item item, int slotIndex){
        Debug.Log("Current slot :" + currentSlot);
        if (slotIndex >= 0 && slotIndex < hotbarSize){
            Debug.Log("Hotbar item: " + item.name);
            Debug.Log("Current slot :" + currentSlot);
            hotbarSlots[slotIndex] = item;
        }

        RefreshUI();

        if (currentSlot != 4){
            currentSlot++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //DropItem();
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Pickup") && hotbarSlots[currentSlot] == null){
            Item item = other.GetComponent<Item>();
            Debug.Log("Item name" + item.name);

            if (hotbarSlots[currentSlot] == null){
                AddToHotbar(item, currentSlot);

                other.transform.parent.gameObject.SetActive(false);
            }
        }
        Debug.Log(hotbarSlots[currentSlot]);
    }

    private void DropItem(){
        if (hotbarSlots[currentSlot] != null){
            //hotbarSlots[currentSlot].gameObject.SetActive(true);
            //hotbarSlots[currentSlot] = null;
        }
    }
}
