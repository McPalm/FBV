using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstructions : MonoBehaviour
{
	static Obstructions _instance;

	public static Obstructions Instance
	{
		get
		{
			if (!_instance) _instance = FindObjectOfType<Obstructions>();
			return _instance;
		}
	}

	Dictionary<IntVector2, MapTerrain> terrain = new Dictionary<IntVector2, MapTerrain>();
	HashSet<Mobile> mobiles = new HashSet<Mobile>();

	public void Add(Mobile m)
	{
		mobiles.Add(m);
	}

	public void Add(MapTerrain t)
	{
		terrain.Add(IntVector2.RoundFrom(t.transform.position), t);
	}

	public MapTerrain TerrainAt(IntVector2 iv2)
	{
		MapTerrain t = null;
		terrain.TryGetValue(iv2, out t);
		return t;
	}

	public Mobile MobileAt(IntVector2 iv2)
	{
		foreach (Mobile m in mobiles)
			if (m.Location == iv2) return m;
		return null;
	}

	// get all currently occupied tiles.
	public HashSet<IntVector2> OccupiedTiles()
	{
		HashSet<IntVector2> o = new HashSet<IntVector2>();
		foreach (Mobile m in mobiles)
			o.Add(m.Location);
		return o;
	}

	internal void Remove(Mobile mobile)
	{
		mobiles.Remove(mobile);
	}
}
