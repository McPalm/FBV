﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AbilityController : AController
{
	[SerializeField]
	ButtonArray buttons;

	Action State;

	GameObject user;
	AAbility activeAbility;

	public UnityEvent EventStateSelectTarget = new UnityEvent();
	public TilesEvent EventSelectTargetTiles = new TilesEvent();

	void Awake()
	{
		buttons.EventButtonPressed.AddListener(SelectAbility);
	}

	protected override void Startup()
	{
		State = SelectTarget;
		EventStateSelectTarget.Invoke();
		if (activeAbility == null)
		{
			Debug.LogWarning("Entered use ability state with no active ability to use!");
			EnableDefaultController();
		}
		else
		{
			EventSelectTargetTiles.Invoke(activeAbility.TargetableTiles());
		}
	}

	void OnDisable()
	{
		buttons.ClearIcons();
	}

	void LateUpdate()
	{
		State();
	}

	public void SetUser(GameObject o)
	{
		user = o;
		AAbility[] aas = o.GetComponentsInChildren<AAbility>();
		if (aas.Length == 0) return;
		activeAbility = aas[0];
		for(int i = 0; i < aas.Length; i++)
		{
			buttons.SetIcon(i, aas[i].icon);
		}
	}

	void SelectTarget()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (Blocked) return;
			if (activeAbility.UseableAt(MouseTile))
			{
				CommandInterface.Instance.CmdUseAbility(user, MouseTile);
				EnableDefaultController();
			}
		}
		else if (Input.GetButtonDown("Cancel"))
		{
			GetComponent<UnitController>().Undo();
			EnableDefaultController();
		}
		else if (Input.GetButtonDown("Skip"))
		{
			EnableDefaultController(); // TODO, chance to return
			CommandInterface.Instance.CmdEndTurn(user);
		}
	}

	void SelectAbility(int i)
	{
		if (enabled && user)
		{
			AAbility[] aas = user.GetComponentsInChildren<AAbility>();
			if (i < aas.Length)
			{
				activeAbility = aas[i];
				EventSelectTargetTiles.Invoke(activeAbility.TargetableTiles());
			}
		}
	}

	[Serializable]
	public class TilesEvent : UnityEvent<IntVector2[]> { }
}
