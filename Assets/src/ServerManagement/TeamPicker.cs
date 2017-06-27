using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamPicker : MonoBehaviour
{
	public void PickTeam(int i)
	{
		ClientRoster.Local.PickTeam(i);
	}

	public void GameMaster()
	{
		ClientRoster.Local.Master();
	}
}
