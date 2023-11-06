using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance => _instance;
    private static GameManager _instance;

	public GameObject goWhenWin;
	public GameObject goWhenLose;

	public string lobbySceneName;

	private void Awake()
	{
		if (_instance != null)
		{
			Destroy(this.gameObject);
		} else
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);
		}

		if (lobbySceneName == string.Empty)
		{
			lobbySceneName = "Lobby"; // auto-assign
		}
	}

	public void WinGame()
	{
		goWhenWin.SetActive(true);
	}

	public void LoseGame()
	{
		goWhenLose.SetActive(true);
	}

	public void GoLobby()
	{
		SceneManager.LoadScene(lobbySceneName);
	}
}
