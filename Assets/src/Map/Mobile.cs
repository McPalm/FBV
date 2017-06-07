using System;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobile : MapObject
{
	void Awake()
	{
		AnimateMove = SlideTo;
	}

	internal void MoveTo(IntVector2 destination)
	{
		Location = destination;
	}

	internal void LocalMove(IntVector2 destination)
	{
		transform.position = new Vector2(Mathf.Round(destination.x), Mathf.Round(destination.y));
	}

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
