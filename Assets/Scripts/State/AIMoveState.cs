using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMoveState : AIState
{
	private GameObject player;
	private Renderer renderer;

	private float waitAtArrival = 2f;
	private float timeWaited = 0f;

	public AIMoveState(NavMeshAgent agent, Animator anim, StateChanged callback) : base(agent, anim, callback)
	{
		player = Object.FindObjectOfType<PlayerMovement>().gameObject;
		renderer = agent.GetComponent<Renderer>();
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);

		if (renderer.isVisible)
		{
			TransitionStates(new StareState(agent, animator, _stateChangedCallback));
			return;
		}

		// If at path, try to find a new path. If fails, wait for a bit and try again
		if (timeWaited != 0f || PathCompleted())
		{
			agent.isStopped = true;
			timeWaited += deltaTime;

			if (timeWaited >= waitAtArrival)
			{
				if (FindPath(GetRandomLocationNearby()))
				{
					timeWaited = 0f;
					agent.isStopped = false;
				}
			}
		}
	}

	protected override void OnEnterState()
	{
		base.OnEnterState();
		//FindPathGuarenteed();
		FindPath(GetRandomLocationNearby());
	}

	void FindPathGuarenteed()
	{
		while (!FindPath(GetRandomLocationNearby()));
	}

	Vector3 GetRandomLocationNearby()
	{
		return agent.transform.position + 
			new Vector3(Random.Range(-1f, 1f), Random.Range(-1.85f, 2f), Random.Range(-1f, 1f));
	}

}
