using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bracken : MonoBehaviour
{
    public GameObject Brackenitem;

    public void Brackenbreak()
    {
        Vector3 BrackenSum = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, gameObject.transform.position.z);
        Instantiate(Brackenitem, BrackenSum, Quaternion.identity);
        Destroy(gameObject);
    }
}
