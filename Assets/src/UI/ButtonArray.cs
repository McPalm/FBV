using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ButtonArray : MonoBehaviour
{
	public bool useIcons = true;

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
			if (useIcons)
			{
				icons[i] = buttons[i].GetComponentsInChildren<Image>()[1];
				icons[i].gameObject.SetActive(false);
			}

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

	public void SetLabel(int index, string body)
	{
		if(index < buttons.Length)
		{
			buttons[index].GetComponentInChildren<Text>().text = body;
		}
		else
			Debug.LogError("Index " + index + " not supported in " + name + "<ButtonArray>");
	}

	public void ClearIcons()
	{
		for (int i = 0; i < icons.Length; i++)
			icons[i].gameObject.SetActive(false);
	}

	public void SetAllLabels(string body = "")
	{
		for (int i = 0; i < buttons.Length; i++)
			buttons[i].GetComponentInChildren<Text>().text = body;
	}

	[System.Serializable]
	public class IntEvent : UnityEvent<int> { }

}
