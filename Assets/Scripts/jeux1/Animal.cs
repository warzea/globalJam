using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour 
{
	public bool GoRight;
	public float MoveSpeedMin;
	public float MaxMoveSpeed;
	Transform getTrans;
	// Use this for initialization
	float MoveSpeed;
	void Awake () 
	{
		MoveSpeed = Random.Range (MoveSpeedMin, MaxMoveSpeed);
		getTrans = transform;
		Destroy (gameObject, 10);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (GoRight) 
		{
			getTrans.localPosition += Vector3.right * MoveSpeed * Time.deltaTime;	
		}
		else 
		{
			getTrans.localPosition -= Vector3.right * MoveSpeed * Time.deltaTime;	
		}	
	}
}
