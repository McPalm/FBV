using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPreviewUI : MonoBehaviour
{
	[SerializeField]
	AttackPrognosis attackPrognosis;
	[SerializeField]
	WorldToScreen followTarget;

	Character user;
	Character lastTarget;

	public void SelectCharacter(GameObject o)
	{
		if(o)
		{
			user = o.GetComponent<Character>();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		// check mouseover character
		Character c = AController.ComponentUnderMouse<Character>();
		if(c && c != user)
		{
			if (lastTarget != c)
			{
				lastTarget = c;
				attackPrognosis.gameObject.SetActive(true);
				attackPrognosis.Show(user, c);
				followTarget.Follow(c.transform);
			}
		}
		else
		{
			attackPrognosis.gameObject.SetActive(false);
			lastTarget = null;
		}
			
	}

	void OnDisable()
	{
		attackPrognosis.gameObject.SetActive(false);
	}
}
