using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Water : MonoBehaviour
{
    private Image WaterFill;

    // Start is called before the first frame update
    void Start()
    { 
        WaterFill = GetComponent<Image>();
        if(!PlayerPrefs.HasKey("Water"))
        {
            PlayerPrefs.SetFloat("Water", 1f);
        }
        if (PlayerPrefs.HasKey("Water"))
        {
            WaterFill.fillAmount = PlayerPrefs.GetFloat("Water");
        }
       
        StartCoroutine("Watercount");
    }

    // Update is called once per frame
    void Update()
    {
        if(WaterFill.fillAmount <= 0)
        {

        }
    }

    IEnumerator Watercount()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            WaterFill.fillAmount -= 0.005f;
        }
    }
}
