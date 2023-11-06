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

    public PlayerController playerController;
    bool canAttack = true;


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
        playerController = FindObjectOfType<PlayerController>();

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
        if (damage <= 0f)
        {
            print("damage under zero");
            return;
        }
        enemyHp -= damage;
        print(enemyHp);
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

    private IEnumerator Attack()//공격
    {
        attackAni.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);
        SetTarget();
    }

    private void Dead()
    {
        waveManager.CurrentEnemyCount--;
        moneyManager.Coin += killValue;

        GameManager.Instance.killedEnemy += 1;
        // playerController.enemySound.Play();
        Destroy(gameObject);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Bunker"))
        {
            if (canAttack)
            {
				bunker.bunkerHp -= enemyData.Damage;
                AfterAttack();
			}
        }
        if (collision.collider.CompareTag("Player"))
        {
            if (canAttack)
            {
				DoDamage(enemyData.Damage / 100f);
                AfterAttack();
			}
        }
    }
    private void DoDamage(float damage)
    {
        playerController.Hit(damage);
    }

    float attackDelay = 1.5f;
    IEnumerator DelayDamage()
    {
        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
    }

    void AfterAttack()
    {
        canAttack = false;
        StartCoroutine(DelayDamage());
    }
}