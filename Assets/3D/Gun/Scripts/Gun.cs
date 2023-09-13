using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [HideInInspector]
    public PlayerController player;
    public Camera cam;
	[SerializeField]
	private float gunDamage = 25;
	GameObject muzzle;

    [SerializeField]
    GunSoundPool gunSoundPool;

	[SerializeField]
    private float fireTime = 0.2f;

    /// <summary> 딜레이 후 발사가 가능할 때 true </summary>
    private bool canFire = true;
    [SerializeField]
    private bool isClosedBolt = true; // 클로즈드볼트

    [Header("Recoil")]
	[SerializeField]
	[Tooltip("좌우 반동")]
	/// <summary> 좌우 반동 </summary>
	private float recoilX = 1;
	[SerializeField]
	[Tooltip("상하 반동")]
	private float recoilY = 1; // 상하 반동
    [Tooltip("앞뒤 반동")]
    [SerializeField]
    private float recoilZ = 1; // 앞뒤 반동


	[SerializeField]
	[Tooltip("인체공학, 높을수록 반동 회복이 빠름")]
    private float ergonomic = 70;

    [SerializeField]
    private AudioClip reloadSound;
    [SerializeField]
    private AudioClip fireSound;
    private AudioSource audioSource;

	private bool isReloading = false;
	private KeyCode reloadKey = KeyCode.R;
    [SerializeField]
    private float reloadTime = 3f;

    [Header("Ammo")]
	[SerializeField]
	private int maxAmmoInMag = 30;
	[SerializeField]
	private int ammoInMag;
	[SerializeField]
	private int maxSpareAmmo;
	[SerializeField]
	private int spareAmmo;

	void Start()
    {
        muzzle = transform.Find("Muzzle").gameObject;
        ammoInMag = maxAmmoInMag;
        spareAmmo = maxSpareAmmo;
		originPos = transform.localPosition;

        player = FindAnyObjectByType<PlayerController>();
        cam = player.theCamera;
        audioSource = GetComponent<AudioSource>();

        gunSoundPool = FindAnyObjectByType<GunSoundPool>();
	}

    
    void Update()
    {
        // 총 발사
        bool isFire = Input.GetButton("Fire1");
        if (isFire && !isReloading && ammoInMag >= 1 && canFire)
        {
            Fire();
        }

        // 재장전
        if (Input.GetKeyDown(reloadKey) && !isReloading && ammoInMag <= maxAmmoInMag && spareAmmo >= 1)
        {
            StartCoroutine(Reload());
        }
    }

    void Fire()
    {
        print("Fire");
        //audioSource.clip = fireSound;
        //audioSource.Play();
        canFire = false;
        ammoInMag -= 1;

        gunSoundPool.Pop();

		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
		RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Enemy") // 적에 맞았을 때
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                    enemy.Hit(gunDamage);
            }
        }

        StartCoroutine(FireDelay()); // 발사 딜레이
        Recoil();
	}

    private Vector3 originPos;
    private Vector3 changePos;
    void Recoil()
    {

        print("Recoil");
        player.currentCameraRotationX -= recoilY;

        Vector3 rot = player.transform.eulerAngles;
        rot.y += Random.Range(-recoilX, recoilX);
        player.transform.eulerAngles = rot;

        // 총 자체 밀리는 반동
        
        changePos = transform.localPosition + (Vector3.back * recoilZ);
        transform.localPosition = changePos;
        StopCoroutine(ReboundRecoil());
        StartCoroutine(ReboundRecoil());
    }

    IEnumerator ReboundRecoil() 
	{
        // 반동 회복
		while (transform.localPosition != originPos)
        {
			transform.localPosition = Vector3.Lerp(transform.localPosition, originPos, 0.1f * (ergonomic / 100f));
			yield return null;
		}

	}
    IEnumerator FireDelay()
    {
        // 발사 딜레이
        yield return new WaitForSeconds(fireTime);
        canFire = true;
        StopCoroutine(FireDelay());
    }

    IEnumerator Reload()
    {
        print("Start Reloading");
        
        isReloading = true;
        audioSource.clip = reloadSound;
        audioSource.Play();

        if (audioSource.isPlaying == false)
        {

        }
        yield return new WaitForSeconds(reloadTime);
        
        if (isClosedBolt && ammoInMag >= 1) // 약실 장전
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
		isReloading = false;
        print("Reloading done");
        print(ammoInMag + " / " + spareAmmo);
		StopCoroutine(Reload());
    }


}
