using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood_Item : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject.Find("Player").GetComponent<PickUP>().PickupItem(0);
        Destroy(gameObject);
    }
}
