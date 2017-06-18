using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TurnEntry : NetworkBehaviour
{
	bool acted = false;

	public Material normal;
	public Material grayscale;

	Color outlineColor;

	public bool Acted
	{
		get
		{
			return acted;
		}
		set
		{
			acted = value;
		}
	}

	void Start()
	{
		outlineColor = GetComponent<SpriteOutline>().Color;
	}

	[ClientRpc]
	public void RpcSetActed(bool acted)
	{
		this.acted = acted;

		GetComponent<SpriteRenderer>().material = (acted) ? grayscale : normal;
		GetComponent<SpriteOutline>().Color = (acted) ? outlineColor * new Color(0.8f, 0.5f, 0.8f) : outlineColor;
	}
}
