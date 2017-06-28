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
			// show(c.Attributes, c.GetComponent<HitPoints>().CurrentHealth);
			string cha = string.Format("HP {0}/{1}\t   Might {2}\nArmor {3} ({4})\t   Speed {5}",
				c.GetComponent<HitPoints>().CurrentHealth, c.Attributes.hp, c.Attributes.might, c.Attributes.armor, c.Attributes.armorType, c.Attributes.speed);
			string wpn = c.GetComponentInChildren<AbilityWeapon>().Description;
			summary.text = c.name + "\n" + cha + "\n\n" + wpn;
		}
	}

	public void Hide()
	{
		summary.text = "";
	}
}
