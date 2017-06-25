using System;
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

	void Update()
	{
		State();
	}

	public void SetUser(GameObject o)
	{
		user = o;
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
