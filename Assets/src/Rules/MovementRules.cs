using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mobile))]
public class MovementRules : MonoBehaviour
{
	[SerializeField]
	int team = 1; // spaghetti ?

	public HashSet<IntVector2> ReachableTiles()
	{ 
		IntVector2 orig = GetComponent<Mobile>().Location;
		// HashSet<IntVector2> s = new HashSet<IntVector2>();

		// aint you a bit t big for an inline function?
		System.Func<IntVector2, int> EnterCost = (IntVector2 iv2) =>
		{
			MapTerrain t = Obstructions.Instance.TerrainAt(iv2);
			MapObject o = Obstructions.Instance.MobileAt(iv2);

			int cost = 1;

			if(t)
			{
				if (t.wall || t.water)
					return 999; // terrain block
				if (t.difficultTerrain)
					cost = 2; // difficult terrain
			}
			if(o)
			{
				MovementRules mr = o.GetComponent<MovementRules>();
				if(mr)
				{
					if (mr.team == team) return cost;
				}
				return 999; // unit block
			}
			return cost; // regular movement
		};

		HashSet<IntVector2> possibleMoves = Sprawl(orig, EnterCost, 5);
		possibleMoves.ExceptWith(Obstructions.Instance.OccupiedTiles());
		possibleMoves.Add(orig);
		return possibleMoves;

			/*
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
		*/
	}

	/// <summary>
	/// A generic implementaiotn of flood filling an area in a square grid enviroment using dnd 3e movement rules.
	/// Returns all valid locations within a certain reach
	/// entercost is a multiplier for movement for entering a location, set it high to prevent movement completely.
	/// </summary>
	/// <param name="origin"></param>
	/// <param name="enterCost"></param>
	/// <param name="range"></param>
	/// <returns></returns>
	static public HashSet<IntVector2> Sprawl(IntVector2 origin, System.Func<IntVector2, int> enterCost, int range)
	{
		int[,] moveLeft = new int[(range * 2 + 3), (range * 2 + 3)];

		System.Func<IntVector2, int> MoveAt = (IntVector2 iv2) => 
		{
			return moveLeft[iv2.x - origin.x + range + 1, iv2.y - origin.y + range + 1];
		};
		System.Action<IntVector2, int> SetMoveAt = (IntVector2 iv2, int i) =>
		{
			moveLeft[iv2.x - origin.x + range + 1, iv2.y - origin.y + range + 1] = i;
		};
		IntVector2[] cardinal = { IntVector2.left, IntVector2.up, IntVector2.right, IntVector2.down };
		IntVector2[] diagonal = { IntVector2.upleft, IntVector2.upright, IntVector2.downleft, IntVector2.downright };

		List<IntVector2> check = new List<IntVector2>();
		check.Add(origin);
		SetMoveAt(origin, range * 2 + 2);
		// moveLeft[range, range] = range * 2 + 2;
		for(int i = 0; i < check.Count; i++)
		{
			int budget = MoveAt(check[i]);
			if (budget < 2) continue;

			foreach(IntVector2 t in cardinal)
			{
				int cost = enterCost(check[i] + t) * 2;
				if (budget - cost > MoveAt(check[i] + t))
				{
					SetMoveAt(check[i] + t, budget - cost);
					check.Add(check[i] + t);
				}
			}

			foreach (IntVector2 t in diagonal)
			{
				int cost = enterCost(check[i] + t) * 3;
				if (budget - cost > MoveAt(check[i] + t))
				{
					SetMoveAt(check[i] + t, budget - cost);
					check.Add(check[i] + t);
				}
			}
		}

		HashSet<IntVector2> result = new HashSet<IntVector2>();
		foreach (IntVector2 iv2 in IntVector2Utility.GetRect(origin - new IntVector2(range, range), origin + new IntVector2(range, range)))
		{
			if (MoveAt(iv2) > 0) result.Add(iv2);
		}

		return result;
	}
		 	 
}
