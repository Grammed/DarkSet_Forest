using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimScript : MonoBehaviour
{
    [SerializeField] KeyCode aimKey;
    float fovNotAim;
    [SerializeField] float fovWhenAim;
    GameObject scopeShapeGO;
    
    Camera theCamera;
    void Start()
    {
		theCamera = Camera.main;
        fovNotAim = theCamera.fieldOfView;
        scopeShapeGO = GameObject.Find("PlayerUI_Canvas").transform.Find("AimImage").gameObject;
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
    }

    void CancelAim()
    {
        theCamera.fieldOfView = fovNotAim;
		scopeShapeGO.SetActive(false);
	}
}
