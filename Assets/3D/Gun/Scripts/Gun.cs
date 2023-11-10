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
	//[SerializeField]
	//private float gunDamage = 25;
	//[SerializeField]
 //   private float fireTime = 0.2f;
 //   [SerializeField]
 //   private bool isAutomatic;
	/// <summary> 딜레이 후 발사가 가능할 때 false </summary>
	private bool isFireDelaying = false;
	public bool fireLock = false;
    [SerializeField]
    List<ParticleSystem> fireParticles;


	public RaycastHit[] hits;


	#region Recoil

	//[Header("Recoil")]
	//[SerializeField]
	//[Tooltip("좌우 반동")]
	///// <summary> 좌우 반동 </summary>
	//private float recoilX = 1;
	//[SerializeField]
	//[Tooltip("상하 반동")]
	///// <summary> 상하 반동 </summary>
	//private float recoilY = 1; // 상하 반동
 //   [Tooltip("총 자체 앞뒤 반동")]
	///// <summary> 총 자체 앞뒤 반동 </summary>
	//[SerializeField]
 //   private float recoilZ = 1; // 앞뒤 반동

	//[SerializeField]
	//[Tooltip("인체공학, 높을수록 반동 회복이 빠름")]
 //   private float ergonomic = 70;

    
	#endregion
	
	#region Sound

	[Header("Sound")]
	public GunSoundPool gunSoundPool;
	//[SerializeField]
 //   private AudioClip reloadSound;
 //   [SerializeField]
 //   private AudioClip fireSound;
    private AudioSource audioSource;

	#endregion

	#region Reload

	[Header("Reload")]
	private bool isReloading = false;
	private KeyCode reloadKey = KeyCode.R;
 //   [SerializeField]
 //   private float reloadTime = 3f;
	//[SerializeField]
	//private bool isClosedBolt = true; // 클로즈드 볼트

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
        // 키 클릭 + 장전 중 아님 + 최대 탄수 아님 + 여분 탄수 하나라도 남아있음
        if (Input.GetKeyDown(reloadKey))
        {
            bool canReload = !isReloading && spareAmmo >= 1;
			bool ammoNotEnough = ammoInMag < maxAmmoInMag || (ammoInMag == maxAmmoInMag && SO_Gun.isClosedBolt);

			if (canReload && ammoNotEnough)
            {
                StartCoroutine(Reload());
                print("can reload");
            }
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

		// 카메라로부터 레이 발사
		//Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		//int hitCount = Physics.RaycastNonAlloc(ray, hits, 1000f);


		//Array.Sort(hits, (RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance));
		//int enemyHitCount = 0;

		//for (int i = 0; i < Mathf.Min(hitCount, SO_Gun.penetrationCnt); i++)
		//{
		//	RaycastHit _hit = hits[i];
		//	if (_hit.transform == null) continue;
		//	if (_hit.transform.CompareTag("Enemy"))
		//	{
		//		print("enemy hit! " + i);
		//	}
		//}

		//if (hitCount > 0)
		//{
		//	for (int i = 0; i < Mathf.Min(hitCount, SO_Gun.penetrationCnt); i++)
		//	{
		//		RaycastHit hit = hits[i];
		//		if (hit.collider == null) continue;
		//		if (hit.transform.CompareTag("Enemy"))
		//		{
		//			print("Enemy hit!");
		//			Enemy hitEnemy = hit.collider.GetComponent<Enemy>();
		//			float actualDamage = SO_Gun.gunDamage - SO_Gun.penetrateDamagePenalty * enemyHitCount;
		//			if (hitEnemy != null)
		//			{
		//				moneyManager.Coin += SO_Gun.hitGold;
		//				hitEnemy.GetDamage(actualDamage);
		//			}

		//			else print("actual enemy not found");

		//			enemyHitCount += 1;
		//		} else
		//		{
		//			break;
		//			// continue;
		//		}
		//	}
		//}

		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			if (hit.collider.tag == "Enemy") // 적에 맞았을 때
			{
				Enemy enemy = hit.collider.GetComponentInParent<Enemy>();
				if (enemy != null)
				{
					moneyManager.Coin += SO_Gun.hitGold;
					GameManager.Instance.earnGold += SO_Gun.hitGold;
					enemy.GetDamage(SO_Gun.gunDamage);

				} else
				{

				}
			}
			else if (hit.collider.tag == "Head")
			{
				Enemy enemy = hit.collider.GetComponentInParent<Enemy>();
				if (enemy != null)
				{
					moneyManager.Coin += SO_Gun.hitGold;
					GameManager.Instance.earnGold += SO_Gun.hitGold;
					enemy.GetDamage(SO_Gun.gunDamage * 2f);
					print("headshot");
				}
			}
		}



		foreach (var p in fireParticles)
        {
            p.Play();
        }

		// 발사 딜레이
		StartCoroutine(FireDelay()); 

        // 반동
        Recoil();
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

	// 카메라 회전 변경
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


        if (WeaponManager.BulletImage == null || WeaponManager.CircleImage == null)
        {
            Debug.Log("Reload failed since bulletimage or circleimage was not found");
            yield break;
        }
        WeaponManager.BulletImage.gameObject.SetActive(true);
		WeaponManager.CircleImage.gameObject.SetActive(true);

        StopCoroutine(WeaponManager.FillCircle(SO_Gun.reloadTime));
        StartCoroutine(WeaponManager.FillCircle(SO_Gun.reloadTime));

		// 장전 사운드 준비 및 플레이
		isReloading = true;
        audioSource.clip = SO_Gun.reloadSound;
        audioSource.Play();
        
        yield return new WaitForSeconds(SO_Gun.reloadTime);
        
        // 총알 수 조절
        if (SO_Gun.isClosedBolt && ammoInMag >= 1) // 약실 장전
        {
			spareAmmo += ammoInMag - 1;
            ammoInMag = 1;
			ammoInMag += Mathf.Min(spareAmmo, maxAmmoInMag);
			spareAmmo -= ammoInMag - 1;
		} else
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
		isFireDelaying = false;
		isReloading = false;
		print("Stop reload\n" + ammoInMag + " / " + spareAmmo);
		gunUI.ChangeAmmoText($"{ammoInMag}/{spareAmmo}");

		if (WeaponManager.BulletImage)
		{
			WeaponManager.BulletImage.gameObject.SetActive(false);
		}

		if (WeaponManager.CircleImage)
		{
			WeaponManager.CircleImage.gameObject.SetActive(false);
		}
		


		StopCoroutine(Reload());
	}
}
