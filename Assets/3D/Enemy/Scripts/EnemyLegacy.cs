using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <c> READ BEFORE BLAME </c>
/// <summary> 테스트용 적. 석원이가 만든 걸로 "교체 가능" </summary>
public class EnemyLegacy: MonoBehaviour
{
    [SerializeField]
    private float maxHealth;
	[SerializeField]
    private float health;

	private bool canAttack = true;

	[SerializeField]
	private float attackTime = 2f;
	[SerializeField]
	private float attackDmg;
	private PlayerController player;

	private void Start()
	{
		health = maxHealth;
		player = FindAnyObjectByType<PlayerController>();

	}

	private void Update()
	{
		// 체력 없으면 죽음
		if (health <= 0f)
		{
			gameObject.SetActive(false);
			//Destroy(gameObject);
			Invoke(nameof(Respawn), 10f);
		}
	}

	void Respawn()
	{
		health = maxHealth;
		gameObject.SetActive(true);
	}

	/// <summary> 총 등에 피격당함 </summary>
	/// <param name="gunDmg"></param>
	public void Hit(float gunDmg)
	{
		health -= gunDmg;
		print("Enemy hit! Its health is now " + health);
	}

	private void OnTriggerEnter(Collider other)
	{
		// 공격 수행
		if (canAttack)
		{
			StartCoroutine(AttackDelay());
		}
	}

	// 공격 딜레이
	IEnumerator AttackDelay()
	{
		canAttack = false;
		yield return new WaitForSeconds(attackTime);
		canAttack = true;
	}
}
