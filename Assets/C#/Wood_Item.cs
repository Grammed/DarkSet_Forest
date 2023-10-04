using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood_Item : MonoBehaviour
{
    public int id;
    private void OnTriggerEnter(Collider other)
    {
        GameObject.Find("Player").GetComponent<PickUP>().PickupItem(id);
        Destroy(gameObject);
        
    }
}
