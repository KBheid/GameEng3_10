using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AISpawner : MonoBehaviour
{
    public GameObject AIPrefab;

    public void Initialize()
	{

		GameObject go = Instantiate(AIPrefab, transform.parent);
		if (go.TryGetComponent(out NavMeshAgent agent))
		{
			agent.updatePosition = false;
			go.transform.position = transform.position;

			StartCoroutine(nameof(UpdatePositionNextFrame), agent);
		}

		else
		{
			go.transform.position = transform.position;
		}
	}

	private IEnumerator UpdatePositionNextFrame(NavMeshAgent agent)
	{
		yield return new WaitForEndOfFrame();
		agent.updatePosition = true;
	}
}
