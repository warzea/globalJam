using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinguin : MonoBehaviour 
{
	public float MoveSpeedMin;
	public float MaxMoveSpeed;
	Transform currTrans;

	public bool GoRight = false;

	float MoveSpeed;
	void Awake ( )
	{
		MoveSpeed = Random.Range (MoveSpeedMin, MaxMoveSpeed);
		//currRig = GetComponent<Rigidbody2D> ();
		currTrans = transform;
		Destroy (gameObject, 20);
	}

	void Update () 
	{
		if (!GoRight) {
			//currRig.AddForce (Vector3.right * MoveSpeed * Time.deltaTime);
			currTrans.localPosition += Vector3.right * MoveSpeed * Time.deltaTime;
		}
		else {
			currTrans.localPosition -= Vector3.right * MoveSpeed * Time.deltaTime;
			//currRig.AddForce (-Vector3.right * MoveSpeed * Time.deltaTime);
		}
	}
}
