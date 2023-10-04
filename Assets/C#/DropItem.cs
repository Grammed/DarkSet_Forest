using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public Inventory_Manager inventory_Manager;
    public GameObject[] itemsToDrop;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Droped(1);
        }
    }

    public void Droped(int id)
    {
        inventory_Manager.Drop(itemsToDrop[id]);
    }
}
