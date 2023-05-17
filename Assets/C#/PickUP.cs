using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUP : MonoBehaviour
{
    public Inventory_Manager inventory_Manager;
    public Item[] itemsToPickup;

    public void PickupItem(int id)
    {
        bool result = inventory_Manager.AddItem(itemsToPickup[id]);
        if(result == true)
        {
            Debug.Log("ItemEat");
        }
        if(result == false)
        {
            Debug.Log("NoItemEat");
        }
    }
}
