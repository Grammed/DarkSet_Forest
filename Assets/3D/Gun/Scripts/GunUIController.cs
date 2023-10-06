using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GunUIController : MonoBehaviour
{
	// 탄 수 확인하는 TMPUGUI
	// null일 시 자동으로 초기화
	public TextMeshProUGUI ammoText;
	public Gun gunScript;

	private void Awake()
	{
		gunScript = GetComponent<Gun>();
		// ammoText 자동 초기화
		if (ammoText == null)
		{
			// ammoText = transform.Find("AmmoText").GetComponent<TextMeshProUGUI>();
		}
		
	}

	/// <summary> ammoText(탄 수 확인하는 텍스트)를 text로 변경 </summary>
	/// <param name="text"></param>
	public void ChangeAmmoText(string text)
	{
		ammoText.text = text;
	}

	/// <summary> Gun UI를 숨긴다 </summary>
	public void HideUI()
	{
		if (ammoText.gameObject.activeSelf == false)
		{
			Debug.Log("Gun UI is already hidden!");
		} else
		{
			ammoText.gameObject.SetActive(false);
		}
	}

	/// <summary> Gun UI를 보인다 </summary>
	public void ShowUI()
	{
		if (ammoText.gameObject.activeSelf == true)
		{
			Debug.Log("Gun UI is already shown!");
		} else
		{
			ammoText.gameObject.SetActive(true);
		}
	}

	private void Update()
	{
		ChangeAmmoText($"{gunScript.ammoInMag}/{gunScript.spareAmmo}");
	}
}
