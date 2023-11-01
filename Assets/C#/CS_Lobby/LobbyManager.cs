using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
	[SerializeField] GameObject nowMapThumbnail;
    [SerializeField] TextMeshProUGUI txt_PrevMap;
    [SerializeField] TextMeshProUGUI txt_PrevMapName;
    [SerializeField] TextMeshProUGUI txt_NowMapName;
    [SerializeField] TextMeshProUGUI txt_NextMap;
    [SerializeField] TextMeshProUGUI txt_NextMapName;

    [SerializeField] GameObject leftBtn;
    [SerializeField] GameObject rightBtn;

	[SerializeField] GameObject titleSet;
    [SerializeField] GameObject chooseMapSet;

    [SerializeField] Sprite[] mapImages;
   
    [SerializeField] string[] mapNamesToDisplay;
	/// <summary>
	/// 맵 이름이랑 같게
	/// </summary>
	[SerializeField] string[] mapNamesInEditor;

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
                txt_PrevMapName.text = mapNamesToDisplay[mapIdx - 1];


            txt_NextMap.gameObject.SetActive(mapIdx != maxIdx);
            txt_NextMapName.gameObject.SetActive(mapIdx != maxIdx);
            rightBtn.SetActive(mapIdx != maxIdx);

            if (mapIdx != maxIdx)
                txt_NextMapName.text = mapNamesToDisplay[mapIdx + 1];


            txt_NowMapName.text = mapNamesToDisplay[mapIdx];
            nowMapThumbnail.GetComponent<Image>().sprite = mapImages[mapIdx];
        }
    }

    void setMapIdx()
    {

    }

    private void Awake()
	{
		maxIdx = Mathf.Min(mapImages.Length, mapNamesToDisplay.Length) - 1;
		MapIdx = 0;
        
        nowMapThumbnail.GetComponent<Image>().sprite = mapImages[MapIdx];
        txt_NowMapName.text = mapNamesToDisplay[MapIdx];
	}

	public void OnClickStartButton()
    {
        titleSet.SetActive(false);
        chooseMapSet.SetActive(true);
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
		titleSet.SetActive(true);
		chooseMapSet.SetActive(false);
	}

    public void OnClickLeftButton()
    {
        MapIdx -= 1;
        print("left");
    }

    public void OnClickRightButton()
    {
        MapIdx += 1;
        print("right");
    }

    public void OnClickChooseButton()
    {
        SceneManager.LoadScene(mapNamesInEditor[MapIdx]);
    }
}
