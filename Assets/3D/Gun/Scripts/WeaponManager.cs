using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> primaryWeaponPrefabs;
	[SerializeField]
	private List<GameObject> secondaryWeaponPrefabs;


	// ����� Ȱ��ȭ ���¿��� ��, �ֹ���� �������� ���ļ� �ּ� �ϳ� �̻�
	public GameObject primaryWeapon; // primaryWeapon�� �ڽ�, �ڽ��� �ϳ����� ��
	public GameObject secondaryWeapon; // secondaryWeapon�� �ڽ�, �ڽ��� �ϳ����� ��
	[SerializeField] TextMeshProUGUI ammoText;
	[SerializeField] TextMeshProUGUI weaponName;
	[SerializeField] Image bulletImage;
	[SerializeField] Image circleImage;

	public void ChangePrimary(GameObject newGun)
	{
		Transform location = primaryWeapon.transform;
		foreach(Transform t in primaryWeapon.transform.parent)
		{
			Destroy(t.gameObject);
		}
		primaryWeapon = Instantiate(newGun, location.parent);
		// secondaryWeapon = newGun;
		InitGun(secondaryWeapon);
	}

	public void ChangePrimary(int gunIdx)
	{
		Transform location = primaryWeapon.transform;
		foreach (Transform t in primaryWeapon.transform.parent)
		{
			Destroy(t.gameObject);
		}
		primaryWeapon = Instantiate(primaryWeaponPrefabs[gunIdx], location.parent);
		// secondaryWeapon = newGun;
		InitGun(secondaryWeapon);
	}

	public void ChangeSecondary(GameObject newGun)
	{
		Transform location = secondaryWeapon.transform;
		foreach (Transform t in secondaryWeapon.transform.parent)
		{
			Destroy(t.gameObject);
		}
		secondaryWeapon = Instantiate(newGun, location.parent);
		// secondaryWeapon = newGun;
		InitGun(secondaryWeapon);
	}

	public void ChangeSecondary(int gunIdx)
	{
		Transform location = secondaryWeapon.transform;
		foreach (Transform t in secondaryWeapon.transform.parent)
		{
			Destroy(t.gameObject);
		}
		secondaryWeapon = Instantiate(secondaryWeaponPrefabs[gunIdx], location.parent);
		// secondaryWeapon = newGun;
		InitGun(secondaryWeapon);
	}

	private void InitGun(GameObject gunGO)
	{
		print("initialize");
		gunGO.GetComponent<GunUIController>().ammoText = this.ammoText;
		

		Gun gunComponent = gunGO.GetComponent<Gun>();
		gunComponent.bulletImage = this.bulletImage;
		gunComponent.circleImage = this.circleImage;
	}

	
	 
	// �� �Ŵ������� �� ����
	//[SerializeField]
	//private List<GameObject> primaryWeaponPrefabs;
	//[SerializeField]
	//private List<GameObject> secondaryWeaponPrefabs;
	 
	 

	// [SerializeField] bool reverseWheel = false;
	KeyCode primaryKey = KeyCode.Alpha1;
	KeyCode secondaryKey = KeyCode.Alpha2;

	private void Awake()
	{
		if (primaryWeapon)
		{
			InitGun(primaryWeapon);
			primaryWeapon.SetActive(true);
			secondaryWeapon.SetActive(false);

			weaponName.text = primaryWeapon.name;
			primaryWeapon.GetComponent<GunUIController>().ammoText = this.ammoText;
		} if (secondaryWeapon)
		{
			InitGun(secondaryWeapon);
			if (!primaryWeapon)
			{
				secondaryWeapon.SetActive(true);
			}
			secondaryWeapon.GetComponent<GunUIController>().ammoText = this.ammoText;
		} 

		if (!primaryWeapon && !secondaryWeapon)
		{
			// ���Ⱑ �� �� ������
			throw new System.Exception("The player needs to have at least 1 weapon");
		}

		
	}

	private void Update()
	{
		float wheelInput = Input.GetAxis("Mouse ScrollWheel");

		bool toPrimary = Input.GetKeyDown(primaryKey) || wheelInput > 0;
		bool toSecondary = Input.GetKeyDown(secondaryKey) || wheelInput < 0;

		if (toPrimary && primaryWeapon != null) // �ֹ��� ��ü
		{
			weaponName.text = primaryWeapon.GetComponent<Gun>().gunName;
			secondaryWeapon.SetActive(false);
			primaryWeapon.SetActive(true);
		}

		if (toSecondary && secondaryWeapon != null) // �������� ��ü
		{
			weaponName.text = secondaryWeapon.GetComponent<Gun>().gunName;
			secondaryWeapon.SetActive(true);
			primaryWeapon.SetActive(false);
		}
	}
}
