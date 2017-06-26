using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InGameNetworkUI : MonoBehaviour
{
	public void Disconnect()
	{
		NetworkManager.singleton.StopServer();
		NetworkManager.singleton.StopClient();
	}
}
