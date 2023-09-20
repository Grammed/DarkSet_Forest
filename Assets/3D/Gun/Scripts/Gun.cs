using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
	[Header("References")]
    [HideInInspector]
    public PlayerController player;
    public Camera cam;
    [SerializeField]
    private GunUI gunUI;
	private GameObject muzzle;

	[Header("Fire")]
	[SerializeField]
	private float gunDamage = 25;
	[SerializeField]
    private float fireTime = 0.2f;

	/// <summary> ������ �� �߻簡 ������ �� false </summary>
	private bool isFireDelaying = false;

	#region Recoil

	[Header("Recoil")]
	[SerializeField]
	[Tooltip("�¿� �ݵ�")]
	/// <summary> �¿� �ݵ� </summary>
	private float recoilX = 1;
	[SerializeField]
	[Tooltip("���� �ݵ�")]
	/// <summary> ���� �ݵ� </summary>
	private float recoilY = 1; // ���� �ݵ�
    [Tooltip("�� ��ü �յ� �ݵ�")]
	/// <summary> �� ��ü �յ� �ݵ� </summary>
	[SerializeField]
    private float recoilZ = 1; // �յ� �ݵ�

	[SerializeField]
	[Tooltip("��ü����, �������� �ݵ� ȸ���� ����")]
    private float ergonomic = 70;

	#endregion

	#region Sound

	[Header("Sound")]
	[SerializeField]
	GunSoundPool gunSoundPool;
	[SerializeField]
    private AudioClip reloadSound;
    [SerializeField]
    private AudioClip fireSound;
    private AudioSource audioSource;

	#endregion

	#region Reload

	[Header("Reload")]
	private bool isReloading = false;
	private KeyCode reloadKey = KeyCode.R;
    [SerializeField]
    private float reloadTime = 3f;
	[SerializeField]
	private bool isClosedBolt = true; // Ŭ����� ��Ʈ

	#endregion

	#region Ammo

	[Header("Ammo")]
	[SerializeField]
	private int maxAmmoInMag = 30;
	[SerializeField]
	private int ammoInMag;
	[SerializeField]
	private int maxSpareAmmo;
	[SerializeField]
	private int spareAmmo;

	#endregion

	void Start()
    {
		#region init

		muzzle = transform.Find("Muzzle").gameObject;
        ammoInMag = maxAmmoInMag;
        spareAmmo = maxSpareAmmo;
		originPos = transform.localPosition;

        player = FindAnyObjectByType<PlayerController>();
        cam = player.theCamera;
        audioSource = GetComponent<AudioSource>();

        gunSoundPool = FindAnyObjectByType<GunSoundPool>();
        gunUI = FindAnyObjectByType<GunUI>() as GunUI;

		#endregion


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
		bool canFire = 
            Input.GetButton("Fire1") && !isReloading && ammoInMag >= 1 && !isFireDelaying;
		if (canFire)
		{
			Fire();
		}

        // ������
        // Ű Ŭ�� + ���� �� �ƴ� + �ִ� ź�� �ƴ� + ���� ź�� �ϳ��� ��������
        bool canReload = 
            Input.GetKeyDown(reloadKey) && !isReloading
            && ammoInMag <= maxAmmoInMag && spareAmmo >= 1; 
		if (canReload)
		{
			StartCoroutine(Reload());
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

        // ������Ʈ Ǯ���� ���� ����
        gunSoundPool.Pop();

		// ī�޶�κ��� ���� �߻�
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
		player.currentCameraRotationX -= recoilY;

		// ī�޶� ȸ�� ���� //
		Vector3 rot = player.transform.eulerAngles;
        rot.y += Random.Range(-recoilX, recoilX);
        player.transform.eulerAngles = rot; 

        // �� ��ü �и��� �ݵ�
        changePos = transform.localPosition + (Vector3.back * recoilZ);
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
			transform.localPosition = Vector3.Lerp(transform.localPosition, originPos, 0.1f * (ergonomic / 100f));
			yield return null;
		}

	}

	// ī�޶� ȸ�� ����
	IEnumerator FireDelay()
    {
        yield return new WaitForSeconds(fireTime);
        isFireDelaying = false;
        StopCoroutine(FireDelay());
    }

	// ������
	IEnumerator Reload()
    {
        // Update �޼��忡�� ������
        print("Start Reloading");
        
        // ���� ���� �غ� �� �÷���
        isReloading = true;
        audioSource.clip = reloadSound;
        audioSource.Play();
        
        yield return new WaitForSeconds(reloadTime);
        
        // �Ѿ� �� ����
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

        // ���� ��
		isReloading = false;
        print("Reloading done\n" + ammoInMag + " / " + spareAmmo);
		StopCoroutine(Reload());
    }


}
