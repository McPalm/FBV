[System.Serializable]
public struct Attributes
{
	public int hp;
	public int might;
	public int armor;
	public ArmorType armorType;
	public int speed;

	static public Attributes Default
	{
		get
		{
			Attributes a = new Attributes();

			a.hp = 20;
			a.might = 1;
			a.armor = 0;
			a.armorType = ArmorType.light;
			a.speed = 5;

			return a;
		}
	}
}
