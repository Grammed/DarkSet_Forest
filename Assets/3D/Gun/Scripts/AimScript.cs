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
    GameObject scopeShapeGO;
    
    Camera theCamera;
    void Start()
    {
		theCamera = Camera.main;
        fovNotAim = theCamera.fieldOfView;
        scopeShapeGO = GameObject.Find("PlayerUI_Canvas").transform.Find("ScopeImage-Parent").Find("ScopeImage").gameObject;
        player = FindObjectOfType<PlayerController>();

        mouseSenseTemp = player.lookSensitivity;
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
        theCamera.fieldOfView = fovWhenAim;
        scopeShapeGO.SetActive(true);
        player.lookSensitivity = mouseSenseWhenAim;

        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");

		if (mouseWheel != 0)
        {
            fovWhenAim += mouseWheel * zoomSpeed;
            fovWhenAim = Mathf.Clamp(fovWhenAim, minFovWhenAim, maxFovWhenAim);
        }
    }

    void CancelAim()
    {
        theCamera.fieldOfView = fovNotAim;
		scopeShapeGO.SetActive(false);
        player.lookSensitivity = mouseSenseTemp;
	}
}
