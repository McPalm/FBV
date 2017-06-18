using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A rather superficial class that signifies that a Mobile can be dragged around by a player.
/// </summary>

[RequireComponent(typeof(Mobile), typeof(CircleCollider2D))]
public class Draggable : MonoBehaviour
{
	Mobile mobile;

	public bool Moveable
	{
		get
		{
			TurnEntry t = GetComponent<TurnEntry>();
			if (t && t.Acted) return false;
			if (!TurnTracker.Instance.IsMyTurn(gameObject)) return false;
			return ClientRoster.Local.Contains(gameObject);
		}
	}

	public Mobile Mobile
	{
		get
		{
			if (!mobile) mobile = GetComponent<Mobile>();
			return mobile;
		}
	}
}
