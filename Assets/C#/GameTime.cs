using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameTime : MonoBehaviour
{
    [SerializeField]
    private Text timeText;
    [SerializeField]
    private Text noonText;
    private float time = 21600;
    private int hour = 0;
    private int minute = 0;

    private void Start()
    {

    }

    private void Update()
    {
        GamingTime();
    }


    private void GamingTime()
    {

        if (time > 0)
        {
            AMChangeTime();
        }

        if(time > 43200)
        {
            noonText.text = "P.M";
        }

        if (time > 46800)
        {
            PMChangeTime();
        }

        if(time > 86400)
        {
            noonText.text = "A.M";
        }

        if(time > 90000)
        {
            time = 3600;
        }

        time += Time.deltaTime * 3600;
    }

    private void AMChangeTime()
    {
        hour = ((int)time / 3600);
        minute = ((int)time / 60 % 60);
        timeText.text = string.Format("{0:00}:{1:00}", hour, minute);
    }

    private void PMChangeTime()
    {
        hour = ((int)time / 3600)-12;
        minute = ((int)time / 60 % 60);
        timeText.text = string.Format("{0:00}:{1:00}", hour, minute);
    }
}