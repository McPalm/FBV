using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : AController
{
	Action State;

	AAbility activeAbility;

	protected override void Startup()
	{
		State = SelectTarget;
		if(activeAbility == null)
		{
			Debug.LogWarning("Entered use ability state with no active ability to use!");
			EnableDefaultController();
		}
	}

	void Update()
	{
		State();
	}

	public void SetUser(GameObject o)
	{
		activeAbility = o.GetComponentInChildren<AAbility>();
	}

	void SelectTarget()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (Blocked) return;
			if (activeAbility.UseableAt(MouseTile))
			{
				// HACK, only usable on server right now.
				// activeAbility.Use(MouseTile);
				CommandInterface.Instance.CmdUseAbility(activeAbility.gameObject, MouseTile);
				EnableDefaultController();
			}
		}
		else if (Input.GetButtonDown("Cancel"))
			EnableDefaultController();
	}
}
