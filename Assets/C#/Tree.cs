using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Tree : MonoBehaviour
{
    private int HP = 3;
    public GameObject Wood;
    
    public void Hit_Tree()
    {
        HP--;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(HP <= 0)
        {
            Vector3 TreeSum = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, gameObject.transform.position.z);
            for (int i = 0; i < Random.Range(1, 3); i++)
            {
                Instantiate(Wood, TreeSum, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
