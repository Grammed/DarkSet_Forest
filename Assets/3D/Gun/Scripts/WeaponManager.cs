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
	private GameObject primaryWeapon;

	public GameObject secondaryLocation; // secondaryWeapon의 부모
	//public SO_Gun secondarySO;
	private GameObject secondaryWeapon;

	[SerializeField] TextMeshProUGUI ammoText;
	[SerializeField] TextMeshProUGUI weaponName;
	[SerializeField] Image _bulletImage;
	public static Image BulletImage { get; set; }
	[SerializeField] Image _circleImage;
	public static Image CircleImage { get; set; }

	[SerializeField]
	private GunSoundPool soundPool;

	enum WeaponType
	{
		Primary,
		Secondary,
	}

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


		if (primaryLocation != null)
		{
			if (primaryLocation.transform.childCount == 1)
			{
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
		float wheelInput = Input.GetAxis("Mouse ScrollWheel");

		bool toPrimary = Input.GetKeyDown(primaryKey) || wheelInput > 0;
		bool toSecondary = Input.GetKeyDown(secondaryKey) || wheelInput < 0;

		if (toPrimary && primaryLocation != null) // 주무기 교체
		{
			SwapTo(WeaponType.Primary);
		}

		if (toSecondary && secondaryLocation != null) // 보조무기 교체
		{
			SwapTo(WeaponType.Secondary);
		}
	}

	void SwapTo(WeaponType type)
	{
		switch(type)
		{
			case WeaponType.Primary:
				if (primaryLocation && primaryLocation.transform.childCount > 0)
				{
					secondaryLocation.SetActive(false);
					primaryLocation.SetActive(true);
					weaponName.text = primaryWeapon.GetComponent<Gun>().SO_Gun.gunName;
					
				}
				break;
			case WeaponType.Secondary:
				if (secondaryLocation.transform.childCount > 0)
				{
					secondaryLocation.SetActive(true);
					primaryLocation.SetActive(false);
					weaponName.text = secondaryWeapon.GetComponent<Gun>().SO_Gun.gunName;
					
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
		// secondaryWeapon = newGun;
		InitGun(primaryWeapon, soGun);
		
		if (secondaryLocation.activeSelf == true)
		{
			primaryLocation.SetActive(false);
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
		foreach (Transform t in secondaryLocation.transform)
		{
			Destroy(t.gameObject);
		}
		secondaryWeapon = Instantiate(newGun, location);
		// secondaryWeapon = newGun;
		InitGun(secondaryWeapon, soGun);

		if (primaryLocation.activeSelf == true)
		{
			secondaryLocation.SetActive(false);
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
		
		if (gunScript.SO_Gun == null)
		{
			Debug.LogError("SO_Gun is not found!");
		}

		gunScript.hits = new RaycastHit[gunScript.SO_Gun.penetrationCnt];
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
