using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : MonoBehaviour
{
	public void Play()
	{
		StartCoroutine(Run());
	}

	IEnumerator Run()
	{
		Vector3 orig = transform.position;
		SpriteRenderer s = GetComponent<SpriteRenderer>();
		Color baseColor = s.color;
		Color targetColor = new Color(baseColor.r, baseColor.g, baseColor.g, 0f);


		for (float time = 0f; time < 1f; time += Time.deltaTime)
		{
			transform.position = orig + Vector3.right * Mathf.Sin(time * time * 40f) * (time * 0.1f + 0.03f);
			s.color = Color.Lerp(baseColor, targetColor, time);
			yield return null;
		}

		gameObject.SetActive(false);
	}
}
