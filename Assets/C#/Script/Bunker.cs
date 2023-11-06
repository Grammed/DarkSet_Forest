using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunker:MonoBehaviour
{
    public float bunkerHp = 10000;
    [SerializeField]
    private GameObject GameOverTxt;
    private void Update()
    {
        if(bunkerHp <= 0)
        {
            GameOverTxt.SetActive(true);
            Destroy(gameObject);
        }

        print(bunkerHp);
    }
}