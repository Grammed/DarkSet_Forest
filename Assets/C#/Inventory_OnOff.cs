using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_OnOff : MonoBehaviour
{
    public GameObject Inventory;
    // Start is called before the first frame update
    void Start()
    {
        Inventory.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        PlayerController.CamRotateEnable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && Inventory.activeSelf == false)
        {
            Inventory.SetActive(true);
            PlayerController.CamRotateEnable = false;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetKeyDown(KeyCode.E) && Inventory.activeSelf == true)
        {
            Inventory.SetActive(false);
            PlayerController.CamRotateEnable=true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
