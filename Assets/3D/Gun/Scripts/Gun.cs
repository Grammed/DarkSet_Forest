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
	/// <summary> ������ �� �߻簡 ������ �� false </summary>
	private bool isFireDelaying = false;
	public bool fireLock = false;
    [SerializeField]
    List<ParticleSystem> fireParticles;


	public RaycastHit[] hits;


	#region Recoil

	//[Header("Recoil")]
	//[SerializeField]
	//[Tooltip("�¿� �ݵ�")]
	///// <summary> �¿� �ݵ� </summary>
	//private float recoilX = 1;
	//[SerializeField]
	//[Tooltip("���� �ݵ�")]
	///// <summary> ���� �ݵ� </summary>
	//private float recoilY = 1; // ���� �ݵ�
 //   [Tooltip("�� ��ü �յ� �ݵ�")]
	///// <summary> �� ��ü �յ� �ݵ� </summary>
	//[SerializeField]
 //   private float recoilZ = 1; // �յ� �ݵ�

	//[SerializeField]
	//[Tooltip("��ü����, �������� �ݵ� ȸ���� ����")]
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
	//private bool isClosedBolt = true; // Ŭ����� ��Ʈ

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


	// �Է�
	private void Inputs()
    {
        // �߻�
        // Ű Ŭ�� + ���� �� �ƴ� + źâ�� �Ѿ� �ϳ��� ���� + ������ ���� �ƴ�
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

        // ������
        // Ű Ŭ�� + ���� �� �ƴ� + �ִ� ź�� �ƴ� + ���� ź�� �ϳ��� ��������
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

    // �� �߻�
    private void Fire()
    {
        print("Fire");
        //audioSource.clip = fireSound;
        //audioSource.Play();
        isFireDelaying = true; // �ڷ�ƾ �Ϸ� ������ �߻� �Ұ�
        ammoInMag -= 1;

        gunUI.ChangeAmmoText($"{ammoInMag}/{spareAmmo}");

        // ������Ʈ Ǯ���� ���� ����
        GameObject soundGO = gunSoundPool.Pop();
		AudioSource source = soundGO.GetComponent<AudioSource>();
		source.clip = SO_Gun.fireSound;
		source.Play();

		// ī�޶�κ��� ���� �߻�
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
			if (hit.collider.tag == "Enemy") // ���� �¾��� ��
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

		// �߻� ������
		StartCoroutine(FireDelay()); 

        // �ݵ�
        Recoil();
	}

    private Vector3 originPos; // �� ��ü�� ���� ��ġ
    private Vector3 changePos; // �ٲ� ��ġ
    void Recoil()
    {
        // �ݵ�
        print("Recoil");
		// ���Ϲݵ�
		player.currentCameraRotationX -= SO_Gun.recoilY;

		// ī�޶� ȸ�� ���� //
		Vector3 rot = player.transform.eulerAngles;
        player._yRotation += Random.Range(-SO_Gun.recoilX, SO_Gun.recoilX);
        player.transform.eulerAngles = rot; 

        // �� ��ü �и��� �ݵ�
        changePos = transform.localPosition + (Vector3.back * SO_Gun.recoilZ);
        transform.localPosition = changePos;
        StopCoroutine(ReboundRecoil());
        StartCoroutine(ReboundRecoil());
    }

    // �ݵ� ȸ��
    IEnumerator ReboundRecoil() 
	{
        // �յ� �ݵ� ȸ��
		while (transform.localPosition != originPos)
        {
			transform.localPosition = Vector3.Lerp(transform.localPosition, originPos, 0.1f * (SO_Gun.ergonomic / 100f));
			yield return null;
		}

	}

	// ī�޶� ȸ�� ����
	IEnumerator FireDelay()
    {
        yield return new WaitForSeconds(SO_Gun.fireTime);
        isFireDelaying = false;
        StopCoroutine(FireDelay());
    }

	// ������
	IEnumerator Reload()
    { 
		// Update �޼��忡�� ������
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

		// ���� ���� �غ� �� �÷���
		isReloading = true;
        audioSource.clip = SO_Gun.reloadSound;
        audioSource.Play();
        
        yield return new WaitForSeconds(SO_Gun.reloadTime);
        
        // �Ѿ� �� ����
        if (SO_Gun.isClosedBolt && ammoInMag >= 1) // ��� ����
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

        // ���� ��
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
