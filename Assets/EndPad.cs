using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPad : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.TryGetComponent(out PlayerMovement pm))
		{
			SceneManager.LoadScene("Game");
		}
	}
}
