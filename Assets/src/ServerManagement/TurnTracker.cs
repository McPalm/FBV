using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TurnTracker : NetworkBehaviour
{
	public void CheckAll()
	{
		if(EndOfTurn())
		{
			StartCoroutine(RefreshAll());
			
		}
	}

	public bool EndOfTurn()
	{
		foreach(TurnEntry e in FindObjectsOfType<TurnEntry>())
			if (!e.Acted) return false;
		return true;
	}

	IEnumerator RefreshAll(float delay = 1f)
	{
		yield return new WaitForSeconds(delay);
		foreach (TurnEntry e in FindObjectsOfType<TurnEntry>())
			e.RpcSetActed(false);
	}
}
