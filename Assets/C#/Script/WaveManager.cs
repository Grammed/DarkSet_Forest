using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum EnemyType
{//몬스터 종류
    nomal,
    walking,
    arrow
}
[System.Serializable]
public class Wave
{
    [SerializeField] public List<WaveCycle> waveCycles = new List<WaveCycle>();
}

[System.Serializable]
public class WaveCycle
{
    [SerializeField] public EnemyType enemyType;
    [SerializeField] public int enemyAmount;
    [SerializeField] public float waitTime;
}

public class WaveManager : MonoBehaviour
{
    private int currentEnemyCount = 0;
    public int CurrentEnemyCount
    {
        get => currentEnemyCount;
        set
        {
            currentEnemyCount = value;
            currentEnemyText.text = currentEnemyCount + " 남음";

			if (CurrentEnemyCount == 0)
			{
				NextWave();
			}
		}
    }
    private int enemyCntInCurrentWave; // 현재 웨이브에 스폰된 적 수
    public int enemyCntUntilNow = 0; // 총 스폰된 모든 적 수
    [SerializeField] private List<Wave> wave = new List<Wave>();

    private int _currentWave = 0;
    public int CurrentWave
    {
        get => _currentWave;
        set
        {
            _currentWave = value;
            waveText.text = "Wave " + _currentWave;
		}
    }

    [SerializeField]
    private List<EnemyData> enemyDatas;
    [SerializeField]
    private GameObject[] enemyPrefab;
    [SerializeField]
    private Transform[] spawnPoints;
    private float currentTime = 0;
    [SerializeField]
    private float waveTime = 30f;

    public GameObject gameClearTxt;

    [SerializeField]
    private Text intermissionText;

    [SerializeField]
    private Text waveText;

    [SerializeField]
    private Text currentEnemyText;

    private void Start()
    {
        //StartCoroutine("LegacyStartWave");
        StartCoroutine(StartWave());
    }

	KeyCode skipIntermissionKey = KeyCode.Y;
    bool skipTrigger = false;
    bool isInIntermission = false;
	private void Update()
    {
        if (Input.GetKeyDown(skipIntermissionKey) && isInIntermission)
        {
            skipTrigger = true;
        }
    }
    public void NextWave()//다음 웨이브 실행
    {
        //if (waveStage >= wave.Count)
        //{
        //    gameClearTxt.SetActive(true);
        //    StartCoroutine("ReturnLobby");
        //} // 
        ///else if (currentTime >= waveTime)
        //{
        //    // StartCoroutine("LegacyStartWave");
        //    StartCoroutine(StartIntermission());
        //    currentTime = 0;
        //}
        //currentTime += Time.deltaTime;

        StartCoroutine(StartIntermission());
    }
    private IEnumerator LegacyStartWave()//웨이브 실행
    {
        for (int i = 0; i < wave[CurrentWave].waveCycles.Count; i++)
        {
            for (int j = 0; j < wave[CurrentWave].waveCycles[i].enemyAmount; j++)
            {
                var enemy = SpawnEnemy(wave[CurrentWave].waveCycles[i].enemyType);
            }
            yield return new WaitForSeconds(wave[CurrentWave].waveCycles[i].waitTime);
        }
    }

    private IEnumerator StartWave()
    {
        CurrentWave += 1;
        enemyCntInCurrentWave += UnityEngine.Random.Range(2, 4);
        enemyCntUntilNow += enemyCntInCurrentWave;

        for (int i = 0; i < enemyCntInCurrentWave; i++)
        {
            EnemyType randomType = (EnemyType)UnityEngine.Random.Range(0, 2);
            SpawnEnemy(randomType);
        }
        yield return null;
    }

    public Enemy SpawnEnemy(EnemyType type)//적 생성
    {
        print(type + "생성");
        int spawnNum = UnityEngine.Random.Range(0, spawnPoints.Length);
        var newEnemy = Instantiate(enemyPrefab[(int)type], spawnPoints[spawnNum]).GetComponent<Enemy>();
        newEnemy.EnemyData = enemyDatas[(int)type];
        CurrentEnemyCount++;
        return newEnemy;
    }

    public void EnableIntermission()
    {
        
        intermissionText.gameObject.SetActive(true);
    }

    public void DisableIntermission()
    {
       
        intermissionText.gameObject.SetActive(false);
    }

    public float intermissionTime;
    
    public IEnumerator StartIntermission()
    {
		isInIntermission = true;
		EnableIntermission();
        float elapsedTime = intermissionTime;

        while(elapsedTime > 0f)
        {
            if (skipTrigger)
            {
                skipTrigger = false;
                break;
			}
            intermissionText.text = "준비 시간: " + (int)elapsedTime + $"\n스킵하려면 {skipIntermissionKey}를 누르세요.";
            elapsedTime -= Time.deltaTime;
            yield return null;
        }
		isInIntermission = false;

		StartCoroutine(StartWave());
		intermissionText.text = "웨이브 " + CurrentWave + " 시작";

        yield return new WaitForSeconds(3f);
		DisableIntermission();
	}
}