using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mobile))]
public class MovementRules : MonoBehaviour
{
	public HashSet<IntVector2> ReachableTiles()
	{
		IntVector2 orig = GetComponent<Mobile>().Location;

		HashSet<IntVector2> s = new HashSet<IntVector2>();

		foreach(IntVector2 iv2 in IntVector2Utility.GetRect(orig - new IntVector2(5, 5), orig + new IntVector2(5, 5)))
		{
			MapTerrain t = Obstructions.Instance.TerrainAt(iv2);
			MapObject o = Obstructions.Instance.MobileAt(iv2);
			if (o)
			{

				if (iv2 == orig) s.Add(iv2);
				// no bueno
			}
			else if (t)
			{
				if (!t.wall & !t.water)
					s.Add(iv2);
			}
			else
				s.Add(iv2);
		}

		
		


		return s;
	}
}
