using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldToScreen : MonoBehaviour
{
	Transform followMe;


	void LateUpdate()
	{
		if(followMe)
		{
			Put(followMe.transform.position);
		}
	}

	public void Put(GameObject o)
	{
		Put(o.transform.position);
	}

	public void Put(Transform t)
	{
		Put(t.position);
	}

	public void Put(Vector2 worldLocation)
	{
		transform.position = Camera.main.WorldToScreenPoint(worldLocation);
	}

	public void Follow(GameObject o)
	{
		followMe = o.transform;
		Put(o);
	}

	public void Follow(Transform t)
	{
		followMe = t;
		Put(t);
	}

	public void StopFollow()
	{
		followMe = null;
	}
}
