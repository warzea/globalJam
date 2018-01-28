using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heri : MonoBehaviour 
{
	public float MoveSpeedMin;
	public float MaxMoveSpeed;
	public float MoveSpeedMinDown;
	public float MaxMoveSpeedMax;
	Transform currTrans;

	public bool GoRight = false;
	public float Dest = 7;

	float MoveSpeed;
	float MoveSpeedDown;
	void Awake ( )
	{
		MoveSpeed = Random.Range (MoveSpeedMin, MaxMoveSpeed);
		//currRig = GetComponent<Rigidbody2D> ();
		currTrans = transform;
		MoveSpeedDown = Random.Range (MoveSpeedMinDown, MaxMoveSpeedMax);
		Destroy (gameObject, Dest);
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

		currTrans.localPosition -= Vector3.up * MoveSpeedDown * Time.deltaTime;
	}
}
