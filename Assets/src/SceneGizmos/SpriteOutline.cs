using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOutline : MonoBehaviour
{
	[SerializeField]
	Material m;
	[SerializeField]
	Color color;
	[SerializeField]
	float thickness = 0.05f;
	[SerializeField]
	string layerName;

	SpriteRenderer up;
	SpriteRenderer down;
	SpriteRenderer left;
	SpriteRenderer right;

	public Color Color
	{
		get
		{
			return color;
		}

		set
		{
			up.color = value;
			down.color = value;
			left.color = value;
			right.color = value;
			color = value;
		}
	}

	public void Start()
	{
		GameObject go = new GameObject();
		up = go.AddComponent<SpriteRenderer>();
		go.transform.parent = transform;
		go.transform.localPosition = new Vector3(0f, thickness);
		go.transform.localScale = Vector3.one;
		up.material = m;
		up.sprite = GetComponent<SpriteRenderer>().sprite;
		up.flipX = GetComponent<SpriteRenderer>().flipX;
		up.sortingLayerName = layerName;

		go = new GameObject();
		left = go.AddComponent<SpriteRenderer>();
		go.transform.parent = transform;
		go.transform.localPosition = new Vector3(-thickness, 0f);
		go.transform.localScale = Vector3.one;
		left.material = m;
		left.sprite = GetComponent<SpriteRenderer>().sprite;
		left.flipX = GetComponent<SpriteRenderer>().flipX;
		left.sortingLayerName = layerName;

		go = new GameObject();
		down = go.AddComponent<SpriteRenderer>();
		go.transform.parent = transform;
		go.transform.localPosition = new Vector3(0f, -thickness);
		go.transform.localScale = Vector3.one;
		down.material = m;
		down.sprite = GetComponent<SpriteRenderer>().sprite;
		down.flipX = GetComponent<SpriteRenderer>().flipX;
		down.sortingLayerName = layerName;

		go = new GameObject();
		right = go.AddComponent<SpriteRenderer>();
		go.transform.parent = transform;
		go.transform.localPosition = new Vector3(thickness, 0f);
		go.transform.localScale = Vector3.one;
		right.material = m;
		right.sprite = GetComponent<SpriteRenderer>().sprite;
		right.flipX = GetComponent<SpriteRenderer>().flipX;
		right.sortingLayerName = layerName;

		Color = Color;
	}
}
