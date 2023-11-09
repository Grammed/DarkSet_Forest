using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimScript : MonoBehaviour
{
    [SerializeField] KeyCode aimKey;
    float fovNotAim;

    [SerializeField] float minFovWhenAim;
    [SerializeField] float maxFovWhenAim;
    [SerializeField] float fovWhenAim;
    [SerializeField] float zoomSpeed = 2;
    [SerializeField] float mouseSenseWhenAim;
    float mouseSenseTemp;

    PlayerController player;
    WeaponManager weaponManager;
    GameObject scopeShapeGO;
    Gun gun;
    SO_Gun soGun;
    float reducedRecoil;
    float originRecoil;
    
    Camera theCamera;
    void Start()
    {
		theCamera = Camera.main;
        fovNotAim = theCamera.fieldOfView;
        scopeShapeGO = GameObject.Find("PlayerUI_Canvas").transform.Find("ScopeImage-Parent").Find("ScopeImage").gameObject;
        player = FindObjectOfType<PlayerController>();
        weaponManager = FindObjectOfType<WeaponManager>();

        mouseSenseTemp = player.lookSensitivity;
        gun = GetComponent<Gun>();
        soGun = gun.SO_Gun;
        reducedRecoil = soGun.recoilY / 3f;
        originRecoil = soGun.recoilY;
	}

    void Update()
    {
        if (Input.GetKey(aimKey))
        {
            TakeAim();
        }

        if (Input.GetKeyUp(aimKey))
        {
            CancelAim();
        }
    }

    void TakeAim()
    {
        weaponManager.canSwap = false;
        theCamera.fieldOfView = fovWhenAim;
        scopeShapeGO.SetActive(true);
        player.lookSensitivity = mouseSenseWhenAim;
        soGun.recoilY = reducedRecoil;


        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");

		if (mouseWheel != 0)
        {
            fovWhenAim += mouseWheel * zoomSpeed;
            fovWhenAim = Mathf.Clamp(fovWhenAim, minFovWhenAim, maxFovWhenAim);
        }
    }

    void CancelAim()
    {
        weaponManager.canSwap = true;
        theCamera.fieldOfView = fovNotAim;
		scopeShapeGO.SetActive(false);
        player.lookSensitivity = mouseSenseTemp;

        soGun.recoilY = originRecoil;
	}
}
