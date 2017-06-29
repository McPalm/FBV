using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

abstract public class AAbility : MonoBehaviour
{
	public Sprite icon;
	public string AbilityName;

	public abstract string Description { get; }

	public abstract void Use(IntVector2 target);

	public abstract bool Useable { get; }

	public abstract bool UseableAt(IntVector2 target);

	/// <summary>
	/// For the users convenience, does not have to be accurate towards what goes thourhg UseableAt, but rather it should give the players a good visualisation of the ability
	/// </summary>
	/// <returns></returns>
	public abstract IntVector2[] TargetableTiles();

	protected GameObject GameObjectAt(IntVector2 location)
	{
		RaycastHit2D hit = Physics2D.Raycast((Vector2)location, Vector2.zero);
		if (hit.collider)
			return hit.collider.gameObject;
		return null;
	}
}
