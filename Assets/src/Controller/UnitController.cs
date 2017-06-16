using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// A controller class that allows us to pickup and move objects with the Draggable component
/// </summary>

public class UnitController : AController
{
	[SerializeField]
	Transform dropShadow;
	[SerializeField]
	StrechBetweenTwoObjects arrow;

	GameObject pickupOrigin;
	System.Action State;
	Draggable heldPiece;
	HashSet<IntVector2> moveTiles;

	public GameObjectEvent EventSelect = new GameObjectEvent();
	public UnityEvent EventStateNeutral = new UnityEvent();
	public UnityEvent EventStatePickup = new UnityEvent();
	public IV2ArrayEvent EventMoveTiles = new IV2ArrayEvent();

	void Awake()
	{
		Default = this;
	}

	// Update is called once per frame
	void Update ()
	{
		State();
	}

	void Neutral()
	{
		if (Blocked) return;
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(MousePosition, Vector2.zero);
			if(hit.collider)
			{
				Draggable d = hit.collider.GetComponent<Draggable>();
				if (d && d.Moveable)
				{
					heldPiece = d;
					State = Drag;
					EventStatePickup.Invoke();
					dropShadow.gameObject.SetActive(true);
					arrow.gameObject.SetActive(true);
					arrow.target = dropShadow.gameObject;
					arrow.origin = pickupOrigin;
					pickupOrigin.transform.position = heldPiece.transform.position;
					dropShadow.position = MousePosition + new Vector3(0f, 0f, 10f);
					EventSelect.Invoke(heldPiece.gameObject);
					if (d.GetComponent<MovementRules>())
					{
						moveTiles = d.GetComponent<MovementRules>().ReachableTiles();
						IntVector2[] arr = new IntVector2[moveTiles.Count];
						moveTiles.CopyTo(arr);
						EventMoveTiles.Invoke(arr);
					}
					else
						moveTiles = null;
				}
			}
		}
	}

	void Drag()
	{
		heldPiece.transform.position = MousePosition + new Vector3(0f, 0.3f, 10f);
		dropShadow.position = MousePosition + new Vector3(0f, 0f, 10f);
		if (Input.GetMouseButtonUp(0))
		{
			if (CanMoveTo(heldPiece.Mobile, MouseTile))
			{
				heldPiece.Mobile.LocalMove(MouseTile); // snap before tween action.
				CommandInterface.Instance.CmdMove(heldPiece.gameObject, MouseTile);
				GetComponent<AbilityController>().enabled = true;
			}
			else
			{
				heldPiece.Mobile.Return();
				State = Neutral;
				EventStateNeutral.Invoke();

			}
			dropShadow.gameObject.SetActive(false);
			arrow.gameObject.SetActive(false);
		}
	}

	protected override void Startup()
	{
		State = Neutral;
		EventStateNeutral.Invoke();
		if (pickupOrigin == null) pickupOrigin = new GameObject("Pickup Origin");
	}

	public bool CanMoveTo(Mobile user, IntVector2 targetLocation)
	{
		if (moveTiles != null)
			return moveTiles.Contains(targetLocation);
		return true;
		/*
		IntVector2 orig = IntVector2.RoundFrom(user.GetComponent<Mobile>().location);
		orig -= targetLocation;
		// if (orig.ManahattanMagnitude > user.movement) return false;

		// TODO, refine this. Use like a ditrjokas algorithm.

		return true;
		*/
	}

	[System.Serializable]
	public class GameObjectEvent : UnityEvent<GameObject> { }
	[System.Serializable]
	public class IV2ArrayEvent : UnityEvent<IntVector2[]> { }
}