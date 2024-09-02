using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class Gun : MonoBehaviour
{
  public string gunName;
	private bool initComplete = false;

	[Header("References")]
  [HideInInspector]
  public PlayerController player;
  public Camera cam;
  [SerializeField]
  private GunUIController gunUI;
	private GameObject muzzle;
	[SerializeField]
  private MoneyManager moneyManager;
  public SO_Gun SO_Gun;

	[Header("Fire")]
	private bool isFireDelaying = false;
	public bool fireLock = false;
  [SerializeField]
  List<ParticleSystem> fireParticles;


	public RaycastHit[] hits;
	
	#region Sound

	[Header("Sound")]
	public GunSoundPool gunSoundPool;
  private AudioSource audioSource;

	#endregion

	#region Reload

	[Header("Reload")]
	private bool isReloading = false;
	private KeyCode reloadKey = KeyCode.R;

	#endregion

	#region Ammo

	[Header("Ammo")]
	public int maxAmmoInMag = 30;
	public int ammoInMag;
	
	[SerializeField]
	public int maxSpareAmmo;
	public int spareAmmo;

	#endregion
	
	private void Start()
	{
		Init();
	}

  void Init()
	{
		gameObject.name = SO_Gun.gunName;

		muzzle = transform.Find("Muzzle").gameObject;
		ammoInMag = SO_Gun.maxAmmoInMag;
		spareAmmo = SO_Gun.maxSpareAmmo;
		originPos = transform.localPosition;

		player = FindAnyObjectByType<PlayerController>();


		cam = player.theCamera;
		audioSource = GetComponent<AudioSource>();

		gunSoundPool = FindObjectOfType<GunSoundPool>();
		// gunUI = FindAnyObjectByType<GunUIController>() as GunUIController;
		gunUI = GetComponent<GunUIController>();
		moneyManager = FindObjectOfType<MoneyManager>();

		

		hits = new RaycastHit[100];
		print(nameof(SO_Gun.penetrationCnt) + ": " + SO_Gun.penetrationCnt);
		initComplete = true;
	}


	private void OnDisable()
	{
		if (initComplete)
		{ 
			StopReload();
		}
	}

		 
	private void Update()
  {
    Inputs();
		//Ray ray = new Ray(player.transform.position, player.transform.forward);
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow, 0.1f, true);
	}
	
	private bool CanReload()
	{
		// 키 클릭 + 장전 중 아님 + 최대 탄수 아님 + 여분 탄수 하나라도 남아있음
		bool isSpareAmmoEnough = spareAmmo >= 1;
		bool isCurrAmmoNotFull = ammoInMag < maxAmmoInMag || (ammoInMag == maxAmmoInMag && SO_Gun.isClosedBolt);
		
		return isSpareAmmoEnough && !isCurrAmmoNotFull && !isReloading;
	}


	// 입력
	private void Inputs()
  {
    // 발사
    // 키 클릭 + 장전 중 아님 + 탄창에 총알 하나라도 있음 + 딜레이 중이 아님
		if (SO_Gun.isAutomatic ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0))
		{
			bool isAmmoInMag = ammoInMag >= 1;
			bool canFire = !isReloading && isAmmoInMag && !isFireDelaying;
			if (canFire && PlayerController.canFire)
			{
				Fire();
				if (ammoInMag == 0)
				{
					StartCoroutine(Reload());
				}
			}
			// else if (!isAmmoInMag && !isReloading)
			//{
			//	StartCoroutine(Reload());
			//}
		}

    // 재장전
    if (Input.GetKeyDown(reloadKey) && CanReload())
    {
      StartCoroutine(Reload());
      print("can reload");
    }
	}

  // 총 발사
  private void Fire()
  {
    print("Fire");
    //audioSource.clip = fireSound;
    //audioSource.Play();
    isFireDelaying = true; // 코루틴 완료 전까지 발사 불가
    ammoInMag -= 1;

    gunUI.ChangeAmmoText($"{ammoInMag}/{spareAmmo}");

    // 오브젝트 풀에서 사운드 꺼냄
    GameObject soundGO = gunSoundPool.Pop();
		AudioSource source = soundGO.GetComponent<AudioSource>();
		source.clip = SO_Gun.fireSound;
		source.Play();

		DrawBulletRay();


		// 발사 파티클 재생 (불꽃, 연기)
		foreach (var p in fireParticles)
    {
        p.Play();
    }

		// 발사 딜레이
		StartCoroutine(FireDelay()); 

    // 반동
    Recoil();
	}
	
	private void DrawBulletRay()
	{
		// 카메라로부터 레이 발사
		//Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			// 적 컴포넌트
			Enemy enemy;
			
			// 다른 물체(장애물 등)가 아닌 적이 맞음
			if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Head"))
			{
				enemy = hit.collider.GetComponentInParent<Enemy>();
				if (enemy != null)
				{
					// 적이 존재하면 골드 벌기
					// 플레이어가 사용할 수 있는 골드
					moneyManager.Coin += SO_Gun.hitGold;
					
					// 이번 게임에서 얻은 총 골드
					GameManager.Instance.earnGold += SO_Gun.hitGold;
				}
			}
			
			if (hit.collider.tag == "Enemy") // 적에 맞았을 때
			{
				enemy.GetDamage(SO_Gun.gunDamage);
			}
			else if (hit.collider.tag == "Head") // 헤드샷
			{
					enemy.GetDamage(SO_Gun.gunDamage * 2f);
					print("headshot");
				
			}
		}
	}

  private Vector3 originPos; // 총 자체의 원래 위치
  private Vector3 changePos; // 바뀐 위치
  void Recoil()
  {
    // 반동
    print("Recoil");
		// 상하반동
		player.currentCameraRotationX -= SO_Gun.recoilY;

		// 카메라 회전 변경 //
		Vector3 rot = player.transform.eulerAngles;
    player._yRotation += Random.Range(-SO_Gun.recoilX, SO_Gun.recoilX);
    player.transform.eulerAngles = rot; 

    // 총 자체 밀리는 반동
    changePos = transform.localPosition + (Vector3.back * SO_Gun.recoilZ);
    transform.localPosition = changePos;
    StopCoroutine(ReboundRecoil());
    StartCoroutine(ReboundRecoil());
  }

    // 반동 회복
  IEnumerator ReboundRecoil() 
	{
    // 앞뒤 반동 회복
		while (transform.localPosition != originPos)
    {
			transform.localPosition = Vector3.Lerp(transform.localPosition, originPos, 0.1f * (SO_Gun.ergonomic / 100f));
			yield return null;
		}

	}

	// 장전 딜레이
	IEnumerator FireDelay()
  {
    yield return new WaitForSeconds(SO_Gun.fireTime);
    isFireDelaying = false;
    StopCoroutine(FireDelay());
  }

	// 재장전
	IEnumerator Reload()
  { 
		// Update 메서드에서 참조됨
		print("Start Reloading");

		// 재장전 관련 이미지가 null임
    if (WeaponManager.BulletImage == null || WeaponManager.CircleImage == null)
    {
        Debug.Log("Reload failed since bulletimage or circleimage was not found");
        yield break;
    }
    
    // 재장전 관련 이미지들을 보이게 하고
    WeaponManager.BulletImage.gameObject.SetActive(true);
		WeaponManager.CircleImage.gameObject.SetActive(true);

		// 재장전 퍼센티지 보이는 코루틴 재시작
		// 실행되고 있는 코루틴 종료
    StopCoroutine(WeaponManager.FillCircle(SO_Gun.reloadTime));
    StartCoroutine(WeaponManager.FillCircle(SO_Gun.reloadTime));

		// 장전 사운드 준비 및 플레이
		isReloading = true;
    audioSource.clip = SO_Gun.reloadSound;
    audioSource.Play();
    
    yield return new WaitForSeconds(SO_Gun.reloadTime);
    
    // 총알 수 조절
    if (SO_Gun.isClosedBolt && ammoInMag >= 1) // 약실 장전(약실에 총알 있으면 장전된 총알이 한 발 더 늘어남)
    {
			spareAmmo += ammoInMag - 1;
      ammoInMag = 1;
			ammoInMag += Mathf.Min(spareAmmo, maxAmmoInMag);
			spareAmmo -= ammoInMag - 1;
		} else // 오픈볼트 장전
    {
			spareAmmo += ammoInMag;
			ammoInMag = Mathf.Min(spareAmmo, maxAmmoInMag);
			spareAmmo -= ammoInMag;
		}

    // 장전 끝
    StopReload();
    yield break;
  }

  void StopReload()
  {
	  // 장전 중지
		isFireDelaying = false;
		isReloading = false;
		print("Stop reload\n" + ammoInMag + " / " + spareAmmo);
		gunUI.ChangeAmmoText($"{ammoInMag}/{spareAmmo}");
	
		// 장전 관련 이미지 비활성화
		if (WeaponManager.BulletImage)
		{
			WeaponManager.BulletImage.gameObject.SetActive(false);
		}
	
		if (WeaponManager.CircleImage)
		{
			WeaponManager.CircleImage.gameObject.SetActive(false);
		}
			
	
		// 총알 채우는 코루틴 중지
		StopCoroutine(Reload());
	}
}
