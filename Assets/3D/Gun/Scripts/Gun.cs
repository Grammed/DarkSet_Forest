using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    GameObject muzzle;
    [SerializeField]
    private float fireTime = 0.2f;
    private bool canFire = true;

    [SerializeField]
    private int maxAmmoInMag = 30;
    private int ammoInMag;
    private int maxSpareAmmo;
    private int spareAmmo;

    [SerializeField]
    private float recoilY = 1;
    void Start()
    {
        muzzle = transform.Find("Muzzle").gameObject;
        ammoInMag = maxAmmoInMag;
        spareAmmo = maxSpareAmmo;
    }

    
    void Update()
    {
        bool isFiring = Input.GetButton("Fire1");
        if (isFiring && ammoInMag >= 1 && canFire)
        {
            Fire();
        }
    }

    void Fire()
    {
        canFire = false;

		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
		RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) ;

        StartCoroutine(FireDelay());
	}

    void Recoil()
    {
        Quaternion q = Camera.main.transform.rotation;
        q.y -= recoilY;
        Camera.main.transform.rotation = q;
    }

    IEnumerator FireDelay()
    {
        yield return new WaitForSeconds(fireTime);
        canFire = true;
    }
}
