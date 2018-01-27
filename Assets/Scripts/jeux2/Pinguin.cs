using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinguin : MonoBehaviour 
{
	public float MoveSpeed;
	Rigidbody2D currRig;

	Vector3 getPos;
	bool run = false;

	void Awake ( )
	{
		currRig = GetComponent<Rigidbody2D> ();
		getPos = transform.localPosition;
	}

	void OnEnable ()
	{
		transform.localPosition = getPos;
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
