using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

/// <summary>
/// Abstract class for controllers.
/// Contains some utility to be used by controllers.
/// The primary thing is that only one controller can ever be active. If a controller is enabled, all other controllers are disabled.
/// </summary>

abstract public class AController : MonoBehaviour
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

	/// <summary>
	/// Called whenever this controller is enabled.
	/// </summary>
	abstract protected void Startup();

	/// <summary>
	/// The current mouse position in worldspace.
	/// </summary>
	static public Vector3 MousePosition
	{
		get
		{
			return Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
	}

	/// <summary>
	/// The current map position rounded to closest integers
	/// </summary>
	static public IntVector2 MouseTile
	{
		get
		{
			return IntVector2.RoundFrom(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		}
	}

	/// <summary>
	/// true if the mouse pointer is over and UI element or the user is panning the camera.
	/// </summary>
	public bool Blocked
	{
		get
		{
			return EventSystem.current.IsPointerOverGameObject() || dragging;
		}
	}

	/// <summary>
	/// Deactivate the current controller and return to the defalt controller.
	/// </summary>
	public void EnableDefaultController()
	{
		Default.enabled = true;
	}

	/// <summary>
	/// Find a component of a certain type attached to the first collider underneath the mouse pointer.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns>A Component or null</returns>
	protected T ComponentUnderMouse<T>() where T : Component
	{
		RaycastHit2D hit = Physics2D.Raycast(MousePosition, Vector2.zero);
		if (hit.collider)
			return hit.collider.GetComponent<T>();
		return null;
	}
}
