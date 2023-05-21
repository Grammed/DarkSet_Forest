using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Food : MonoBehaviour
{
    private Image FoodFill;

    // Start is called before the first frame update
    void Start()
    {
        FoodFill = GetComponent<Image>();
        if (!PlayerPrefs.HasKey("Food"))
        {
            PlayerPrefs.SetFloat("Food", 1f);
        }
        if (PlayerPrefs.HasKey("Food"))
        {
            FoodFill.fillAmount = PlayerPrefs.GetFloat("Food");
        }
       
        StartCoroutine("Foodcount");
    }

    // Update is called once per frame
    void Update()
    {
        if (FoodFill.fillAmount <= 0)
        {

        }
    }

    IEnumerator Foodcount()
    {
        while (true)
        {
            if (FoodFill.fillAmount >= 0)
            {
                yield return new WaitForSeconds(1f);
                FoodFill.fillAmount -= 0.002f;
            }
        }
    }
}
