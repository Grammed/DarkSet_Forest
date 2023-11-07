using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShowShopUI : MonoBehaviour
{
	[SerializeField] GameObject shopCanvas;
	[SerializeField] GameObject shopManager;
	[SerializeField] GameObject interactPopup;

	[HideInInspector]
	private bool canShowShop;

	[HideInInspector]
	private bool isShopOpened;

	[SerializeField] KeyCode openShopKey = KeyCode.E;
	[SerializeField] KeyCode closeShopKey = KeyCode.Escape;

	private void Awake()
	{
		canShowShop = false;
		isShopOpened = false;
	}

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
		if (!isShopOpened && canShowShop && Input.GetKeyDown(openShopKey))
		{
			ShowShop();
		}

		else if (isShopOpened && (Input.GetKeyDown(openShopKey) || Input.GetKeyDown(closeShopKey)))
		{
			CloseShop();
		}
	}

	void ShowShop()
	{
		print("Shop opened");
		// show shop
		isShopOpened = true;
		shopCanvas.SetActive(true);
		shopManager.SetActive(true);

		PlayerController.CamRotateEnable = false;
		PlayerController.canFire = false;

		Cursor.lockState = CursorLockMode.None;
	}

	void CloseShop()
	{
		print("shop closed");

		isShopOpened = false;
		shopCanvas.SetActive(false);
		shopManager.SetActive(false);

		PlayerController.CamRotateEnable = true;
		PlayerController.canFire = true;

		Cursor.lockState = CursorLockMode.Locked;
	}

	private void OnTriggerExit(Collider other)
	{
		canShowShop = false;
		interactPopup.SetActive(false);
		CloseShop();
	}
}
