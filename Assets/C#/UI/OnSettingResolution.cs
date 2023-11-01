using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSettingResolution : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            panel.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
