using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShowShopUI : MonoBehaviour
{
	[SerializeField] GameObject shopCanvas;
	[SerializeField] GameObject shopManager;


	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			// show shop
			shopCanvas.SetActive(true);
			shopManager.SetActive(true);

			Cursor.lockState = CursorLockMode.None;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		shopCanvas.SetActive(false);
		shopManager.SetActive(false);

		Cursor.lockState = CursorLockMode.Locked;
	}
}
