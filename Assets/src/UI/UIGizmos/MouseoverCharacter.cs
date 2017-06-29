using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseoverCharacter : MonoBehaviour
{
	public CharacterEvent EventOnMouseOver = new CharacterEvent();
	public UnityEvent EventLeaveMouseOver = new UnityEvent();

	Character lastMouseOver;

	void LateUpdate()
	{
		Character c = AController.ComponentUnderMouse<Character>();
		if(c != lastMouseOver)
		{
			if (c)
				EventOnMouseOver.Invoke(c);
			else
				EventLeaveMouseOver.Invoke();
			lastMouseOver = c;
		}
	}

	void OnDisable()
	{
		lastMouseOver = null;
	}

	[System.Serializable]
	public class CharacterEvent : UnityEvent<Character> { }
}
