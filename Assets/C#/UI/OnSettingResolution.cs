using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSettingResolution : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private bool isOn;

    private void Start()
    {
        isOn = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && isOn == false)
        {
            isOn = true;
            panel.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
        }
        
        else if (Input.GetKeyDown(KeyCode.O) && isOn == true)
        {
            isOn = false;
            panel.SetActive(false);
            Time.timeScale = 1.0f;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
    }
}
