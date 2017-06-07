using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

abstract public class AController : NetworkBehaviour
{
	private bool dragging = false;
	AController defaultController;
	protected AController Default
	{	// a syncronized default controller across the game object.
		set
		{
			foreach (AController c in GetComponents<AController>())
				c.defaultController = value;
		}
		get
		{
			if (!defaultController)
				foreach (AController c in GetComponents<AController>())
					if (c.defaultController) defaultController = c.defaultController;
			return defaultController;
		}
	}

	protected void Start()
	{
		CameraDrag.Instance.EventStartDrag.AddListener(delegate { dragging = true; });
		CameraDrag.Instance.EventStopDrag.AddListener(delegate { dragging = false; });
	}

	protected void OnEnable()
	{
		foreach(AController c in GetComponents<AController>())
		{
			if (c != this && c.enabled == true) c.enabled = false;
		}
		Startup();
	}

	abstract protected void Startup();

	static public Vector3 MousePosition
	{
		get
		{
			return Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
	}

	static public IntVector2 MouseTile
	{
		get
		{
			return IntVector2.RoundFrom(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		}
	}

	public bool Blocked
	{
		get
		{
			return EventSystem.current.IsPointerOverGameObject() || dragging;
		}
	}

	public void EnableDefaultController()
	{
		Default.enabled = true;
	}

	protected T ComponentUnderMouse<T>() where T : Component
	{
		RaycastHit2D hit = Physics2D.Raycast(MousePosition, Vector2.zero);
		if (hit.collider)
			return hit.collider.GetComponent<T>();
		return null;
	}
}
