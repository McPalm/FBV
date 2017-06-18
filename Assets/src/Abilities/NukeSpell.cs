using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public class NukeSpell : AAbility
{
	public GameObjectEvent EventHit = new GameObjectEvent();
	public GameObjectEvent EventMiss = new GameObjectEvent();

	public int range = 3;

	public override bool Useable
	{
		get
		{
			return true;
		}
	}

	public override IntVector2[] TargetableTiles()
	{
		transform.localPosition = Vector3.zero;
		IntVector2 tile = IntVector2.RoundFrom(transform.position);
		List<IntVector2> l = new List<IntVector2>();
		foreach(IntVector2 iv2 in IntVector2Utility.GetRect(tile - new IntVector2(range, range), tile + new IntVector2(range, range)))
		{
			if ((tile - iv2).MagnitudePF <= range)
				l.Add(iv2);
			// TODO, line of sight?
		}
		return l.ToArray();
	}

	public override void Use(IntVector2 target)
	{
		GameObject o = GameObjectAt(target);
		// reduce health at the place
		if (o)
		{
			Character c = GetComponentInParent<Character>();
			// Character t = o.GetComponent<Character>();
			bool hit = c.RollHit(o);
			int dmg = c.RollDamage(o);
			if (hit) o.GetComponent<HitPoints>().Hurt(dmg);
			RpcClientCode(o, hit);
		}
	}

	public override bool UseableAt(IntVector2 target)
	{
		if((GetComponentInParent<Mobile>().Location - target).MagnitudePF <= range)
		{
			GameObject o = GameObjectAt(target);
			return o && o.GetComponent<HitPoints>();
		}
		return false;
	}

	[ClientRpc]
	public void RpcClientCode(GameObject target, bool hit)
	{
		
	}

	[Serializable]
	public class GameObjectEvent : UnityEvent<GameObject> { }
}
