using UnityEngine;
using System.Collections;

/// <summary>
/// Continously streches a vertical object to fit between two objects.
/// </summary>

public class StrechBetweenTwoObjects : MonoBehaviour
{
	public Transform target;
	public Transform origin;

	// Update is called once per frame
	protected void LateUpdate ()
	{
		if(target && origin)
		{
			transform.position = (target.position + origin.position) / 2f;
			transform.localScale = new Vector3((target.position - origin.position).magnitude, 1f, 1f);
			transform.right = target.position - origin.position;
		}
	}
}
