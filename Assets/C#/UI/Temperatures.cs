using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Temperatures : MonoBehaviour
{
    private Image TemperaturesFill;

    // Start is called before the first frame update
    void Start()
    {
        TemperaturesFill = GetComponent<Image>();
        if (!PlayerPrefs.HasKey("Temperatures"))
        {
            PlayerPrefs.SetFloat("Temperatures", 0.5f);
        }
        if (PlayerPrefs.HasKey("Temperatures"))
        {
            TemperaturesFill.fillAmount = PlayerPrefs.GetFloat("Temperatures");
        }
        
        StartCoroutine("Temperaturescount");
    }

    // Update is called once per frame
    void Update()
    {
        if (TemperaturesFill.fillAmount <= 0)
        {

        }
    }

    IEnumerator Temperaturescount()
    {
        while (true)
        {
            if (SceneManager.GetActiveScene().name == "Forest")
            {
                yield return new WaitForSeconds(1f);
                TemperaturesFill.fillAmount -= 0.0001f;
            }
            /* �� �̸��� �־ �µ� �����ϴ� �ڵ��� �ʸ��� �ٸ��� �����ؾ��ϴϱ� �� ������
            if (SceneManager.GetActiveScene().name == "Forest")
            {
                yield return new WaitForSeconds(1f);
                TemperaturesFill.fillAmount -= 0.0001f;
            }
            */
        }
    }
}
