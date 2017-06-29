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
		AbilityWeapon weapon = attacker.GetComponentInChildren<AbilityWeapon>();
		int minDamage = 0, maxDamage = 0;

		if (weapon.damageType == DamageType.magic)
		{
			minDamage = 2;
			maxDamage = weapon.diceSize * 2;
		}
		else
		{
			int sides = AbilityWeapon.DiceVS(weapon.damageType, target.Attributes.armorType);
			minDamage = sides + attacker.Attributes.might - target.Attributes.armor;
			maxDamage = sides * weapon.diceSize + attacker.Attributes.might - target.Attributes.armor;
			
		}

		if (minDamage < 0) minDamage = 0;
		if (maxDamage < 0) maxDamage = 0;
		display.text = "HP " + target.GetComponent<HitPoints>().CurrentHealth + "\n" + minDamage + "-" + maxDamage;
	}
}
