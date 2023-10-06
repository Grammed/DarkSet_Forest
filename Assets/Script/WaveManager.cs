using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int enemyCount = 0;
    [SerializeField] private List<Wave> wave = new List<Wave>();

    private int waveStage = 0;
    public int WaveStage => waveStage;

    [SerializeField]
    private List<EnemyData> enemyDatas;
    [SerializeField]
    private GameObject[] enemyPrefab;
    [SerializeField]
    private Transform[] spawnPoints;

    private void Start()
    {
        StartCoroutine("StartWave");
    }
	private void Update()
	{
		if (enemyCount == 0)
		{
			StartCoroutine("NextWave");
		}
	}
	public IEnumerator NextWave()//다음 웨이브 실행
    {
        yield return new WaitForSeconds(15f);
        StartCoroutine("StartWave");
        waveStage++;
    }
    private IEnumerator StartWave()//웨이브 실행
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

    public Enemy SpawnEnemy(EnemyType type)//적 생성
    {
        print(type + "생성");
        int spawnNum = Random.Range(0, spawnPoints.Length);
        var newEnemy = Instantiate(enemyPrefab[(int)type], spawnPoints[spawnNum]).GetComponent<Enemy>();
        newEnemy.EnemyData = enemyDatas[(int)type];
        enemyCount++;
        return newEnemy;
    }

   

}