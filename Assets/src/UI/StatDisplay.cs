using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatDisplay : MonoBehaviour
{
	[SerializeField]
	Text summary;

	public void Show(GameObject o)
	{
		if(o)
		{
			Show(o.GetComponent<Character>());
		}

	}

	public void Show(Character c)
	{
		if(c)
		{
			show(c.Attributes, c.GetComponent<HitPoints>().CurrentHealth);
		}
	}

	void show(Attributes a, int hp)
	{
		summary.text =
			"HP: " + hp + "/" + a.hp + "\n" +
			"Dmg: 1d" + a.damageRoll + "+" + a.damageBonus + "\n" +
			"Hit: " + a.hit + "\n" +
			"Dodge: " + a.dodge + "\n" +
			"Defence: " + a.defence + " (" + Mathf.RoundToInt(100f - a.DefenceDamageFactor * 100f) + "%)\n" +
			"Resistance: " + a.resistance + " (" + Mathf.RoundToInt(100f - a.ResistanceDamageFactor * 100f) + "%)\n" +
			"Speed: " + a.movement;
	}

	public void Hide()
	{
		summary.text = "";
	}
}
