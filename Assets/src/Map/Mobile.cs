using System;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A moveable mapobject.
/// Will slide into place rather than immediately snap into place.
/// </summary>

public class Mobile : MapObject
{
	void Awake()
	{
		AnimateMove = SlideTo;
	}

	/// <summary>
	/// Syncronizes the objects posiotion among all cline.ts
	/// Can only be called by the server.
	/// </summary>
	/// <param name="destination"></param>
	internal void MoveTo(IntVector2 destination)
	{
		if (isServer)
			Location = destination;
		else
			Debug.LogWarning("Mobile.MoveTo called by client");
	}

	/// <summary>
	/// Snaps the object to a given location locally.
	/// </summary>
	/// <param name="destination"></param>
	internal void LocalMove(IntVector2 destination)
	{
		transform.position = (Vector3)destination;
	}

	/// <summary>
	/// return the object to its actual position.
	/// </summary>
	internal void Return()
	{
		transform.position = (Vector3)Location;
	}

	private IEnumerator TweenTo(Vector3 destination, float duration)
	{
		Vector3 start = transform.position;
		float timer = 0;
		while(timer < duration)
		{
			timer += Time.deltaTime;
			transform.position = Vector3.Lerp(start, destination, timer / duration);
			yield return null;
		}
		transform.position = destination;
	}

	void SlideTo(IntVector2 iv2)
	{
		Location = iv2;
		float distance = ((Vector2)transform.position - (Vector2)iv2).magnitude;
		if (distance < 0.5f)
			transform.position = (Vector2)iv2;
		else
			StartCoroutine(TweenTo((Vector2)iv2, 0.1f + distance / 6f));
	}
}
