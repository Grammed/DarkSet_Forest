using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// �׽�Ʈ�� PathFinder
/// </summary>
public class PathFinder : MonoBehaviour
{
	public GameObject target;
	NavMeshAgent agent;

	void Update()
	{
		agent = GetComponent<NavMeshAgent>();
		agent.SetDestination(target.transform.position);
	}
}
