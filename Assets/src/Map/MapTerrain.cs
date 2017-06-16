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
	}
}
