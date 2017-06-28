using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class AbilityWeapon : AAbility
{
	public int diceSize = 6;
	public int range = 1;
	public DamageType damageType = DamageType.slashing;

	public GameObjectEvent EventHit = new GameObjectEvent();
	public GameObjectEvent EventMiss = new GameObjectEvent();

	public override string Description
	{
		get
		{
			if(damageType == DamageType.magic) return "DMG: 2d" + diceSize + "\tRange: " + range + "\n" + DamageTypeDescription(damageType, diceSize);
			return "DMG: 2d" + diceSize + "+Might \tRange: " + range + "\n" + DamageTypeDescription(damageType, diceSize);
		}
	}

	static public string DamageTypeDescription(DamageType dt, int diceSize)
	{
		switch(dt)
		{
			case DamageType.slashing:
				return "Slashing (+1d" + diceSize + " vs light, -1d" + diceSize + " vs chain)";
			case DamageType.piercing:
				return "Piercing (+1d" + diceSize + " vs chain, -1d" + diceSize + " vs plate)";
			case DamageType.crushing:
				return "Crushing (+1d" + diceSize + " vs plate, -1d" + diceSize + " vs light)";
			case DamageType.magic:
				return "Magic (ignores armor)";
		}
		return "";
	}

	public override bool Useable
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IntVector2[] TargetableTiles()
	{
		if(range == 1)
		{
			transform.localPosition = Vector3.zero;
			IntVector2 orig = IntVector2.RoundFrom(transform.position);
			IntVector2[] r =
				{
			orig + IntVector2.left,
			orig + IntVector2.up,
			orig + IntVector2.right,
			orig + IntVector2.down,

			orig + IntVector2.upleft,
			orig + IntVector2.upright,
			orig + IntVector2.downleft,
			orig + IntVector2.downright,
			};
			return r;
		}

		transform.localPosition = Vector3.zero;
		IntVector2 tile = IntVector2.RoundFrom(transform.position);
		List<IntVector2> l = new List<IntVector2>();
		foreach (IntVector2 iv2 in IntVector2Utility.GetRect(tile - new IntVector2(range, range), tile + new IntVector2(range, range)))
		{
			if (tile == iv2) continue;
			if ((tile - iv2).MagnitudePF <= range)
				if (Obstructions.HasLOS(tile, iv2))
					l.Add(iv2);
		}
		return l.ToArray();

	}

	public override void Use(IntVector2 target)
	{
		GameObject o = GameObjectAt(target);
		if (o)
		{
			// determine damage roll
			Character c = GetComponentInParent<Character>();
			int dice = 2; 
			Character enemy = o.GetComponent<Character>();
			int armor = 0;
			if (enemy)
			{
				armor = enemy.Attributes.armor;
				dice = DiceVS(damageType, enemy.Attributes.armorType);
				print(dice);
			}

			// roll and apply armor
			Dice d = new Dice(diceSize);
			int totalDamage = 0;
			if (damageType != DamageType.magic) totalDamage += c.Attributes.might - armor;
			for(int i = 0; i < dice; i++)
			{
				totalDamage += d.Roll();
			}

			if (totalDamage < 0) totalDamage = 0;

			if (damageType == DamageType.magic) print(dice + "d" + diceSize + " = " + totalDamage);
			else print(dice + "d" + diceSize + "+" + c.Attributes.might + "-" + armor + " = " + totalDamage);

			// apply damage
			bool hit = true;
			if (hit) o.GetComponent<HitPoints>().Hurt(totalDamage);
			RpcClientCode(o, hit);
		}
	}

	[ClientRpc]
	public void RpcClientCode(GameObject target, bool hit)
	{
		if (hit) EventHit.Invoke(target);
		else EventMiss.Invoke(target);
	}

	static public int DiceVS(DamageType dt, ArmorType at)
	{
		print(dt);
		print(at);
		switch (dt)
		{
			case DamageType.slashing:
				if (at == ArmorType.chain) return 1;
				if (at == ArmorType.light) return 3;
				return 2;
			case DamageType.piercing:
				if (at == ArmorType.plate) return 1;
				if (at == ArmorType.chain) return 3;
				return 2;
			case DamageType.crushing:
				if (at == ArmorType.light) return 1;
				if (at == ArmorType.plate) return 3;
				return 2;
		}
		print("Fall Through");
		return 2; // default to 2 d6.
	}
			 

	public override bool UseableAt(IntVector2 target)
	{
		Mobile m = GetComponentInParent<Mobile>();

		if (target == m.Location) return false;

		if (range == 1)
		{
			if ((m.Location - target).MagnitudeMax == 1)
			{
				// in melee range
				GameObject o = GameObjectAt(target);
				return o && o.GetComponent<HitPoints>();
			}
			return false;
		}

		
		if ((m.Location - target).MagnitudePF <= range)
		{
			if (Obstructions.HasLOS(IntVector2.RoundFrom(transform.position), target))
			{
				GameObject o = GameObjectAt(target);
				return o && o.GetComponent<HitPoints>();
			}
		}
		return false;
	}

	[Serializable]
	public class GameObjectEvent : UnityEvent<GameObject> { }
}
