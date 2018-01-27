using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour 
{
	public Transform[] Decor;
	public float SizeParallax = 10;

	public void UpdateRight ( Transform player )
	{
		for (int a = 0; a < Decor.Length; a++) 
		{
			if (Decor [a].position.x < player.transform.position.x - SizeParallax * 2) 
			{
				Decor [a].localPosition += Vector3.right * SizeParallax * Decor.Length;
			}
		}
	}

	public void UpdateLeft ( Transform player )
	{
		for (int a = 0; a < Decor.Length; a++) 
		{
			if (Decor [a].position.x > player.transform.position.x + SizeParallax * 2) 
			{
				Decor [a].localPosition -= Vector3.right * SizeParallax * Decor.Length;
			}
		}
	}
}
