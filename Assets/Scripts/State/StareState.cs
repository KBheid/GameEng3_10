using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StareState : AIState
{
	Renderer renderer;

	public StareState(NavMeshAgent agent, Animator anim, StateChanged callback) : base(agent, anim, callback)
	{
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);

		if (!renderer.isVisible)
		{
			TransitionStates(new AIMoveState(agent, animator, _stateChangedCallback));
		}
	}

	protected override void OnEnterState()
	{
		base.OnEnterState();

		renderer = agent.GetComponent<Renderer>();
		agent.isStopped = true;
	}

}
