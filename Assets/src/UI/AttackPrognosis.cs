using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackPrognosis : MonoBehaviour
{
	[SerializeField]
	Text display;

	public void Show(Character attacker, Character target)
	{
		int hitChance = Mathf.RoundToInt(100f * attacker.Attributes.HitVS(target.Attributes));
		int minDamage = attacker.Attributes.MinDamageVersus(target.Attributes);
		int maxDamage = attacker.Attributes.MaxDamageVersus(target.Attributes);

		display.text = minDamage + "-" + maxDamage + " (" + hitChance + "%)";
	}
}
