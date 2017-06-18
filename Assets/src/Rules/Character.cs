using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * A lil class to put on an object to make it a character.
 * includes all the things you need
 */

[RequireComponent(typeof(Mobile), typeof(HitPoints), typeof(Draggable))]
[RequireComponent(typeof(DeathEffect), typeof(MovementRules), typeof(SortRenderingOrder))]
public class Character : MonoBehaviour
{
	[SerializeField]
	Attributes attributes;

	public Attributes Attributes
	{
		get
		{
			return attributes;
		}
		set
		{
			attributes = value;
		}
	}

	// Use this for initialization
	void Start ()
	{
		GetComponent<MovementRules>().speed = attributes.movement;
		GetComponent<HitPoints>().MaxHealth = attributes.hp;
	}

	public bool RollHit(GameObject o)
	{
		Character c = o.GetComponent<Character>();
		if(c)
			return Random.value < attributes.HitVS(c.attributes);
		return true;
	}

	public int RollDamage(GameObject o, bool spell = false)
	{
		Character c = o.GetComponent<Character>();
		if (c)
			if(spell)
				return Mathf.RoundToInt((Random.Range(0, attributes.damageRoll + 1) + attributes.damageBonus) * c.attributes.ResistanceDamageFactor);
			else
				return Mathf.RoundToInt((Random.Range(0, attributes.damageRoll + 1) + attributes.damageBonus) * c.attributes.DefenceDamageFactor);
		return (Random.Range(0, attributes.damageRoll + 1) + attributes.damageBonus);

	}
}
