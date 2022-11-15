using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideRenderOnEnable : MonoBehaviour
{
	private void OnEnable()
	{
		TryGetComponent(out Renderer r);
		r.enabled = false;

		foreach (Transform t in transform)
		{
			if (t.TryGetComponent(out Renderer r2))
			{
				r2.enabled = false;
			}
		}
	}
}
