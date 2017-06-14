using UnityEngine;
using UnityEngine.Events;
using System.Collections;
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
	// CharacterSelector characterSelector;


	public GameObjectEvent EventSelect = new GameObjectEvent();
	// public CharacterEvent EventPickup = new CharacterEvent();
	// public CharacterEvent EventDrop = new CharacterEvent();

	void Awake()
	{
		Default = this;
		// characterSelector = FindObjectOfType<CharacterSelector>();
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
					dropShadow.gameObject.SetActive(true);
					arrow.gameObject.SetActive(true);
					arrow.target = dropShadow.gameObject;
					arrow.origin = pickupOrigin;
					pickupOrigin.transform.position = heldPiece.transform.position;
					dropShadow.position = MousePosition + new Vector3(0f, 0f, 10f);
					EventSelect.Invoke(heldPiece.gameObject);
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
				// EventDrop.Invoke(heldPiece.GetComponent<Character>());
				GetComponent<AbilityController>().enabled = true;
			}
			else
			{
				heldPiece.Mobile.Return();
			}
			State = Neutral;
			dropShadow.gameObject.SetActive(false);
			arrow.gameObject.SetActive(false);
		}
	}

	protected override void Startup()
	{
		State = Neutral;
		if(pickupOrigin == null) pickupOrigin = new GameObject("Pickup Origin");
	}

	public bool CanMoveTo(Mobile user, IntVector2 targetLocation)
	{
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
}