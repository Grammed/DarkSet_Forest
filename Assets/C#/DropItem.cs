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
            // 아이템 드롭
            // Droped(1);
        }
    }

    public void Droped(int id)
    {
        // Drop 구현 안됨
        inventory_Manager.Drop(itemsToDrop[id]);
    }
}
