using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public int currentEnemyCnt = 0; // 남은 적 수
    private int enemyCntInCurrentWave; // 현재 웨이브에 스폰된 적 수
    public int enemyCntUntilNow = 0; // 총 스폰된 모든 적 수
    [SerializeField] private List<Wave> wave = new List<Wave>();

    private int waveStage = 0;
    public int WaveStage => waveStage;

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
    private void Start()
    {
        //StartCoroutine("LegacyStartWave");
        StartCoroutine(StartWave());
    }
    private void Update()
    {
        if (currentEnemyCnt == 0)
        {
            NextWave();
        }
    }
    public void NextWave()//다음 웨이브 실행
    {
        //if (waveStage >= wave.Count)
        //{
        //    gameClearTxt.SetActive(true);
        //    StartCoroutine("ReturnLobby");
        //} // 
        /*else*/if (currentTime >= waveTime)
        {
            // StartCoroutine("LegacyStartWave");
            StartCoroutine(StartWave());
            currentTime = 0;
        }
        currentTime += Time.deltaTime;
    }
    private IEnumerator LegacyStartWave()//웨이브 실행
    {
        for (int i = 0; i < wave[waveStage].waveCycles.Count; i++)
        {
            for (int j = 0; j < wave[waveStage].waveCycles[i].enemyAmount; j++)
            {
                var enemy = SpawnEnemy(wave[waveStage].waveCycles[i].enemyType);
            }
            yield return new WaitForSeconds(wave[waveStage].waveCycles[i].waitTime);
        }
    }

    private IEnumerator StartWave()
    {
        waveStage += 1;
        enemyCntInCurrentWave += UnityEngine.Random.Range(2, 4);
        enemyCntUntilNow += enemyCntInCurrentWave;

        for (int i = 0; i < enemyCntInCurrentWave; i++)
        {
            EnemyType randomType = (EnemyType)UnityEngine.Random.Range(0, 3);
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
        currentEnemyCnt++;
        return newEnemy;
    }
}