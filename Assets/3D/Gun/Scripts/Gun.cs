using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public string gunName;

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
	[SerializeField]
	private int maxAmmoInMag = 30;
	public int ammoInMag;
	[SerializeField]
	public int maxSpareAmmo;
	public int spareAmmo;

	#endregion

	void Awake()
    {
		Init();
	}

    void Init()
    {
		muzzle = transform.Find("Muzzle").gameObject;
		ammoInMag = maxAmmoInMag;
		spareAmmo = maxSpareAmmo;
		originPos = transform.localPosition;

		player = FindAnyObjectByType<PlayerController>();
		cam = player.theCamera;
		audioSource = GetComponent<AudioSource>();

		gunSoundPool = FindAnyObjectByType<GunSoundPool>();
		// gunUI = FindAnyObjectByType<GunUIController>() as GunUIController;
		gunUI = GetComponent<GunUIController>();
		moneyManager = FindObjectOfType<MoneyManager>();

		hits = new RaycastHit[SO_Gun.penetrationCnt];
	}


	private void OnDisable()
	{
        StopReload();
	}

	private void Update()
    {
        Inputs();
    }

    // �Է�
    private void Inputs()
    {
        // �߻�
        // Ű Ŭ�� + ���� �� �ƴ� + źâ�� �Ѿ� �ϳ��� ���� + ������ ���� �ƴ�
		if (SO_Gun.isAutomatic ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1"))
		{
			bool canFire = !isReloading && ammoInMag >= 1 && !isFireDelaying;
            if (canFire)
            { 
			    Fire();
			}
		}

        // ������
        // Ű Ŭ�� + ���� �� �ƴ� + �ִ� ź�� �ƴ� + ���� ź�� �ϳ��� ��������
        if (Input.GetKeyDown(reloadKey))
        {
            bool canReload = !isReloading && ammoInMag <= maxAmmoInMag && spareAmmo >= 1;
            if (canReload)
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
        gunSoundPool.sound = SO_Gun.fireSound;
        gunSoundPool.Pop();

		// ī�޶�κ��� ���� �߻�
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));


		int hitCount = Physics.RaycastNonAlloc(ray, hits, 10000f);
		int enemyHitCount = 0;

		if (hitCount > 0)
		{
			foreach(var hit in hits)
			{
				if (hit.transform.CompareTag("Enemy") == false)
				{
					print("Not Enemy");
					break;
				} else if (enemyHitCount <= SO_Gun.penetrationCnt)
				{
					print("enemy hit!");
					Enemy hitEnemy = hit.collider.GetComponent<Enemy>();
					print(hitEnemy.gameObject);
					hitEnemy.GetDamage(SO_Gun.gunDamage - SO_Gun.penetrateDamagePenalty * enemyHitCount);
					enemyHitCount += 1;
				} else
				{
					// enemyHitCount > SO_Gun.penetrationCnt
					print(enemyHitCount + " : " + SO_Gun.penetrationCnt);
					break;
				}
			}
		}

    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        if (hit.transform.tag == "Enemy") // ���� �¾��� ��
    //        {
    //            Enemy enemy = hit.collider.GetComponent<Enemy>();
    //            if (enemy != null)
    //            {
    //                moneyManager.Coin += mainGun.hitGold;
				//	enemy.GetDamage(mainGun.gunDamage);
                    
				//} else
    //            {
					
				//}
    //        }
    //    }



        foreach(var p in fireParticles)
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
