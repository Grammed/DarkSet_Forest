using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShowShopUI : MonoBehaviour
{
	[SerializeField] GameObject shopCanvas;
	[SerializeField] GameObject shopManager;
	[SerializeField] GameObject interactPopup;

	private bool canShowShop = false;
	[SerializeField] KeyCode openShopKey = KeyCode.E;
	[SerializeField] KeyCode closeShopKey = KeyCode.Escape;


	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			canShowShop = true;
			interactPopup.SetActive(true);
		}
	}

	void Update()
	{
		if (canShowShop && Input.GetKeyDown(openShopKey))
		{
			ShowShop();
		}

		if (Input.GetKeyDown(closeShopKey))
		{
			CloseShop();
		}
	}

	void ShowShop()
	{
		// show shop
		shopCanvas.SetActive(true);
		shopManager.SetActive(true);

		Cursor.lockState = CursorLockMode.None;
	}

	void CloseShop()
	{
		shopCanvas.SetActive(false);
		shopManager.SetActive(false);

		Cursor.lockState = CursorLockMode.Locked;
	}

	private void OnTriggerExit(Collider other)
	{
		canShowShop = false;
		interactPopup.SetActive(false);
		CloseShop();
	}
}
