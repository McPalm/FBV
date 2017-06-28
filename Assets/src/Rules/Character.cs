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
		GetComponent<MovementRules>().speed = attributes.speed;
		GetComponent<HitPoints>().MaxHealth = attributes.hp;

		UnityEngine.Events.UnityAction<int> hurt = (int n) =>
		{
			PrintCombatMessage("-" + n.ToString(), new Color(0.8f, 0f, 0.2f));
		};
		UnityEngine.Events.UnityAction<int> heal = (int n) =>
		{
			PrintCombatMessage("+" + n.ToString(), new Color(0f, 0.8f, 0.2f));
		};

		GetComponent<HitPoints>().EventHurt.AddListener(hurt);
		GetComponent<HitPoints>().EventHeal.AddListener(heal);
	}

	public bool RollHit(GameObject o)
	{
		//Character c = o.GetComponent<Character>();
		//if(c)
		//	return Random.value < attributes.HitVS(c.attributes);
		return true;
	}

	public int RollDamage(GameObject o, bool spell = false)
	{
		/*
		Character c = o.GetComponent<Character>();
		if (c)
			if(spell)
				return Mathf.RoundToInt((Random.Range(1, attributes.damageRoll + 1) + attributes.might) * c.attributes.ResistanceDamageFactor);
			else
				return Mathf.RoundToInt((Random.Range(1, attributes.damageRoll + 1) + attributes.might) * c.attributes.DefenceDamageFactor);
		return (Random.Range(0, attributes.damageRoll + 1) + attributes.might);
		*/
		return 5; // TODO
	}

	public void PrintCombatMessage(string m, Color c)
	{
		CombatTextPool.Instance.PrintAt(transform.position + new Vector3(0f, 0.65f), m, c, 1f + m.Length * 0.07f);
	}
}
