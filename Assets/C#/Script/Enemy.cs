using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public Animator attackAni;
    public Sun_Rotation sun_rotation;
    [SerializeField]
    private GameTime gameTime;
    public NavMeshAgent m_enemy;
    protected Transform playerPos;
    Transform towerPos;
    RaycastHit hit;
    [SerializeField]
    Transform rayPosition;
    [SerializeField]
    protected EnemyData enemyData;
    public float enemyHp;
    public EnemyData EnemyData { set { enemyData = value; } }
    private Shop_Manager shopManager;
    [SerializeField]
    private int killValue;//ų���� �� �÷��̾�� �� ��
    private Bunker bunker;

    private WaveManager waveManager;

    private MoneyManager moneyManager;

    public PlayerController playerController;
    bool canAttack = true;

    protected bool isThrowing = false;

    EnemyHealer healer;

    //�ʱ�ȭ
    private void Start()
    {
        enemyHp = enemyData.Hp;
        //healer = GameObject.FindAnyObjectByType<EnemyHealer>();
        attackAni = GetComponent<Animator>();
        playerPos = GameObject.Find("Player").transform;
        towerPos = GameObject.Find("Bunker").transform;
        m_enemy = GetComponent<NavMeshAgent>();
        bunker = GameObject.Find("Bunker").GetComponent<Bunker>();
        m_enemy.speed = enemyData.MoveSpeed;
        moneyManager = GameObject.Find("MoneyManager").GetComponent<MoneyManager>();
        playerController = FindObjectOfType<PlayerController>();
        //StartCoroutine(healer.Heal());  
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
    public void SetTarget()//���Ϳ� �÷��̾��� �Ÿ��� �����ٸ� �÷��̾ ����� �ƴ϶�� Ÿ���� ��
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
    private void FwdObject()//�տ� �÷��̾ �ִ��� Ȯ��
    {
        var enemyRay = Physics.Raycast(rayPosition.transform.position, transform.forward, out hit, enemyData.Range);
        if (enemyRay)
        {
            if (Vector3.Distance(transform.position, playerPos.position) <= enemyData.Range && !isThrowing)
            {
                if (hit.collider.gameObject.CompareTag("Player") || hit.collider.gameObject.CompareTag("Bunker"))
                {
                    //��Ÿ��ȿ� �÷��̾ ��� �������, ������ ������(���Ÿ��� �ٰŸ�)

                    StartCoroutine(Attack());
                }
            }
            else
            {
                StopCoroutine(Attack());
            }
        }
    }

    public virtual IEnumerator Attack()//����
    {
        yield return null;
    }

    private void Dead()
    {
        waveManager.CurrentEnemyCount--;
        moneyManager.Coin += killValue;

        GameManager.Instance.killedEnemy += 1;
        // playerController.enemySound.Play();
        Destroy(gameObject);
    }

    public void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Bunker") && enemyData.name == "Gangbu")
        {
            if (canAttack)
            {
				bunker.nowBunkerHp -= enemyData.Damage * 3;
                AfterAttack();
			}
        }
        else if (collision.collider.CompareTag("Bunker") && enemyData.name != "Gangbu")
        {
            if (canAttack)
            {
                bunker.nowBunkerHp -= enemyData.Damage;
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