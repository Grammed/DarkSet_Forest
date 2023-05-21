using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bracken_Item : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject.Find("Player").GetComponent<PickUP>().PickupItem(1);
        Destroy(gameObject);
        
    }
}
