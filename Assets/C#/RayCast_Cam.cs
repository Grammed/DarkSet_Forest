using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast_Cam : MonoBehaviour
{
    public float maxDistance = 2;
    private RaycastHit hit;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(hit.collider.tag == "Tree")
                {
                    hit.collider.GetComponent<Tree>().Hit_Tree();
                }
            }
            //Debug.Log("hit point : " + hit.point + ", distance : " + hit.distance + ", name : " + hit.collider.name);
            //Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 1000f, Color.red);
        }
    }
}
