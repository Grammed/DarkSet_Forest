using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowFollow : MonoBehaviour
{
    public float followSpeed = 100.0f;

    public GameObject player;

    private void Update()
    {
        transform.position = player.transform.position + new Vector3(0, 2, 0);
    }

}
