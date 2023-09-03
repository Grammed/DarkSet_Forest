using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Item : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		GameObject.Find("Player").GetComponent<PickUP>().PickupItem(2);
		Destroy(gameObject);
	}
}
