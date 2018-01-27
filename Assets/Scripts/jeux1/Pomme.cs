using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pomme : MonoBehaviour 
{
	public float LifeTime;
	void Awake () 
	{
		Destroy (gameObject, LifeTime);
	}
}
