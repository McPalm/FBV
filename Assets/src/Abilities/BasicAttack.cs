﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BasicAttack : AAbility
{
	public override bool Useable
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override void Use(IntVector2 target)
	{
		GameObject o = GameObjectAt(target);
		// reduce health at the place
		if (o)
		{
			o.GetComponent<HitPoints>().Hurt(UnityEngine.Random.Range(3, 6));
			RpcClientCode(o);
		}
	}

	public override bool UseableAt(IntVector2 target)
	{
		Mobile m = GetComponentInParent<Mobile>();

		if((m.Location - target).MagnitudeMax == 1)
		{
			// in melee range
			GameObject o = GameObjectAt(target);
			return o && o.GetComponent<HitPoints>();
		}
		return false;
	}

	[ClientRpc]
	private void RpcClientCode(GameObject target)
	{
		// animate
		Mobile m = GetComponentInParent<Mobile>();
		StartCoroutine(Animation(m, target));
	}

	private IEnumerator Animation(Mobile m, GameObject target)
	{
		Vector2 midpoint = ((Vector3)m.Location + target.transform.position) / 2;
		m.transform.position = midpoint;
		for(float t = 0; t < 1f; t += Time.deltaTime * 3f)
		{
			m.transform.position = Vector2.Lerp(midpoint, (Vector3)m.Location, t);
			yield return null; // wait for next frame.
		}
		m.transform.position = (Vector3)m.Location;
	}

	public override IntVector2[] TargetableTiles()
	{
		transform.localPosition = Vector3.zero;
		IntVector2 orig = IntVector2.RoundFrom(transform.position);
		IntVector2[] r =
			{
			orig + IntVector2.left,
			orig + IntVector2.up,
			orig + IntVector2.right,
			orig + IntVector2.down,

			orig + IntVector2.upleft,
			orig + IntVector2.upright,
			orig + IntVector2.downleft,
			orig + IntVector2.downright,
			};
		return r;
		
	}
}
