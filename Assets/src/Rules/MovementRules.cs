using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mobile))]
public class MovementRules : MonoBehaviour
{
	public int team = 1; // spaghetti ?
	public int speed = 5;
	public bool flight = false;

	public HashSet<IntVector2> ReachableTiles()
	{ 
		IntVector2 orig = GetComponent<Mobile>().Location;
		HashSet<IntVector2> possibleMoves;
		if (flight) possibleMoves = Sprawl(orig, FlyingEnterCost, speed);
		else possibleMoves = Sprawl(orig, EnterCost, speed);
		possibleMoves.ExceptWith(Obstructions.Instance.OccupiedTiles());
		possibleMoves.Add(orig);
		return possibleMoves;
	}

	int FlyingEnterCost(IntVector2 origin, IntVector2 destination)
	{
		if (BlockedFlightAt(destination)) return 999;

		MapTerrain t = Obstructions.Instance.TerrainAt(destination);
		// Diagonal check
		if (origin.x != destination.x && origin.y != destination.y)
		{
			if (
				BlockedFlightAt(new IntVector2(origin.x, destination.y))
				&&
				BlockedFlightAt(new IntVector2(destination.x, origin.y))
				)
				return 999;
		}

		return 1; // regular movement
	}

	int EnterCost(IntVector2 origin, IntVector2 destination)
	{
		if (BlockedAt(destination)) return 999;

		MapTerrain t = Obstructions.Instance.TerrainAt(destination);
		// Diagonal check
		if (origin.x != destination.x && origin.y != destination.y)
		{
			if (
				BlockedAt(new IntVector2(origin.x, destination.y))
				&&
				BlockedAt(new IntVector2(destination.x, origin.y))
				)
				return 999;
		}

		if (t && t.difficultTerrain) return 2;
		return 1; // regular movement
	}

	bool BlockedAt(IntVector2 t)
	{
		MapTerrain mt = Obstructions.Instance.TerrainAt(t);
		MapObject o = Obstructions.Instance.MobileAt(t);
		if (mt && (mt.water || mt.wall)) return true;
		if (o)
		{
			MovementRules mr = o.GetComponent<MovementRules>();
			if (mr)
			{
				if (mr.team == team) return false;
			}
			return true; // unit block
		}
		return false;
	}

	bool BlockedFlightAt(IntVector2 t)
	{
		MapTerrain mt = Obstructions.Instance.TerrainAt(t);
		if (mt && mt.wall) return true;
		return false;
	}

	/// <summary>
	/// A generic implementaiotn of flood filling an area in a square grid enviroment using dnd 3e movement rules.
	/// Returns all valid locations within a certain reach
	/// entercost is a multiplier for movement for entering a location, set it high to prevent movement completely.
	/// </summary>
	/// <param name="origin"></param>
	/// <param name="enterCost">A function that gives the cost for entering a square. The fields are origin, destination, it returns an integer. increased cost for diagonal movement should NOT be a part of this!</param>
	/// <param name="range"></param>
	/// <returns></returns>
	static public HashSet<IntVector2> Sprawl(IntVector2 origin, System.Func<IntVector2, IntVector2, int> enterCost, int range)
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
				int cost = enterCost(check[i], check[i] + t) * 2;
				if (budget - cost > MoveAt(check[i] + t))
				{
					SetMoveAt(check[i] + t, budget - cost);
					check.Add(check[i] + t);
				}
			}

			foreach (IntVector2 t in diagonal)
			{
				int cost = enterCost(check[i], check[i] + t) * 3;
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
