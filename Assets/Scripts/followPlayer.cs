using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour 
{
	public Transform Player;
	public bool FollowX;
	public bool FollowY;

	Transform getT;

	void Awake ( )
	{
		getT = transform;
	}
	// Update is called once per frame
	void Update () 
	{
		if ( FollowX )
		{
			getT.position = new Vector3 ( Player.position.x, getT.position.y, -10);
		}

		if ( FollowY )
		{
			getT.position = new Vector3 ( getT.position.x, Player.position.y + 1, -10 );
		}
	}
}
