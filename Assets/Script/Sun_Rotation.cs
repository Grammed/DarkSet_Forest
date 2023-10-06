using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun_Rotation : MonoBehaviour
{
    public GameObject Sun;
    public bool isDay;

    void Start()
    {
        Sun.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        isDay = true;
    }
    void Update()
    {
        Sun.transform.Rotate(new Vector3(-1f, 0, 0) * Time.deltaTime);
        /*if (Sun.transform.rotation.x < 0f && Sun.transform.rotation.x > -170f)
        {
            isDay = false;
            print("¹ã");
        }
        else
        {
            isDay = true;
            print("³·");
        }
        */
    }
}
