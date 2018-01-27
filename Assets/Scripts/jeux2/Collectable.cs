using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour 
{

	void OnTriggerEnter2D ( Collider2D thisColl )
	{
		if (thisColl.tag == "Player") 
		{
			Destroy (this);
		}
	}
}
