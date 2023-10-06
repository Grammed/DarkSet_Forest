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


	// 무기는 활성화 상태여야 함, 주무기와 보조무기 합쳐서 최소 하나 이상
	public GameObject primaryWeapon; // primaryWeapon의 자식, 자식은 하나여야 함
	public GameObject secondaryWeapon; // secondaryWeapon의 자식, 자식은 하나여야 함
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

	
	 
	// 샵 매니저에서 쓸 예정
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
			// 무기가 둘 다 없으면
			throw new System.Exception("The player needs to have at least 1 weapon");
		}

		
	}

	private void Update()
	{
		float wheelInput = Input.GetAxis("Mouse ScrollWheel");

		bool toPrimary = Input.GetKeyDown(primaryKey) || wheelInput > 0;
		bool toSecondary = Input.GetKeyDown(secondaryKey) || wheelInput < 0;

		if (toPrimary && primaryWeapon != null) // 주무기 교체
		{
			weaponName.text = primaryWeapon.GetComponent<Gun>().gunName;
			secondaryWeapon.SetActive(false);
			primaryWeapon.SetActive(true);
		}

		if (toSecondary && secondaryWeapon != null) // 보조무기 교체
		{
			weaponName.text = secondaryWeapon.GetComponent<Gun>().gunName;
			secondaryWeapon.SetActive(true);
			primaryWeapon.SetActive(false);
		}
	}
}
