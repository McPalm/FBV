using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class TileOverlay : MonoBehaviour
{
	List<SpriteRenderer> pool = new List<SpriteRenderer>();
	Color color = Color.white;

	[SerializeField]
	SpriteRenderer prefab;

	
	void Start()
	{
		// test code
		Show(IntVector2Utility.GetRect(IntVector2.down, new IntVector2(2, 2)));
		Hide();
		//Show(IntVector2Utility.GetRect(new IntVector2(-2, 2), new IntVector2(2, 2)));
		SetColor(new Color(.9f, .2f, .5f, .7f));
	}
	

	public void Hide()
	{
		for (int i = 0; i < pool.Count; i++)
			pool[i].gameObject.SetActive(false);
	}

	public void Show(IntVector2[] locations)
	{
		int i = 0;
		while(i < locations.Length)
		{
			if (i == pool.Count)
			{
				pool.Add(Instantiate(prefab));
				pool[i].transform.parent = transform;
				pool[i].color = color;
			}
			pool[i].gameObject.SetActive(true);
			pool[i].transform.position = (Vector3)locations[i];
			i++;
		}
		while(i < pool.Count)
		{
			pool[i].gameObject.SetActive(false);
			i++;
		}
	}

	public void SetColor(Color c)
	{
		color = c;
		for (int i = 0; i < pool.Count; i++)
			pool[i].color = c;
	}

	// because colour is not serailiazable, so it cannot be exposed to the editor in events. pure retardation.
	public void MakeBlue()
	{
		SetColor(new Color(0.2f, 0.3f, 0.9f, 0.7f));
	}

	public void MakeRed()
	{
		SetColor(new Color(0.9f, 0.1f, 0.3f, 0.7f));
	}

}
