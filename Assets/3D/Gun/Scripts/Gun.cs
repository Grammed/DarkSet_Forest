using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Camera cam;
	[SerializeField]
	private float gunDamage = 25;
	GameObject muzzle;

	[SerializeField]
    private float fireTime = 0.2f;

    /// <summary> 딜레이 후 발사가 가능할 때 true </summary>
    private bool canFire = true;
    [SerializeField]
    private bool isClosedBolt = true; // 클로즈드볼트

    [Header("Recoil")]
	[SerializeField]
	[Tooltip("좌우 반동")]
	private float recoilX = 1; // 좌우 반동
	[SerializeField]
	[Tooltip("상하 반동")]
	private float recoilY = 1; // 상하 반동
    [Tooltip("앞뒤 반동")]
    [SerializeField]
    private float recoilZ = 1; // 앞뒤 반동


	[SerializeField]
	[Tooltip("인체공학, 높을수록 반동 회복이 빠름")]
    private float ergonomic = 70;
    

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
        if (Input.GetKeyDown(reloadKey) && !isReloading && ammoInMag <= maxAmmoInMag)
        {
            StartCoroutine(Reload());
        }
    }

    void Fire()
    {
        print("Fire");
        canFire = false;
        ammoInMag -= 1;

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
        /// !!! NOT WORKING! NEEDS TO BE FIXED ASAP !!!
        /// 농민교야 도와줭
        /// 상하좌우 반동 안먹음 ㅠㅠ

        //print("Recoil");
        //Quaternion q = cam.transform.localRotation;
        //q.y -= recoilY;
        //q.x += Random.Range(-recoilX, recoilX);
        //cam.transform.localRotation = q;


        // 총 자체 밀리는 반동
        
        changePos = transform.localPosition + (Vector3.back * recoilZ);
        transform.localPosition = changePos;
        StartCoroutine(ReboundRecoil());
    }

    IEnumerator ReboundRecoil() 
	{
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
        yield return new WaitForSeconds(reloadTime);
        
        if (isClosedBolt && ammoInMag >= 1) // 약실 장전
        {
            spareAmmo -= maxAmmoInMag - ammoInMag + 1;
            ammoInMag = maxAmmoInMag + 1;
        } else
        {
			spareAmmo -= maxAmmoInMag - ammoInMag;
			ammoInMag = maxAmmoInMag;
		}
		isReloading = false;
        print("Reloading done");
		StopCoroutine(Reload());
    }
}
