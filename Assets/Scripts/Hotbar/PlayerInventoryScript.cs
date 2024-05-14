using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerInventoryScript : MonoBehaviour
{
    [Header("General")]
    public List<ItemSO.ItemType> inventoryList;
    public int selectedItem;
    public float playerReach;

    [Header("Keys")]
    [SerializeField] KeyCode throwItemKey;
    [SerializeField] KeyCode pickItemKey;

    [Header("Item GameObjects")]
    public GameObject banana;
    public GameObject apple;
    public GameObject cherry;

    private Dictionary<ItemSO.ItemType, GameObject> itemSetActive = new Dictionary<ItemSO.ItemType, GameObject>() { };

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(itemSetActive);
        
        itemSetActive.Add(ItemSO.ItemType.apple, apple);
        itemSetActive.Add(ItemSO.ItemType.banana, banana);
        itemSetActive.Add(ItemSO.ItemType.cherry, cherry);

        NewItemSelected();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && inventoryList.Count > 0){
            selectedItem = 0;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && inventoryList.Count > 1){
            selectedItem = 1;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && inventoryList.Count > 2){
            selectedItem = 2;
            NewItemSelected();
        }

        //Debug.Log(selectedItem);
    }

    private void NewItemSelected(){
        apple.SetActive(false);
        banana.SetActive(false);
        cherry.SetActive(false);

        GameObject selectedItemGameObject = itemSetActive[inventoryList[selectedItem]];
        selectedItemGameObject.SetActive(true);
    }

    /*
    private void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Pickup")){
            itemSetActive.Add(ItemSO.ItemType.apple, apple);
        }
        Debug.Log(itemSetActive);
    }
    */
}
