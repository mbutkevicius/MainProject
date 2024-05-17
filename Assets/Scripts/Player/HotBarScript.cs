using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HotBarScript : MonoBehaviour
{
    public GameObject slotHolderUI;
    public GameObject[] slotsUI;
    public int hotbarSize = 5;
    public int currentSlot = 0;
    public Item[] hotbarSlots;

    public bool inItemRange = false;
    public bool pickupButtonPressed = false;
    // Start is called before the first frame update
    void Start()
    {
        slotsUI = new GameObject[slotHolderUI.transform.childCount];
        
        for (int i = 0; i < slotHolderUI.transform.childCount; i++){
            slotsUI[i] = slotHolderUI.transform.GetChild(i).gameObject;
        }
        //RefreshUI();
        
        hotbarSlots = new Item[hotbarSize];
    }

    // Update is called once per frame
    void Update()
    {
        if (UserInput.instance.controls.Hotbar.Pickup.WasPressedThisFrame()  && inItemRange){
            pickupButtonPressed = true;
        }
        else if (UserInput.instance.controls.Hotbar.ScrollRight.WasPressedThisFrame()){
            ScrollRight();
        }
        else if (UserInput.instance.controls.Hotbar.ScrollLeft.WasPressedThisFrame()){
            ScrollLeft();
        }
        else if (UserInput.instance.controls.Hotbar.Drop.WasPressedThisFrame()){
            DropItem(currentSlot);
        }
    }

    private void OnTriggerStay2D(Collider2D other){
        if (other.CompareTag("Pickup")){
            inItemRange = true;
        }

        if (pickupButtonPressed && other.CompareTag("Pickup")){ //&& hotbarSlots[currentSlot] == null){
            pickupButtonPressed = false;

            Item item = other.GetComponent<Item>();
            Debug.Log("Item name" + item.name);

            int nextSlotIndex = NextHotbarSlotIndex();

            // add to empty hotbar slot
            if ((nextSlotIndex != currentSlot) || (nextSlotIndex == currentSlot && hotbarSlots[currentSlot] == null)){
                AddToHotbar(item, nextSlotIndex);

                other.transform.parent.gameObject.SetActive(false);
            }
            // swap item for item in current slot
            else {
                SwapHotbarItem(item, nextSlotIndex);
                other.transform.parent.gameObject.SetActive(false);
                //DropItem(nextSlotIndex);
            }
        }
        //Debug.Log(hotbarSlots[currentSlot]);
    }

    private void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Pickup")){
            inItemRange = false;
        }    
    }

    public void RefreshUI(){
        //Debug.Log(hotbarSlots[0].GetComponent<SpriteRenderer>().sprite);
        for (int i = 0; i < slotsUI.Length; i++){
            if (hotbarSlots[i] != null){
                slotsUI[i].transform.GetChild(0).GetComponent<Image>().sprite = hotbarSlots[i].GetComponent<SpriteRenderer>().sprite;
                //Debug.Log("Are we getting an image: " + hotbarSlots[i].GetComponent<SpriteRenderer>().sprite);
            }
            // hot bar slot is null
            else {
                slotsUI[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
            }
            slotsUI[i].transform.GetComponent<Image>().color = Color.blue;
        }
        slotsUI[currentSlot].transform.GetComponent<Image>().color = Color.green;
    }

    public int NextHotbarSlotIndex(){
        for (int i = 0; i < hotbarSlots.Length; i++){
            if (hotbarSlots[i] == null){
                return i;
            }
        }

        return currentSlot;
    }

    public void AddToHotbar(Item item, int slotIndex){
        Debug.Log("Current slot :" + currentSlot);
        if (slotIndex >= 0 && slotIndex < hotbarSize){
            Debug.Log("Hotbar item: " + item.name);
            Debug.Log("Current slot :" + currentSlot);
            hotbarSlots[slotIndex] = item;
        }

        RefreshUI();
    }

    public void SwapHotbarItem(Item item, int slotIndex){
        DropItem(slotIndex);
        AddToHotbar(item, slotIndex);
    }

    private void DropItem(int slotIndex){
        if (hotbarSlots[slotIndex] != null){
            hotbarSlots[slotIndex].transform.parent.gameObject.SetActive(true);
            hotbarSlots[slotIndex] = null;
        }
        RefreshUI();
    }

    private void ScrollRight(){
        if (currentSlot == hotbarSlots.Length - 1){
            currentSlot = 0;
        }
        else{
            currentSlot++;
        }
        Debug.Log("currentSlot: " + currentSlot);
        RefreshUI();
    }

    private void ScrollLeft(){
        if (currentSlot == 0){
            currentSlot = hotbarSlots.Length - 1;
        }
        else{
            currentSlot--;
        }
        Debug.Log("currentSlot: " + currentSlot);
        RefreshUI();
    }
}
