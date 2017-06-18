[System.Serializable]
public struct Attributes
{
	public int hp;
	public int hit;
	public int dodge;
	public int damageBonus;
	public int damageRoll;
	public int defence;
	public int resistance;
	public int movement;

	public float HitVS(Attributes other)
	{
		int sum = hit - other.dodge;
		if(sum < 0)
		{
			return (2f) / (4f - sum) * 0.8f + 0.2f;
		}
		return (sum + 2f) / (sum + 4f) * 0.8f + 0.2f;
	}

	public float DefenceDamageFactor
	{
		get
		{
			return 9f / (9f + defence);
		}
	}

	public float ResistanceDamageFactor
	{
		get
		{
			return 9f / (9f + resistance);
		}
	}

	public int MinDamageVersus(Attributes other)
	{
		return UnityEngine.Mathf.RoundToInt(damageBonus * other.DefenceDamageFactor);
	}

	public int MaxDamageVersus(Attributes other)
	{
		return UnityEngine.Mathf.RoundToInt((damageRoll + damageBonus) * other.DefenceDamageFactor);
	}

	static public Attributes Default
	{
		get
		{
			Attributes a = new Attributes();
			a.hit = 5;
			a.dodge = 5;
			a.damageBonus = 5;
			a.damageRoll = 5;
			a.defence = 0;
			a.hp = 20;
			return a;
		}
	}
}
