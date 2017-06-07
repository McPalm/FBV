using UnityEngine;
using System.Collections;

public class StrechBetweenTwoObjects : MonoBehaviour
{
	public GameObject target;
	public GameObject origin;

	
	// Update is called once per frame
	public void Update ()
	{
		if(target && origin)
		{
			transform.position = (target.transform.position + origin.transform.position) / 2f;
			transform.localScale = new Vector3((target.transform.position - origin.transform.position).magnitude, 1f, 1f);
			transform.right = target.transform.position - origin.transform.position;
		}
	}
}
