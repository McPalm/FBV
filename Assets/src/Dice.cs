using UnityEngine;

[System.Serializable]
public class Dice
{
	public int sides;
	public int result = 0;

	public Dice(int sides)
	{
		if (sides < 1) this.sides = 1;
		else this.sides = sides;
	}

	public int Roll()
	{
		result = DX(sides);
		return result;
	}

	static public int DX(int sides)
	{
		if (sides == 0) return 0;
		if (sides < 0) return 1;
		return Random.Range(1, sides + 1);
	}
}
