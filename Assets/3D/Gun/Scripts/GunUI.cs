using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GunUI : MonoBehaviour
{
	// ź �� Ȯ���ϴ� TMPUGUI
	// null�� �� �ڵ����� �ʱ�ȭ
	[SerializeField]
	private TextMeshProUGUI ammoText;

	private void Start()
	{
		// ammoText �ڵ� �ʱ�ȭ
		if (ammoText == null)
		{
			ammoText = transform.Find("AmmoText").GetComponent<TextMeshProUGUI>();
		}
		ChangeAmmoText("NaN/NaN");
	}

	/// <summary> ammoText(ź �� Ȯ���ϴ� �ؽ�Ʈ)�� text�� ���� </summary>
	/// <param name="text"></param>
	public void ChangeAmmoText(string text)
	{
		ammoText.text = text;
	}

	/// <summary> Gun UI�� ����� </summary>
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

	/// <summary> Gun UI�� ���δ� </summary>
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
}
