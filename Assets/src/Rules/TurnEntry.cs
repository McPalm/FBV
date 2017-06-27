using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;


public class TurnEntry : NetworkBehaviour
{
	static public UnityEvent EventInactive = new UnityEvent(); // invoked whenever a unit becomes unable to act. Calls a refresh on the turn tracker.
	static public GameObjectEvent EventEndTurn = new GameObjectEvent(); // invoked when a unit acts

	bool acted = false;

	public Material normal;
	public Material grayscale;

	Color outlineColor;

	public bool CanAct
	{
		get
		{
			return !acted && enabled;
		}
		set
		{
			acted = !value;
			if (acted) EventInactive.Invoke();
		}
	}

	void Start()
	{
		outlineColor = GetComponent<SpriteOutline>().Color;
		HitPoints hp = GetComponent<HitPoints>();
		if (hp)
			hp.EventDeath.AddListener(() => { enabled = false; EventInactive.Invoke(); });
	}

	public void EndTurn()
	{
		acted = true;
		EventEndTurn.Invoke(gameObject);
		EventInactive.Invoke();
	}

	[ClientRpc]
	public void RpcSetActed(bool acted)
	{
		this.acted = acted;

		GetComponent<SpriteRenderer>().material = (acted) ? grayscale : normal;
		GetComponent<SpriteOutline>().Color = (acted) ? outlineColor * new Color(0.8f, 0.5f, 0.8f) : outlineColor;
	}

	public class GameObjectEvent : UnityEvent<GameObject> { }
}
