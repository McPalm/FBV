using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class NetworkUI : MonoBehaviour
{
	private string roomName = "default";

	public UnityEvent EventConnect = new UnityEvent();
	public UnityEvent EventDisconnect = new UnityEvent();
	public UnityEvent EventFailConnect = new UnityEvent();

	List<MatchInfoSnapshot> matches;

	[SerializeField]
	ButtonArray roomList;

	public string RoomName
	{
		get
		{
			return roomName;
		}

		set
		{
			roomName = value;
		}
	}

	public void LocalHost()
	{
		NetworkManager.singleton.StartHost();
	}

	public void LocalJoin()
	{
		NetworkManager.singleton.StartClient();
	}

	public void EnterMatchMaker()
	{
		NetworkManager.singleton.StartMatchMaker();
		FindInternetMatch();
	}

	public void LeaveMatchMaker()
	{
		NetworkManager.singleton.StopMatchMaker();
		EventDisconnect.Invoke();
	}

	public void CreateInternetMatch()
	{
		NetworkManager.singleton.matchMaker.CreateMatch(roomName, 2, true, "", "", "", 0, 0, OnInternetMatchCreate);
	}

	private void OnInternetMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
	{
		if (success)
		{
			//Debug.Log("Create match succeeded");

			MatchInfo hostInfo = matchInfo;
			NetworkServer.Listen(hostInfo, 9000);

			NetworkManager.singleton.StartHost(hostInfo);
		}
		else
		{
			Debug.LogError("Create match failed");
		}
	}

	//call this method to find a match through the matchmaker
	public void FindInternetMatch()
	{
		NetworkManager.singleton.matchMaker.ListMatches(0, 10, "", true, 0, 0, OnInternetMatchList);
	}

	//this method is called when a list of matches is returned
	private void OnInternetMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
	{
		if (success)
		{
			this.matches = matches;
			if (matches.Count != 0)
			{
				Debug.Log("A list of matches was returned");

				//join the last server (just in case there are two...)
				//NetworkManager.singleton.matchMaker.JoinMatch(matches[matches.Count - 1].networkId, "", "", "", 0, 0, OnJoinInternetMatch);
				roomList.SetAllLabels();
				for (int i = 0; i < matches.Count; i++)
				{
					roomList.SetLabel(i, matches[i].name);
				}
			}
			else
			{
				Debug.Log("No matches in requested room!");
			}
			EventConnect.Invoke();
		}
		else
		{
			EventFailConnect.Invoke();
			Debug.LogWarning("Couldn't connect to match maker");
		}
	}

	public void JoinMatch(int i)
	{
		if(i < matches.Count)
			NetworkManager.singleton.matchMaker.JoinMatch(matches[i].networkId, "", "", "", 0, 0, OnJoinInternetMatch);
	}

	//this method is called when your request to join a match is returned
	private void OnJoinInternetMatch(bool success, string extendedInfo, MatchInfo matchInfo)
	{
		if (success)
		{
			//Debug.Log("Able to join a match");

			MatchInfo hostInfo = matchInfo;
			NetworkManager.singleton.StartClient(hostInfo);
		}
		else
		{
			Debug.LogError("Join match failed");
		}
	}
}
