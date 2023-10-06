using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dead : MonoBehaviour
{
    void Update()
    {
        if(gameObject.transform.position.y <= 380)
            SceneManager.LoadScene("Dead");
        
    }
}
