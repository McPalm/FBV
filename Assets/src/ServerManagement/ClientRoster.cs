using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientRoster : NetworkBehaviour
{

	static ClientRoster local;

	GameObject[] roster;

	public static ClientRoster Local
	{
		get
		{
			return local;
		}
	}

	void Start()
	{
		if (isLocalPlayer)
			local = this;
	}

	[ClientRpc]
	public void RpcAssignRoster(GameObject[] roster)
	{
		this.roster = roster;
	}

	public bool Contains(GameObject o)
	{
		foreach (GameObject o2 in roster)
			if (o2 == o) return true;
		return false;
	}

	public void Master()
	{
		CmdRequestTeam(0);
	}

	public void PickTeam(int team)
	{
		CmdRequestTeam(team);
	}

	[Command]
	public void CmdRequestTeam(int i)
	{
		GameObject[] os = null;
		if (i == 0)
			os = FindObjectOfType<RosterManager>().GetAll();
		if (i == 1)
			os = FindObjectOfType<RosterManager>().Team1;
		if (i == 2)
			os = FindObjectOfType<RosterManager>().Team2;
		if (os != null)
			RpcAssignRoster(os);
	}
}
