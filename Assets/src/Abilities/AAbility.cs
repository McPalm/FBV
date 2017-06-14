using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

abstract public class AAbility : NetworkBehaviour
{
	public abstract void Use(IntVector2 target);

	public abstract bool Useable { get; }

	public abstract bool UseableAt(IntVector2 target);

	protected GameObject GameObjectAt(IntVector2 location)
	{
		RaycastHit2D hit = Physics2D.Raycast((Vector2)location, Vector2.zero);
		if (hit.collider)
			return hit.collider.gameObject;
		return null;
	}
}
