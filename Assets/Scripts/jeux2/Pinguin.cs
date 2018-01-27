using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinguin : MonoBehaviour 
{
	public float MoveSpeed;
	Rigidbody2D currRig;

	bool run = false;

	void Awake ()
	{
		currRig = GetComponent<Rigidbody2D> ();
	}
	
	void Update () 
	{
		if (run) 
		{
			currRig.AddForce (Vector3.right * MoveSpeed * Time.deltaTime);
		}
	}

	void OnTriggerEnter2D ( Collider2D currCol )
	{
		if (currCol.tag == "Player") 
		{
			run = true;
		}
	}
}
