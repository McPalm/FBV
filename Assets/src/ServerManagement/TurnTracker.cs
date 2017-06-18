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

	public int moves = 0;
	[SyncVar]
	public int activeTeam = 0;

	public static TurnTracker Instance
	{
		get
		{
			if (!_instance) _instance = FindObjectOfType<TurnTracker>();
			return _instance;
		}
	}

	public void CheckAll()
	{
		moves++;
		bool switchSide = false;
		bool newTurn = false;
		if (EndOfTurn())
		{
			newTurn = true;
			StartCoroutine(RefreshAll());
		}
		else
		{
			// if all units in active team has moved, switch side
			bool allMoved = true;

			if (activeTeam == 0)
				foreach (GameObject o in RosterManager.Instance.Team1)
					if (!o.GetComponent<TurnEntry>().Acted) allMoved = false;

			if (activeTeam == 1)
				foreach (GameObject o in RosterManager.Instance.Team2)
					if (!o.GetComponent<TurnEntry>().Acted) allMoved = false;

			if (allMoved) switchSide = true;
		}
		if (!switchSide && moves == 2)
		{
			switchSide = true;
		}
		if (switchSide)
		{
			if(!newTurn)
			{
				bool allMoved = true;
				// ensure team we switch into have moveable units.
				if (activeTeam == 1)
					foreach (GameObject o in RosterManager.Instance.Team1)
						if (!o.GetComponent<TurnEntry>().Acted) allMoved = false;

				if (activeTeam == 0)
					foreach (GameObject o in RosterManager.Instance.Team2)
						if (!o.GetComponent<TurnEntry>().Acted) allMoved = false;
				if (allMoved)
					return;
			}
			moves = 0;
			activeTeam++;
			activeTeam %= 2;
			RpcAnnounceTurn(activeTeam);
		}
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
		foreach(TurnEntry e in FindObjectsOfType<TurnEntry>())
			if (!e.Acted) return false;
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
