using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A rather superficial class that signifies that a Mobile can be dragged around by a player.
/// </summary>

[RequireComponent(typeof(Mobile), typeof(CircleCollider2D))]
public class Draggable : MonoBehaviour
{
	[SerializeField]
	bool owned = true;
	Mobile mobile;

	public bool Owned
	{
		get
		{
			return owned;
		}

		set
		{
			owned = value;
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
