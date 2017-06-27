using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using System;

/// <summary>
/// Contrains methods that sends commands to the server with actions the player wish to perform.
/// The classes uses the [command] tag. This tag flags the function so that whenever its called, it invokes
/// that method no the server rather than actually running it in the script.
/// [command] methods can only be called on a gameobject that the local player owns. Thus we need to collect
/// all commands in a singular location.
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
		if (isLocalPlayer) // So what this means, is that while COmmandInterface is simlilar to a singleton. It is actually not. A commandInterface will exsist for every player on the server. But only the one that the player control with be accessible through the Instance propert.
			_instance = this;
	}

	/// <summary>
	/// Moves a gameobject with a Mobile component to a certain location.
	/// It will tween to that location.
	/// </summary>
	/// <param name="target"></param>
	/// <param name="destination"></param>
	[Command]
	public void CmdMove(GameObject target, IntVector2 destination)
	{
		target.GetComponent<Mobile>().MoveTo(destination);
	}

	[Command]
	public void CmdUseAbility(GameObject ability, IntVector2 target)
	{
		ability.GetComponent<AAbility>().Use(target);
		CmdEndTurn(ability.transform.parent.gameObject);
	}

	[Command]
	public void CmdEndTurn(GameObject o)
	{
		o.GetComponent<TurnEntry>().RpcSetActed(true);
		o.GetComponent<TurnEntry>().EndTurn();
	}
}

