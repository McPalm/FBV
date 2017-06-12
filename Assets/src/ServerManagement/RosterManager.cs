using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RosterManager : NetworkBehaviour
{
	[SerializeField]
	GameObject[] team1;
	[SerializeField]
	GameObject[] team2;

	public int count = 0;

	public GameObject[] GetRoster()
	{
		count++;
		if(count == 1)
			return team1;
		return team2;
	}
}
