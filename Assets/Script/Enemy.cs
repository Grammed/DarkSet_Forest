using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Animator attackAni;
    public Sun_Rotation sun_rotation;
    [SerializeField]
    private GameTime gameTime;
    public NavMeshAgent m_enemy;
    Transform playerPos;
    Transform towerPos;
    RaycastHit hit;
    [SerializeField]
    Transform rayPosition;
    [SerializeField]
    private EnemyData enemyData;
    private float enemyHp;
    public EnemyData EnemyData { set { enemyData = value; } }
    private Shop_Manager shopManager;
    [SerializeField]
    private int killValue;//킬했을 때 플레이어에게 들어갈 돈
    private Bunker bunker;
    [SerializeField]
    private WaveManager waveManager;

    private MoneyManager moneyManager;


    //초기화
    private void Start()
    {
        attackAni = GetComponent<Animator>();
        playerPos = GameObject.Find("Player").transform;
        towerPos = GameObject.Find("Bunker").transform;
        m_enemy = GetComponent<NavMeshAgent>();
        bunker = GameObject.Find("Bunker").GetComponent<Bunker>();
        m_enemy.speed = enemyData.MoveSpeed;
        moneyManager = GameObject.Find("MoneyManager").GetComponent<MoneyManager>();
        enemyHp = enemyData.Hp;

        if (waveManager == null)
        {
            waveManager = FindAnyObjectByType<WaveManager>();
        }
	}

    // Update is called once per frame
    private void Update()
    {

        SetTarget();
        FwdObject();
        Debug.DrawRay(rayPosition.transform.position, transform.forward * enemyData.Range, Color.red);
        if(enemyHp <= 0)
        {
            Dead();
        }
    }
    public void SetTarget()//몬스터와 플레이어의 거리가 가깝다면 플레이어를 따라옴 아니라면 타워로 감
    {
        if (Vector3.Distance(transform.position, playerPos.position) <= enemyData.SightRange)
        {
            m_enemy.SetDestination(playerPos.position);
        }
        else
        {
            m_enemy.SetDestination(towerPos.position);
        }
    }
    public void GetDamage(float damage)
    {
        enemyHp= enemyHp - damage;
    }
    private void FwdObject()//앞에 플레이어가 있는지 확인
    {
        var enemyRay = Physics.Raycast(rayPosition.transform.position, transform.forward, out hit, enemyData.Range);
        if (enemyRay)
        {
            if (hit.collider.gameObject.CompareTag("Player") || hit.collider.gameObject.CompareTag("Bunker"))
            {
                //사거리안에 플레이어가 계속 있을경우, 가만히 공격함(원거리나 근거리)
                if (Vector3.Distance(transform.position, playerPos.position) <= enemyData.Range)
                {
                    StartCoroutine(Attack());
                } 
       
            }
            else
            {
                StopCoroutine(Attack());
            }
        }
    }

    public virtual IEnumerator Attack()//공격
    {
        yield return null;
    }

    private void Dead()
    {
        waveManager.enemyCount--;
        moneyManager.Coin += killValue;
        Destroy(gameObject);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Bunker"))
        {
            bunker.bunkerHp -= enemyData.Damage;
        }
    }
}