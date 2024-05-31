using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UI;

public class HotBarScript : MonoBehaviour
{
    public Transform playerHand;
    public GameObject slotHolderUI;
    public GameObject[] slotsUI;
    public int hotbarSize = 5;
    public int currentSlot = 0;
    public GameObject[] physicalItems;
    public Item[] hotbarSlots;
    public float pickupRadius = 1f;

    public bool inItemRange = false;
    public bool pickupButtonPressed = false;

    public GameObject itemPrefab;
    private Dictionary<GameObject, Vector3> itemPositions = new Dictionary<GameObject, Vector3>();

    private GameObject itemInHand;
    private bool isHoldingItem = false;
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
        if (UserInput.instance.controls.Hotbar.Pickup.WasPressedThisFrame() && !isHoldingItem){  //&& inItemRange){
            CheckForPickup();
        }
        else if (UserInput.instance.controls.Hotbar.Pickup.WasPressedThisFrame() && isHoldingItem){
            bool containsNull = hotbarSlots.Any(item => item == null);
            if (containsNull){
                int slot = NextHotbarSlotIndex();
                AddToHotbar(itemInHand.GetComponent<Item>(), slot);
                isHoldingItem = false;
                itemInHand.SetActive(false);
                itemInHand = null;
            }
        }
        else if (UserInput.instance.controls.Hotbar.Throw.WasPressedThisFrame() && itemInHand != null){
            IThrowable throwableItem = itemInHand.GetComponent<IThrowable>();
            
            if (throwableItem != null){
                itemInHand.transform.SetParent(null);

                PlayerScript player = GetComponentInParent<PlayerScript>();
                throwableItem.Throw(player.facingRight);
                
                isHoldingItem = false;
                itemInHand = null;
            }            
        }
        else if (UserInput.instance.controls.Hotbar.ScrollRight.WasPressedThisFrame()){
            ScrollRight();
        }
        else if (UserInput.instance.controls.Hotbar.ScrollLeft.WasPressedThisFrame()){
            ScrollLeft();
        }
        else if (UserInput.instance.controls.Hotbar.Drop.WasPressedThisFrame()){
            if (itemInHand != null){
                IThrowable DroppableItem = itemInHand.GetComponent<IThrowable>();

                itemInHand.transform.SetParent(null);
                DroppableItem.Drop();
                isHoldingItem = false;
                itemInHand = null;
            }
            //RemoveItemFromHotbar(currentSlot);
            //hotbarSlots[currentSlot] = null;
        }

        else if (UserInput.instance.controls.Hotbar.Use.WasPressedThisFrame() && !isHoldingItem){
            // Get the child GameObject by index
            if (hotbarSlots[currentSlot] != null){
                GameObject childObject = transform.GetChild(currentSlot).gameObject;
                // Activate the child GameObject    
                childObject.SetActive(true);
                itemInHand = childObject;
                isHoldingItem = true;
                RemoveItemFromHotbar(currentSlot);
            }
        }

        // TODO: Swap indices so that it works more than once. So essentially, always make item in hand the 5th index
        else if (UserInput.instance.controls.Hotbar.Use.WasPressedThisFrame() && isHoldingItem){
            // LINQ check to see if null value exists
            bool containsNull = hotbarSlots.Any(item => item == null);
            if (!containsNull && isHoldingItem){
                GameObject itemToSwap = transform.GetChild(currentSlot).gameObject;
                // get the items index
                //int index = itemInHand.transform.GetSiblingIndex();
                //Debug.Log("Index: " + index);

                // add item in hand to slot
                RemoveItemFromHotbar(currentSlot);
                itemInHand.SetActive(false);
                AddToHotbar(itemInHand.GetComponent<Item>(), currentSlot);

                itemInHand.transform.SetSiblingIndex(currentSlot);

                itemInHand = itemToSwap;
                itemInHand.transform.SetSiblingIndex(hotbarSize);
                itemInHand.SetActive(true);
                //itemInHand.transform.SetParent(null);   
            }
        }
    }

/*
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
                //RemoveItemFromHotbar(nextSlotIndex);
            }
        }
        //Debug.Log(hotbarSlots[currentSlot]);
    }


    private void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Pickup")){
            inItemRange = false;
        }    
    }
*/

// old version
/*
    private void CheckForPickup()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, pickupRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Pickup"))
            {
                Item item = hitCollider.GetComponent<Item>();
                int slot = NextHotbarSlotIndex();
                AddToHotbar(item, slot);
                break;
            }
        }
    }
*/

    private void CheckForPickup()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, pickupRadius);


        // LINQ to get closest collider to player
        var closestCollider = hitColliders
            .Where(hitCollider => hitCollider.CompareTag("Pickup"))
            .Select(hitCollider => new { Collider = hitCollider, Item = hitCollider.GetComponent<GameObject>() })
            .OrderBy(hit => Vector2.Distance(transform.position, hit.Collider.transform.position))
            .Select(hit => hit.Collider)
            .FirstOrDefault();

        Debug.Log("closestcolliders: " + closestCollider);

        if (closestCollider != null)
        {
            Item item = closestCollider.GetComponent<Item>();
            GameObject itemGO = closestCollider.gameObject;
            if (item != null) {
                int slot = NextHotbarSlotIndex();
                //AddToHotbar(item, slot);
                PickUpItem(itemGO);
                isHoldingItem = true;        
            }
        }
    }

    public void PickUpItem(GameObject item)
    {
        item.transform.SetParent(playerHand);
        item.transform.localPosition = Vector3.zero; // Adjust as needed
        item.transform.localRotation = Quaternion.identity; // Reset rotation
        //item.SetActive(false); // Deactivate the item
        
        Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
        if (rb != null){
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
            rb.isKinematic = true; // Make sure the item doesn't react to physics
            Debug.Log("Is Kinematic: " + rb.isKinematic);
        }

        itemInHand = rb.gameObject;
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
            if (hotbarSlots[currentSlot] == null){
                return currentSlot;
            }

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

        Debug.Log(item);
        //item.transform.gameObject.SetActive(false);

        RefreshUI();
    }

    public void SwapHotbarItem(Item item, int slotIndex){
        RemoveItemFromHotbar(slotIndex);
        AddToHotbar(item, slotIndex);
    }

    private void RemoveItemFromHotbar(int slotIndex){
        if (hotbarSlots[slotIndex] != null){
            //hotbarSlots[slotIndex].transform.parent.gameObject.SetActive(true);
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
