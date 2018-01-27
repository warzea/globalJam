using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pomme : MonoBehaviour 
{
	void Awake () 
	{
		Destroy (gameObject, 10);
	}

	void OnCollisionEnter2D ( Collision2D thisCol )
	{
		if (thisCol.gameObject.layer == 9) 
		{
			Destroy (gameObject, 2);
		}
	}
}
