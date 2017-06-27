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

	public GameObject[] GetAll()
	{
		GameObject[] both = new GameObject[team1.Length + team2.Length];
		for(int i = 0; i < both.Length; i++)
		{
			if (i < team1.Length)
				both[i] = team1[i];
			else
				both[i] = team2[i - team1.Length];
		}
		return both;
	}
}
