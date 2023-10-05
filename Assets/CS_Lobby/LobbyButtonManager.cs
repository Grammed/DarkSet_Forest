using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyButtonManager : MonoBehaviour
{
	[SerializeField] GameObject nowMapBG;
    [SerializeField] TextMeshProUGUI txt_PrevMap;
    [SerializeField] TextMeshProUGUI txt_PrevMapName;
    [SerializeField] TextMeshProUGUI txt_NowMapName;
    [SerializeField] TextMeshProUGUI txt_NextMap;
    [SerializeField] TextMeshProUGUI txt_NextMapName;

    [SerializeField] GameObject leftBtn;
    [SerializeField] GameObject rightBtn;

	[SerializeField] GameObject[] titleObjs;
    [SerializeField] GameObject[] chooseMapObjs;

    [SerializeField] Sprite[] mapImages;
    /// <summary>
    /// 맵 이름이랑 같게
    /// </summary>
    [SerializeField] string[] mapNames;

    int maxIdx = -1;
    int mapIdx = 0;
    public int MapIdx /// need hotfix
    {
        get => mapIdx;
        set
        {
            mapIdx = Mathf.Clamp(value, 0, maxIdx);

            txt_PrevMap.gameObject.SetActive(mapIdx != 0);
            txt_PrevMapName.gameObject.SetActive(mapIdx != 0);
            leftBtn.SetActive(mapIdx != 0);
            if (mapIdx != 0)
                txt_PrevMapName.text = mapNames[mapIdx - 1];


            txt_NextMap.gameObject.SetActive(mapIdx != maxIdx);
            txt_NextMapName.gameObject.SetActive(mapIdx != maxIdx);
            rightBtn.SetActive(mapIdx != maxIdx);

            if (mapIdx != maxIdx)
                txt_NextMapName.text = mapNames[mapIdx + 1];
        }
    }


    private void Awake()
	{
		maxIdx = Mathf.Min(mapImages.Length, mapNames.Length) - 1;
		MapIdx = 0;
        
        nowMapBG.GetComponent<Image>().sprite = mapImages[MapIdx];
        txt_NowMapName.text = mapNames[MapIdx];
	}

	public void OnClickStartButton()
    {
        foreach(var obj in titleObjs) 
        {
            obj.SetActive(false);
        }
        foreach(var obj in chooseMapObjs) 
        {
            obj.SetActive(true);
        }
    }

    public void OnClickExitButton()
    {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
	}

    public void OnClickGoBack()
    {
		foreach (var obj in chooseMapObjs) 
        {
			obj.SetActive(false);
		}
		foreach (var obj in titleObjs) 
        {
			obj.SetActive(true);
		}
	}

    public void OnClickLeftButton()
    {
        MapIdx -= 1;
    }

    public void OnClickRightButton()
    {
        MapIdx += 1;
    }

    public void OnClickChooseButton()
    {
        SceneManager.LoadScene(mapNames[MapIdx]);
    }
}
