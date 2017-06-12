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

	protected void Start()
	{
		if (isServer)
		{
			GetID();
		}
		if(isLocalPlayer)
		{
			local = this;
		}
	}

	public void GetID()
	{
		roster = FindObjectOfType<RosterManager>().GetRoster();
		RpcAssignRoster(roster);
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
}
