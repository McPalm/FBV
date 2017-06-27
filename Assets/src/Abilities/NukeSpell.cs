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
			if (tile == iv2) continue;
			if ((tile - iv2).MagnitudePF <= range)
				if(Obstructions.HasLOS(tile, iv2))
					l.Add(iv2);
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
			int dmg = c.RollDamage(o, true);
			if (hit) o.GetComponent<HitPoints>().Hurt(dmg);
			RpcClientCode(o, hit);
		}
	}

	public override bool UseableAt(IntVector2 target)
	{
		if (target == IntVector2.RoundFrom(transform.position)) return false;
		if((GetComponentInParent<Mobile>().Location - target).MagnitudePF <= range)
		{
			if (Obstructions.HasLOS(IntVector2.RoundFrom(transform.position), target))
			{
				GameObject o = GameObjectAt(target);
				return o && o.GetComponent<HitPoints>();
			}
		}
		return false;
	}

	[ClientRpc]
	public void RpcClientCode(GameObject target, bool hit)
	{
		if (hit) EventHit.Invoke(target);
		else EventMiss.Invoke(target);
	}

	[Serializable]
	public class GameObjectEvent : UnityEvent<GameObject> { }
}
