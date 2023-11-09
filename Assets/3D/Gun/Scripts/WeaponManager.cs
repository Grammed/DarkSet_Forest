using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
	//[SerializeField]
	//private List<GameObject> primaryWeaponPrefabs;
	//[SerializeField]
	//private List<GameObject> secondaryWeaponPrefabs;


	// 무기는 활성화 상태여야 함, 주무기와 보조무기 합쳐서 최소 하나 이상
	public GameObject primaryLocation; // primaryWeapon의 부모
	//public SO_Gun primarySO;
	public GameObject primaryWeapon;
	public Gun primaryGunScript;

	public GameObject secondaryLocation; // secondaryWeapon의 부모
	//public SO_Gun secondarySO;
	public GameObject secondaryWeapon;
	public Gun secondaryGunScript;

	[SerializeField] TextMeshProUGUI ammoText;
	[SerializeField] TextMeshProUGUI weaponNameText;
	[SerializeField] Image _bulletImage;
	public static Image BulletImage { get; set; }
	[SerializeField] Image _circleImage;
	public static Image CircleImage { get; set; }

	[SerializeField]
	private GunSoundPool soundPool;

	public bool canSwap = true;

	enum WeaponType
	{
		Primary,
		Secondary,
	}

	WeaponType nowWeapon;
	KeyCode primaryKey = KeyCode.Alpha1;
	KeyCode secondaryKey = KeyCode.Alpha2;


	public static void SetBulletImageActive(bool status) 
	{
		if (BulletImage)
		{
			BulletImage.gameObject.SetActive(status);
		} else
		{
			Debug.Log("bullet image is not found");
		}
	}


	private void Awake()
	{
		BulletImage = _bulletImage;
		CircleImage = _circleImage;

		#region notUsed
		// [ deprecated / 현재 사용 안 함 ]
		//if (primaryLocation != null && primarySO != null)
		//{
		//	primaryWeapon = Instantiate(primarySO.prefab, primaryLocation.transform);
		//	InitGun(primaryWeapon, primarySO);
		//	primaryLocation.SetActive(true);

		//	secondaryLocation.SetActive(false);

		//	weaponName.text = primarySO.gunName;
		//}
		//if (secondaryLocation != null && secondarySO != null)
		//{
		//	secondaryWeapon = Instantiate(secondarySO.prefab, secondaryLocation.transform);
		//	InitGun(secondaryWeapon);
		//	if (!primaryLocation)
		//	{
		//		secondaryLocation.SetActive(true);
		//		weaponName.text = secondarySO.gunName;
		//	}
		//	}
		#endregion

		if (primaryLocation != null)
		{
			if (primaryLocation.transform.childCount == 1)
			{
				nowWeapon = WeaponType.Primary;
				primaryWeapon = primaryLocation.transform.GetChild(0).gameObject;
				InitGun(primaryWeapon);
			} else if (primaryLocation.transform.childCount >= 2)
			{
				Debug.LogError("There's over 1 primary weapon in player!");
			}
		} else
		{
			Debug.LogError("Please set the location of primary weapon.");
		}

		if (secondaryLocation != null)
		{
			if (secondaryLocation.transform.childCount == 1)
			{
				nowWeapon = WeaponType.Secondary;
				secondaryWeapon = secondaryLocation.transform.GetChild(0).gameObject;
				InitGun(secondaryWeapon);
				if (primaryWeapon != null)
				{
					secondaryWeapon.SetActive(false);
				}
			} else if (secondaryLocation.transform.childCount >= 2)
			{
				Debug.LogError("There's over 1 secondary weapon in player!");
			}
		} else
		{
			Debug.LogError("Please set the location of secondary weapon.");
		}

		if (primaryLocation == null && secondaryLocation == null)
		{
			// 무기가 둘 다 없으면
			throw new System.Exception("The player needs to have at least one weapon");
		} 


	}

	private void Update()
	{
		if (canSwap)
		{
			float wheelInput = Input.GetAxis("Mouse ScrollWheel");

			bool toPrimary = Input.GetKeyDown(primaryKey) || wheelInput > 0;
			bool toSecondary = Input.GetKeyDown(secondaryKey) || wheelInput < 0;

			if (toPrimary && primaryWeapon) // 주무기 교체
			{
				SwapTo(WeaponType.Primary);
			}

			if (toSecondary && secondaryWeapon) // 보조무기 교체
			{
				SwapTo(WeaponType.Secondary);
			}
		}
	}

	void SwapTo(WeaponType type)
	{
		nowWeapon = type;
		switch(type)
		{
			case WeaponType.Primary:
				if (secondaryLocation.transform.childCount >= 1)
				{
					secondaryLocation.SetActive(false);
					primaryLocation.SetActive(true);

					InitGun(primaryWeapon);
				}
				break;
			case WeaponType.Secondary:
				if (primaryLocation.transform.childCount >= 1)
				{
					secondaryLocation.SetActive(true);
					primaryLocation.SetActive(false);

					InitGun(secondaryWeapon);
				}
				break;
		}

		StopFillCircle();
	}

	public static IEnumerator FillCircle(float reloadTime)
	{
		CircleImage.fillAmount = 0f;

		float processTime = 0f;

		while (processTime < reloadTime)
		{
			processTime += Time.deltaTime;

			CircleImage.fillAmount = processTime / reloadTime;
			yield return null;
		}
	}

	void StopFillCircle()
	{
		StopCoroutine(nameof(FillCircle));

		if (CircleImage)
		{
			CircleImage.gameObject.SetActive(false);
		}
		if (BulletImage)
		{
			BulletImage.gameObject.SetActive(false);

		}
	}


	public void ChangePrimary(GameObject newGun, SO_Gun soGun)
	{
		//primarySO = soGun;
		Transform location = primaryLocation.transform;
		foreach(Transform t in primaryLocation.transform)
		{
			Destroy(t.gameObject);
		}
		primaryWeapon = Instantiate(newGun, location);
        primaryGunScript = primaryWeapon.GetComponent<Gun>();
        // secondaryWeapon = newGun;
        InitGun(primaryWeapon, soGun);
		
		if (nowWeapon == WeaponType.Secondary)
		{
			SwapTo(WeaponType.Secondary);
		} else
		{
			weaponNameText.text = soGun.gunName;
		}

		
	}

	//public void ChangePrimary(int gunIdx)
	//{
	//	Transform location = primaryWeapon.transform;
	//	foreach (Transform t in primaryWeapon.transform.parent)
	//	{
	//		Destroy(t.gameObject);
	//	}
	//	primaryWeapon = Instantiate(primaryWeaponPrefabs[gunIdx], location.parent);
	//	// secondaryWeapon = newGun;
	//	InitGun(primaryWeapon);
	//}

	public void ChangeSecondary(GameObject newGun, SO_Gun soGun)
	{
		// secondarySO = soGun;
		Transform location = secondaryLocation.transform;
		foreach (Transform t in location)
		{
			Destroy(t.gameObject);
		}
		secondaryWeapon = Instantiate(newGun, location);
        secondaryGunScript = secondaryWeapon.GetComponent<Gun>();
        // secondaryWeapon = newGun;
        InitGun(secondaryWeapon, soGun);

		if (nowWeapon == WeaponType.Primary)
		{
			SwapTo(WeaponType.Primary);
		} else
		{
			weaponNameText.text = soGun.gunName;
		}

		
	}

	//public void ChangeSecondary(int gunIdx)
	//{
	//	Transform location = secondaryWeapon.transform;
	//	foreach (Transform t in secondaryWeapon.transform.parent)
	//	{
	//		Destroy(t.gameObject);
	//	}
	//	secondaryWeapon = Instantiate(secondaryWeaponPrefabs[gunIdx], location.parent);
	//	// secondaryWeapon = newGun;
	//	InitGun(secondaryWeapon);
	//}
	private void InitGun(GameObject gunGO)
	{
		print(gunGO);
		GunUIController gunUI = gunGO.GetComponent<GunUIController>();
		gunUI.ammoText = ammoText;



		Gun gunScript = gunGO.GetComponent<Gun>();
		gunScript.gunSoundPool = soundPool;
		gunGO.name = gunScript.SO_Gun.GunName;

		
		if (gunScript.SO_Gun == null)
		{
			Debug.LogError("SO_Gun is not found!");
		}

		gunScript.hits = new RaycastHit[gunScript.SO_Gun.penetrationCnt];
		weaponNameText.text = gunScript.SO_Gun.gunName;
		// weaponName.text = gunScript.gunName;
	}
	private void InitGun(GameObject gunGO, SO_Gun soGun)
	{
		Gun gunScript = gunGO.GetComponent<Gun>();
		gunScript.SO_Gun = soGun;

		InitGun(gunGO);

		
		// weaponName.text = gunScript.gunName;
	}

	
	 
	// 샵 매니저에서 쓸 예정
	//[SerializeField]
	//private List<GameObject> primaryWeaponPrefabs;
	//[SerializeField]
	//private List<GameObject> secondaryWeaponPrefabs;
	 
	 

	// [SerializeField] bool reverseWheel = false;
	

	
}
