using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
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
		if (health <= 0f)
		{
			Destroy(gameObject);
		}
	}

	public void Hit(float gunDmg)
	{
		health -= gunDmg;
		print("Enemy hit! Its health is now " + health);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (canAttack)
		{
			StartCoroutine(AttackDelay());
			
		}
	}

	IEnumerator AttackDelay()
	{
		canAttack = false;
		yield return new WaitForSeconds(attackTime);
		canAttack = true;
	}
}
