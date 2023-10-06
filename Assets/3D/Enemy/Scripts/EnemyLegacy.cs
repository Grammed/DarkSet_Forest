using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <c> READ BEFORE BLAME </c>
/// <summary> �׽�Ʈ�� ��. �����̰� ���� �ɷ� "��ü ����" </summary>
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
		// ü�� ������ ����
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

	/// <summary> �� � �ǰݴ��� </summary>
	/// <param name="gunDmg"></param>
	public void Hit(float gunDmg)
	{
		health -= gunDmg;
		print("Enemy hit! Its health is now " + health);
	}

	private void OnTriggerEnter(Collider other)
	{
		// ���� ����
		if (canAttack)
		{
			StartCoroutine(AttackDelay());
		}
	}

	// ���� ������
	IEnumerator AttackDelay()
	{
		canAttack = false;
		yield return new WaitForSeconds(attackTime);
		canAttack = true;
	}
}
