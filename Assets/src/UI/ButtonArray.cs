using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ButtonArray : MonoBehaviour
{
	[SerializeField]
	Button[] buttons;
	Image[] icons;

	public IntEvent EventButtonPressed = new IntEvent();

	// Use this for initialization
	void Awake()
	{
		icons = new Image[buttons.Length];
		for(int i = 0; i < buttons.Length; i++)
		{
			icons[i] = buttons[i].GetComponentsInChildren<Image>()[1];
			icons[i].gameObject.SetActive(false);

			int local = i;
			UnityAction observer = () => EventButtonPressed.Invoke(local);
			buttons[i].onClick.AddListener(observer);
		}
		
	}

	public void SetIcon(int index, Sprite icon)
	{
		if (index < icons.Length)
		{
			icons[index].gameObject.SetActive(icon != null);
			icons[index].sprite = icon;
		}
		else
			Debug.LogError("Index " + index + " not supported in " + name + "<ButtonArray>");
	}

	public void ClearIcons()
	{
		for (int i = 0; i < icons.Length; i++)
			icons[i].gameObject.SetActive(false);
	}

	[System.Serializable]
	public class IntEvent : UnityEvent<int> { }

}
