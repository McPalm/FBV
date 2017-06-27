using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Immovable terrain
/// its rather crude, but hey, whatever works.
/// </summary>

public class MapTerrain : MonoBehaviour
{
	public bool wall;
	public bool water;
	public bool difficultTerrain;

	void Start()
	{
		Obstructions.Instance.Add(this);
		if(wall)
		{
			GameObject o = new GameObject("Hitbox");
			o.transform.parent = transform;
			o.transform.localPosition = Vector3.zero;
			//o.transform.RotateAround(Vector3.up, 45f);
			o.transform.Rotate(Vector3.forward, 45f);
			BoxCollider2D p = o.AddComponent<BoxCollider2D>();
			p.size = new Vector2(0.75f, 0.75f);
			o.layer = 8; // wall layer
		}
	}
}
