using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIState
{
	public delegate void StateChanged(AIState state);

	protected NavMeshAgent agent;
	protected Animator animator;

	protected StateChanged _stateChangedCallback;

	protected AIState(NavMeshAgent agent, Animator anim, StateChanged callback)
	{
		this.agent = agent;
		animator = anim;
		_stateChangedCallback = callback;
	}

	protected virtual void OnEnterState() { }
	protected virtual void OnExitState() { }
	public virtual void Update(float deltaTime) { }
	// pressed 
	public virtual void Input(KeyCode key, bool pressed) { }

	protected virtual void TransitionStates(AIState state)
	{
		OnExitState();
		_stateChangedCallback(state);
		state.OnEnterState();
	}

	/// <summary>
	/// Update path to target and move towards it.
	/// </summary>
	/// <param name="target"></param>
	/// <returns>True if path valid, false otherwise</returns>
	protected bool FindPath(Vector3 target)
	{
		NavMeshPath path = new();
		agent.CalculatePath(target, path);

		if (path.status != NavMeshPathStatus.PathComplete)
		{
			agent.isStopped = true;
		}

		agent.path = path;
		agent.SetPath(path);

		agent.isStopped = false;

		return path.status == NavMeshPathStatus.PathComplete;
	}

	protected bool PathCompleted()
	{
		if (!agent.pathPending)
		{
			if (agent.remainingDistance <= agent.stoppingDistance)
			{
				return !agent.hasPath || agent.velocity.sqrMagnitude == 0f;
			}
		}

		return false;
	}

	public static void SetState(AIState state)
	{
		state.OnEnterState();
	}
}