using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;
	[SerializeField]
    private float health;

	private void Start()
	{
		health = maxHealth;
	}

	private void Update()
	{
		if (health <= 0f)
		{
			Destroy(gameObject);
		}
	}

	public void Hit(float damage)
	{
		health -= damage;
		print("Enemy hit! Its health is now " + health);
	}
}
