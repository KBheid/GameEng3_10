using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Chaser : MonoBehaviour
{
	AIState state;
	
	private void Start()
	{
		state = new AIMoveState(GetComponent<NavMeshAgent>(), null, SetState);
		AIState.SetState(state);
	}

	private void Update()
	{
		state.Update(Time.deltaTime);
	}

	private void SetState(AIState state)
	{
		this.state = state;
	}
}
