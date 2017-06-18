using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RosterManager : NetworkBehaviour
{
	static RosterManager _instance;

	[SerializeField]
	GameObject[] team1;
	[SerializeField]
	GameObject[] team2;

	public int count = 0;

	public GameObject[] Team1
	{
		get
		{
			return team1;
		}
	}

	public GameObject[] Team2
	{
		get
		{
			return team2;
		}
	}

	public static RosterManager Instance
	{
		get
		{
			if (!_instance) _instance = FindObjectOfType<RosterManager>();
			return _instance;
		}
	}

	public GameObject[] GetRoster()
	{
		count++;
		if(count == 1)
			return Team1;
		return Team2;
	}
}
