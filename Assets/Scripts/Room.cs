using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class Room : MonoBehaviour
{
    public List<Transform> exitsOrEntries;
	public List<AISpawner> spawners;
	public List<NavMeshSurface> additionalSurfaces;
	public List<NavMeshLink> additionalLinks;

	/// <summary>
	/// Sets the navmesh for the room. Called after positioned.
	/// </summary>
    public void Initialize()
	{
		TryGetComponent(out NavMeshSurface surface);
		surface.BuildNavMesh();

		StartCoroutine(nameof(SpawnEnemies));
		StartCoroutine(nameof(InitSubSurfaces));
	}

	IEnumerator SpawnEnemies()
	{
		foreach (AISpawner spawner in spawners)
		{
			yield return new WaitForEndOfFrame();
			spawner.Initialize();
		}
	}

	IEnumerator InitSubSurfaces()
	{
		foreach (NavMeshSurface surface in additionalSurfaces)
		{
			yield return new WaitForEndOfFrame();
			surface.BuildNavMesh();
		}

		InitLinks();
	}

	void InitLinks()
	{
		foreach (Transform t in exitsOrEntries)
		{
			if (t.gameObject.TryGetComponent(out NavMeshLink link))
			{
				link.UpdateLink();
			}
		}

		foreach (NavMeshLink link in additionalLinks)
		{
			link.UpdateLink();
		}
	}

	private void OnDestroy()
	{
		foreach (Transform t in exitsOrEntries)
		{
			if (t.gameObject.TryGetComponent(out NavMeshLink link))
			{
				NavMesh.RemoveLink(link.m_LinkInstance);
			}
		}
	}
}
