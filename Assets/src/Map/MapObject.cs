using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System;

public class MapObject : NetworkBehaviour
{
	[SyncVar(hook = "Put")]
	IntVector2 location;

	protected Action<IntVector2> AnimateMove;

	virtual public IntVector2 Location
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

	protected void Put(IntVector2 iv2)
	{
		if (AnimateMove == null) transform.position = (Vector3)iv2;
		else AnimateMove(iv2);
		location = iv2;
	}
}
