using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TurnTracker : NetworkBehaviour
{

	static TurnTracker _instance;

	[SerializeField]
	UnityEngine.UI.Text centre;
	[SerializeField]
	UnityEngine.UI.Text corner;

	[SyncVar]
	public int activeTeam = 0;
	[SyncVar]
	public int turn = 0;

	public static TurnTracker Instance
	{
		get
		{
			if (!_instance) _instance = FindObjectOfType<TurnTracker>();
			return _instance;
		}
	}

	void Start()
	{
		if (isServer)
		{
			TurnEntry.EventInactive.AddListener(CheckAll);
			TurnEntry.EventEndTurn.AddListener(EndTurn);
		}
	}

	/// <summary>
	/// Check if we should let the turn over to the other player
	/// </summary>
	/// <param name="o"></param>
	void EndTurn(GameObject acted)
	{
		bool allMoved = true;
		// ensure team we switch into have moveable units.
		if (activeTeam == 1)
			foreach (GameObject o in RosterManager.Instance.Team1)
				if (o.GetComponent<TurnEntry>().CanAct) allMoved = false;
		if (activeTeam == 0)
			foreach (GameObject o in RosterManager.Instance.Team2)
				if (o.GetComponent<TurnEntry>().CanAct) allMoved = false;
		if (allMoved)
				return;
		
		activeTeam++;
		activeTeam %= 2;
		RpcAnnounceTurn(activeTeam);
	}

	/// <summary>
	/// Checks if we need to refresh all units
	/// </summary>
	void CheckAll()
	{	
		if (EndOfTurn())
		{
			print("End Turn!");
			StartCoroutine(RefreshAll());
			turn++;
			activeTeam = turn % 2;
			RpcAnnounceTurn(activeTeam);
		}
		print("...");
	}

	public bool IsMyTurn(GameObject o1)
	{
		if (activeTeam == 0)
			foreach(GameObject o2 in RosterManager.Instance.Team1)
				if(o2 == o1) return true;
		if (activeTeam == 1)
			foreach (GameObject o2 in RosterManager.Instance.Team2)
				if (o2 == o1) return true;
		return false;
	}

	public bool EndOfTurn()
	{
		foreach (TurnEntry e in FindObjectsOfType<TurnEntry>())
		{
			print(e.name + " " + e.CanAct);
			if (e.CanAct) return false;
		}
		return true;
	}

	IEnumerator RefreshAll(float delay = 0.25f)
	{
		yield return new WaitForSeconds(delay);
		foreach (TurnEntry e in FindObjectsOfType<TurnEntry>())
			e.RpcSetActed(false);
	}

	[ClientRpc]
	void RpcAnnounceTurn(int team)
	{
		centre.gameObject.SetActive(true);
		if (team == 0)
		{
			centre.text = "Blue Move";
			centre.color = new Color(0.3f, 0.2f, 1f);
			corner.text = "Blue Move";
			corner.color = new Color(0.3f, 0.2f, 1f);
		}
		else
		{
			centre.text = "Red Move";
			centre.color = new Color(1f, 0.2f, 0.3f);
			corner.text = "Blue Move";
			corner.color = new Color(0.3f, 0.2f, 1f);
		}
		StartCoroutine(HideText());
	}

	IEnumerator HideText()
	{
		yield return new WaitForSeconds(1.5f);
		centre.gameObject.SetActive(false);
	}
}
