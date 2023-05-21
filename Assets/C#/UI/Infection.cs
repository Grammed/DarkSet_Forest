using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Infection : MonoBehaviour
{
    private Image InfectionFill;

    // Start is called before the first frame update
    void Start()
    {
        InfectionFill = GetComponent<Image>();
        if (!PlayerPrefs.HasKey("Infection"))
        {
            PlayerPrefs.SetFloat("Infection", 0f);
        }
        if (PlayerPrefs.HasKey("Infection"))
        {
            InfectionFill.fillAmount = PlayerPrefs.GetFloat("Infection");
        }
        
        //StartCoroutine("Infectioncount"); //나중에 상처 입을때 켜져야 함
    }

    // Update is called once per frame
    void Update()
    {
        if (InfectionFill.fillAmount <= 0)
        {

        }
    }

    IEnumerator Temperaturescount()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            InfectionFill.fillAmount -= 0.005f;
        }
    }
}
