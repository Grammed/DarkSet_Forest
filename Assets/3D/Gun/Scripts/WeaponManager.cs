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
	[SerializeField] Image _bulletImage;
	public static Image BulletImage { get; set; }
	[SerializeField] Image _circleImage;
	public static Image CircleImage { get; set; }

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

		if (primaryWeapon)
		{
			InitGun(primaryWeapon);
			primaryWeapon.SetActive(true);
			secondaryWeapon.SetActive(false);

			weaponName.text = primaryWeapon.name;
			primaryWeapon.GetComponent<GunUIController>().ammoText = this.ammoText;
		}
		if (secondaryWeapon)
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
			SwapTo(WeaponType.Primary);
		}

		if (toSecondary && secondaryWeapon != null) // �������� ��ü
		{
			SwapTo(WeaponType.Secondary);
		}
	}

	void SwapTo(WeaponType type)
	{
		switch(type)
		{
			case WeaponType.Primary:
				weaponName.text = primaryWeapon.GetComponent<Gun>().gunName;
				secondaryWeapon.SetActive(false);
				primaryWeapon.SetActive(true);
				break;
			case WeaponType.Secondary:
				weaponName.text = secondaryWeapon.GetComponent<Gun>().gunName;
				secondaryWeapon.SetActive(true);
				primaryWeapon.SetActive(false);
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
	}

	
	 
	// �� �Ŵ������� �� ����
	//[SerializeField]
	//private List<GameObject> primaryWeaponPrefabs;
	//[SerializeField]
	//private List<GameObject> secondaryWeaponPrefabs;
	 
	 

	// [SerializeField] bool reverseWheel = false;
	

	
}
