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

    /// <summary> ������ �� �߻簡 ������ �� true </summary>
    private bool canFire = true;
    [SerializeField]
    private bool isClosedBolt = true; // Ŭ����庼Ʈ

    [Header("Recoil")]
	[SerializeField]
	[Tooltip("�¿� �ݵ�")]
	private float recoilX = 1; // �¿� �ݵ�
	[SerializeField]
	[Tooltip("���� �ݵ�")]
	private float recoilY = 1; // ���� �ݵ�
    [Tooltip("�յ� �ݵ�")]
    [SerializeField]
    private float recoilZ = 1; // �յ� �ݵ�


	[SerializeField]
	[Tooltip("��ü����, �������� �ݵ� ȸ���� ����")]
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
        // �� �߻�
        bool isFire = Input.GetButton("Fire1");
        if (isFire && !isReloading && ammoInMag >= 1 && canFire)
        {
            Fire();
        }

        // ������
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
        /// !!! NOT WORKING! NEEDS TO BE FIXED ASAP !!!
        /// ��α��� ���͢a
        /// �����¿� �ݵ� �ȸ��� �Ф�

        //print("Recoil");
        //Quaternion q = cam.transform.localRotation;
        //q.y -= recoilY;
        //q.x += Random.Range(-recoilX, recoilX);
        //cam.transform.localRotation = q;


        // �� ��ü �и��� �ݵ�
        
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
        // �߻� ������
        yield return new WaitForSeconds(fireTime);
        canFire = true;
        StopCoroutine(FireDelay());
    }

    IEnumerator Reload()
    {
        print("Start Reloading");
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        
        if (isClosedBolt && ammoInMag >= 1) // ��� ����
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
