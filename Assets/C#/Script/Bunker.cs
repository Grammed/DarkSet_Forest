using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bunker:MonoBehaviour
{
    public float nowBunkerHp;
    public float maxBunkerHp = 2000;
    [SerializeField]
    private GameObject GameOverTxt;

    [SerializeField]
    private Slider healthSlider;

	private void Awake()
	{
        nowBunkerHp = maxBunkerHp;
	}
	private void Update()
    {
        if(nowBunkerHp <= 0)
        {
            GameManager.Instance.LoseGame();
            Destroy(gameObject);
        }

        healthSlider.value = nowBunkerHp / maxBunkerHp;

        print(nowBunkerHp);
    }
}