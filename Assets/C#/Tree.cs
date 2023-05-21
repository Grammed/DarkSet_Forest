using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Tree : MonoBehaviour
{
    private int HP = 1;
    public GameObject Wood;
    private bool doAxe = false;

    public IEnumerator Hit_Tree()
    {
        if (doAxe == false)
        {
            yield return new WaitForSeconds(4f);
            HP--;
        }
        if(doAxe == true)
        {
            yield return new WaitForSeconds(2f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            for (int i = 0; i < Random.Range(1, 3); i++)
            {
                StartCoroutine(Woodspawn());
            }
        }

        IEnumerator Woodspawn()
        {
            Vector3 TreeSum = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, gameObject.transform.position.z);
            Instantiate(Wood, TreeSum, Quaternion.identity);
            Destroy(gameObject);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
