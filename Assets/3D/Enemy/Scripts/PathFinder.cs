using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 테스트용 PathFinder
/// </summary>
public class PathFinder : MonoBehaviour
{
	public GameObject target;
	NavMeshAgent agent;

	private void Awake()
	{
		target = GameObject.Find("Player");
	}
	void Update()
	{
		agent = GetComponent<NavMeshAgent>();
		agent.SetDestination(target.transform.position);
	}
}
