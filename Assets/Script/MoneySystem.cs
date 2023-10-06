using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneySystem : MonoBehaviour
{
    public int count;
    [SerializeField]private Text moneyText;
    void Awake()
    {
        count = 0;
    }

    void Update()
    {
        moneyText.text = "money : " + count;
    }


}
