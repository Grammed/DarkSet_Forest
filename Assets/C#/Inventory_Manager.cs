using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Manager : MonoBehaviour
{
    public int maxStackedItems = 64;
    public Inventory_Slot[] inventory_Slots;
    public GameObject InventoryItemPrefab;

    int selectedSlot = -1;


    private void Start()
    {
        ChangeSelectedSlot(0);
    }

    private void Update()
    {
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");
        if(Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if(isNumber && number > 0 && number < 10)
            {
                ChangeSelectedSlot(number - 1); ;
            }
        }

    }

    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventory_Slots[selectedSlot].DeSelect();
        }

        inventory_Slots[newValue].Select();
        selectedSlot = newValue;
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventory_Slots.Length; i++)
        {
            Inventory_Slot slot = inventory_Slots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStackedItems && itemInSlot.item.Stackable == true)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        // ÀÎº¥Åä¸® ºóÄ­ Ã£±â
        for (int i = 0; i < inventory_Slots.Length; i++)
        {
            Inventory_Slot slot = inventory_Slots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if( itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }

        return false;
    }

    void SpawnNewItem(Item item, Inventory_Slot slot)
    {
        GameObject newItemGo = Instantiate(InventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    public Item GetSelectedItem(bool use)
    {
        Inventory_Slot slot = inventory_Slots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null)
        {
            Item item = itemInSlot.item;
            if(use == true)
            {
                itemInSlot.count--;
                if(itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }

            return item;
        }

        return null;
    }
}
