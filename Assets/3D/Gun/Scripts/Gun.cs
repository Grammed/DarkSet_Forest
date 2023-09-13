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

    /// <summary> ������ �� �߻簡 ������ �� true </summary>
    private bool canFire = true;
    [SerializeField]
    private bool isClosedBolt = true; // Ŭ����庼Ʈ

    [Header("Recoil")]
	[SerializeField]
	[Tooltip("�¿� �ݵ�")]
	/// <summary> �¿� �ݵ� </summary>
	private float recoilX = 1;
	[SerializeField]
	[Tooltip("���� �ݵ�")]
	private float recoilY = 1; // ���� �ݵ�
    [Tooltip("�յ� �ݵ�")]
    [SerializeField]
    private float recoilZ = 1; // �յ� �ݵ�


	[SerializeField]
	[Tooltip("��ü����, �������� �ݵ� ȸ���� ����")]
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
        // �� �߻�
        bool isFire = Input.GetButton("Fire1");
        if (isFire && !isReloading && ammoInMag >= 1 && canFire)
        {
            Fire();
        }

        // ������
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
            if (hit.transform.tag == "Enemy") // ���� �¾��� ��
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                    enemy.Hit(gunDamage);
            }
        }

        StartCoroutine(FireDelay()); // �߻� ������
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

        // �� ��ü �и��� �ݵ�
        
        changePos = transform.localPosition + (Vector3.back * recoilZ);
        transform.localPosition = changePos;
        StopCoroutine(ReboundRecoil());
        StartCoroutine(ReboundRecoil());
    }

    IEnumerator ReboundRecoil() 
	{
        // �ݵ� ȸ��
		while (transform.localPosition != originPos)
        {
			transform.localPosition = Vector3.Lerp(transform.localPosition, originPos, 0.1f * (ergonomic / 100f));
			yield return null;
		}

	}
    IEnumerator FireDelay()
    {
        // �߻� ������
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
        
        if (isClosedBolt && ammoInMag >= 1) // ��� ����
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
