using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Put this on a UnityEngine.UI.Button to assign a hotkey to that button.
/// </summary>

public class HotKey : MonoBehaviour
{
	public string key;

	void Update ()
	{
		if (Input.GetButtonDown(key))
		{
			GetComponent<Button>().onClick.Invoke();
		}
	}
}
