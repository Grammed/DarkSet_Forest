using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EnemyType
{//���� ����
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
    public int currentEnemyCnt = 0; // ���� �� ��
    private int enemyCntInCurrentWave; // ���� ���̺꿡 ������ �� ��
    public int enemyCntUntilNow = 0; // �� ������ ��� �� ��
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
    public void NextWave()//���� ���̺� ����
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
    private IEnumerator LegacyStartWave()//���̺� ����
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

    public Enemy SpawnEnemy(EnemyType type)//�� ����
    {
        print(type + "����");
        int spawnNum = UnityEngine.Random.Range(0, spawnPoints.Length);
        var newEnemy = Instantiate(enemyPrefab[(int)type], spawnPoints[spawnNum]).GetComponent<Enemy>();
        newEnemy.EnemyData = enemyDatas[(int)type];
        currentEnemyCnt++;
        return newEnemy;
    }
}