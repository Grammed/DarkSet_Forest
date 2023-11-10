using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance => _instance;
    private static GameManager _instance;

	[SerializeField] GameObject canvasGO;
	[SerializeField] GameObject goWhenWin;
	[SerializeField] GameObject goWhenLose;

	[SerializeField] Text killedEnemyText;
	[SerializeField] Text survivedWaveText;
	[SerializeField] Text earnGoldText;

	[SerializeField] string lobbySceneName;

	public GameObject panel;

	[HideInInspector]
	public int killedEnemy = 0;
	[HideInInspector]
	public int survivedWave = 0;
	[HideInInspector]
	public int earnGold = 0;

	private void Awake()
	{
		Cursor.lockState = CursorLockMode.Locked;
		if (_instance != null)
		{
			Destroy(this.gameObject);
		} else
		{
			_instance = this;
			//DontDestroyOnLoad(gameObject);
		}

		if (lobbySceneName == string.Empty)
		{
			lobbySceneName = "Lobby"; // auto-assign
		}
	}


	public void WinGame()
	{
		EndGame();
		goWhenWin.SetActive(true);
	}

	public void LoseGame()
	{
		EndGame();
		goWhenLose.SetActive(true);
	}
	public void Resume()
	{
        panel.SetActive(false);
		Time.timeScale = 1f;

    }
	public void ExitButton()
	{
		Application.Quit();
	}
	private void EndGame()
	{
		canvasGO.SetActive(true);
		Cursor.lockState = CursorLockMode.None;

		killedEnemyText.text = "무력화한 적 수: " + killedEnemy;
		survivedWaveText.text = "생존한 웨이브 수: " + survivedWave;
		earnGoldText.text = "얻은 코인: " + earnGold;
	}

	public void GoLobby()
	{
		SceneManager.LoadScene(lobbySceneName);
	}
}
