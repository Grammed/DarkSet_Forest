using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Back_to_main : MonoBehaviour
{
    public void Click()
    {
        SceneManager.LoadScene("SampleScene");
    }

}
