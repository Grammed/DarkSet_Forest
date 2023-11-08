using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BunkerLocation : MonoBehaviour
{
    private Image bunkerIcon;
    [SerializeField]
    private GameObject bunkerGO;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;


    private void Start()
    {
        bunkerIcon = GetComponent<Image>();

        float width = bunkerIcon.rectTransform.rect.height;
        float height = bunkerIcon.rectTransform.rect.height;

        //minX = Screen.width / -2 + width;
        //maxX = Screen.width / 2 - width;

        //minY = Screen.height / -2 + height;
        //maxY = Screen.height / 2 - height;

        minX = width / 2;
        maxX = Screen.width - width / 2;
        minY = height / 2;
        maxY = Screen.height - height / 2;
    }


    private void Update()
    {
        Vector3 bunkerLocationOnScreen = Camera.main.WorldToScreenPoint(bunkerGO.transform.position + new Vector3(0, 2));
        print(bunkerLocationOnScreen);

        // 상점이 시야 내에 있을 때 위치를 화면 경계 내로 제한
        bunkerLocationOnScreen.x = Mathf.Clamp(bunkerLocationOnScreen.x, minX, maxX);
        bunkerLocationOnScreen.y = Mathf.Clamp(bunkerLocationOnScreen.y, minY, maxY);
        
        if (bunkerLocationOnScreen.z < 0) // 뒤에 있다면
        {
            bunkerLocationOnScreen.y *= -1;
			bunkerLocationOnScreen.x *= -1;
        }

        bunkerIcon.rectTransform.position = bunkerLocationOnScreen;
    }
}
