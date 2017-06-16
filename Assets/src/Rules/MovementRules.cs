using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mobile))]
public class MovementRules : MonoBehaviour
{
	public HashSet<IntVector2> ReachableTiles()
	{
		IntVector2 orig = GetComponent<Mobile>().Location;

		HashSet<IntVector2> s = new HashSet<IntVector2>(IntVector2Utility.GetRect(orig - new IntVector2(5, 5), orig + new IntVector2(5, 5)));
		
		return s;
	}
}
