using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System;

/// <summary>
/// This class have the potiential to evolve into a behemoth.
/// I guess, if it does, consider splitting it up betweeen subclasses.
/// </summary>
public class CommandInterface : NetworkBehaviour
{
	static CommandInterface _instance;
	static public CommandInterface Instance
	{
		get { return _instance; }
	}

	void Start()
	{
		if (isLocalPlayer)
			_instance = this;
	}

	[Command]
	public void CmdMove(GameObject target, IntVector2 destination)
	{
		target.GetComponent<Mobile>().MoveTo(destination);
	}
}

