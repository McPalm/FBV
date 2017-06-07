using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System;

/// <summary>
/// An object that exsist on a location on the map.
/// And syncronizes their position across all clients if the Location property is accessed.
/// MapObjects are not supposed to be moved after spawned, for that use Mobile
/// </summary>

public class MapObject : NetworkBehaviour
{
	[SyncVar(hook = "Put")]
	IntVector2 location;

	/// <summary>
	/// Even tho a MapObject should not be moved, the code for syncronizing their position all persist in here.
	/// If moved, AnimateMove will be called, and is expected to move the gameobject to the given location.
	/// If AnimateMove is null, the object will simply snap into position instantly.
	/// </summary>
	protected Action<IntVector2> AnimateMove;

	protected void Start()
	{
		location = IntVector2.RoundFrom(transform.position);
	}

	/// <summary>
	/// The location of the object.
	/// Can only be properly assigned by the server.
	/// </summary>
	public IntVector2 Location
	{
		get
		{
			return location;
		}
		protected set
		{
			location = value;
		}
	}

	void Put(IntVector2 iv2)
	{
		if (AnimateMove == null) transform.position = (Vector3)iv2;
		else AnimateMove(iv2);
		location = iv2;
	}
}
