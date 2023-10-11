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
	private Shop_Manager shopManager;
    [SerializeField]
    private SO_MainGun mainGun;

	[Header("Fire")]
	//[SerializeField]
	//private float gunDamage = 25;
	//[SerializeField]
 //   private float fireTime = 0.2f;
 //   [SerializeField]
 //   private bool isAutomatic;
	/// <summary> 딜레이 후 발사가 가능할 때 false </summary>
	private bool isFireDelaying = false;
    [SerializeField]
    List<ParticleSystem> fireParticles;


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
	[SerializeField]
	GunSoundPool gunSoundPool;
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
 
    public Image bulletImage;
    public Image circleImage;

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
		#region Init

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

        shopManager = FindAnyObjectByType<Shop_Manager>();

		#endregion

		
	}

	private void OnEnable()
	{
	}

	private void Update()
    {
        Inputs();
    }

    // 입력
    private void Inputs()
    {
        // 발사
        // 키 클릭 + 장전 중 아님 + 탄창에 총알 하나라도 있음 + 딜레이 중이 아님
		if (mainGun.isAutomatic ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1"))
		{
			bool canFire = !isReloading && ammoInMag >= 1 && !isFireDelaying;
            if (canFire)
            { 
			    Fire();
			}
		}

        // 재장전
        // 키 클릭 + 장전 중 아님 + 최대 탄수 아님 + 여분 탄수 하나라도 남아있음
        if (Input.GetKeyDown(reloadKey))
        {
            bool canReload = !isReloading && ammoInMag <= maxAmmoInMag && spareAmmo >= 1;
            if (canReload)
            {
                StartCoroutine(Reload());

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
        gunSoundPool.sound = mainGun.fireSound;
        gunSoundPool.Pop();

		// 카메라로부터 레이 발사
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f)); 
		RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Enemy") // 적에 맞았을 때
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    shopManager.Coin += mainGun.hitGold;
					enemy.GetDamage(mainGun.gunDamage);
                    
				}

                EnemyLegacy legacyEnemy = hit.collider.GetComponent<EnemyLegacy>();
                legacyEnemy.Hit(mainGun.gunDamage);
            }
        }

        foreach(var p in fireParticles)
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
		player.currentCameraRotationX -= mainGun.recoilY;

		// 카메라 회전 변경 //
		Vector3 rot = player.transform.eulerAngles;
        player._yRotation += Random.Range(-mainGun.recoilX, mainGun.recoilX);
        player.transform.eulerAngles = rot; 

        // 총 자체 밀리는 반동
        changePos = transform.localPosition + (Vector3.back * mainGun.recoilZ);
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
			transform.localPosition = Vector3.Lerp(transform.localPosition, originPos, 0.1f * (mainGun.ergonomic / 100f));
			yield return null;
		}

	}

	// 카메라 회전 변경
	IEnumerator FireDelay()
    {
        yield return new WaitForSeconds(mainGun.fireTime);
        isFireDelaying = false;
        StopCoroutine(FireDelay());
    }

	// 재장전
	IEnumerator Reload()
    { 
		// Update 메서드에서 참조됨
		print("Start Reloading");
        bulletImage.gameObject.SetActive(true);
		circleImage.gameObject.SetActive(true);

        StopCoroutine(FillCircle());
		StartCoroutine(FillCircle()); // 원 채우기 시작

		// 장전 사운드 준비 및 플레이
		isReloading = true;
        audioSource.clip = mainGun.reloadSound;
        audioSource.Play();
        
        yield return new WaitForSeconds(mainGun.reloadTime);
        
        // 총알 수 조절
        if (mainGun.isClosedBolt && ammoInMag >= 1) // 약실 장전
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
		isReloading = false;
        print("Reloading done\n" + ammoInMag + " / " + spareAmmo);
		gunUI.ChangeAmmoText($"{ammoInMag}/{spareAmmo}");

		bulletImage.gameObject.SetActive(false);
		circleImage.gameObject.SetActive(false);


		StopCoroutine(Reload());
    }

    IEnumerator FillCircle()
    {
        circleImage.fillAmount = 0f;

		float processTime = 0f;
        float amount = 0f;

        while (processTime < mainGun.reloadTime)
        {
            processTime += Time.deltaTime;
            amount = processTime / mainGun.reloadTime;

            circleImage.fillAmount = amount;
            yield return null;
        }
    }
}
