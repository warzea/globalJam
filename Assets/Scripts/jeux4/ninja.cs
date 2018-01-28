using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ninja : MonoBehaviour 
{
	public float MaxSpeed = 10;
	public float MoveSpeed = 10;
	bool walk = false;
	public bool CanTakeDmg = false;

	bool goLeft = false;
	bool goRight = false;
	bool alive = true;
	bool canAttack = true;
	Rigidbody2D thisRig;
	Transform getPlay;
	Animator curranim;
	bool goAttack = false;
	// Use this for initialization
	void Start () 
	{
		thisRig = GetComponent<Rigidbody2D> ();

		DOVirtual.DelayedCall (0.7f, () => {
			CanTakeDmg = true;
		});

		DOVirtual.DelayedCall (Random.Range(1.5f, 2), () => 
		{
				goAttack = true;
		});

		getPlay = Manager.MainManager.CurrPlayer;
		curranim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!alive|| !CanTakeDmg) 
		{
			return;
		}

		walk = false;
		if (goRight) 
		{
			walk = true;
			transform.localScale = new Vector3 (0.4f, 0.4f, 0.4f);
			thisRig.AddForce (-Vector3.right * MoveSpeed * Time.deltaTime );
		}
		else if (goLeft) 
		{
			walk = true;
			transform.localScale = new Vector3 (-0.4f, 0.4f, 0.4f);
			thisRig.AddForce (Vector3.right * MoveSpeed * Time.deltaTime );
		}

		curranim.SetBool ("marche",walk);

		if ( Mathf.Abs ( thisRig.velocity.x ) > MaxSpeed) 
		{
			if (thisRig.velocity.x < 0)
			{
				thisRig.velocity = new Vector2 ( -MaxSpeed, thisRig.velocity.y );
			} 
			else 
			{
				thisRig.velocity = new Vector2 ( MaxSpeed, thisRig.velocity.y );
			}
		}

		if (goAttack) 
		{
			if (Vector3.Distance (getPlay.position, transform.position) > 0.25f) {
				if (getPlay.position.x > transform.position.x) {
					goLeft = true;
					goRight = false;
				} else {
					goLeft = false;
					goRight = true;
				}
			} else if (canAttack) {
				canAttack = false;
				curranim.SetTrigger ("attack");

				DOVirtual.DelayedCall (Random.Range (1.5f, 3f), () => {
					canAttack = true;
				});
			} else {
				goLeft = false;
				goRight = false;
			}


		}
	}

	void Action ( )
	{
		
	}

	void OnTriggerEnter2D ( Collider2D currCol )
	{
		if (currCol.tag == "punch"&& CanTakeDmg&&alive) 
		{
			alive = false;
			GetComponent<Collider2D> ().enabled = false;
			if (currCol.transform.parent.position.x > transform.position.x) {
				thisRig.AddForce (-Vector3.right * 1000);
				thisRig.AddForce (Vector3.up * Random.Range(0,1000));

			} else {
				thisRig.AddForce (Vector3.right * 1000);
				thisRig.AddForce (Vector3.up * Random.Range(0,1000));
			}

			GetComponentInParent<compteurNINJA> ().Newnin ();

			Destroy (gameObject, 2);
		}
	}


	void OnTriggerStay2D ( Collider2D currCol )
	{
		if (currCol.tag == "punch"&& CanTakeDmg && alive) 
		{
			alive = false;
			GetComponent<Collider2D> ().enabled = false;
			if (currCol.transform.parent.position.x > transform.position.x) {
				thisRig.AddForce (-Vector3.right * 1000);
				thisRig.AddForce (Vector3.up * Random.Range(0,1000));

			} else {
				thisRig.AddForce (Vector3.right * 1000);
				thisRig.AddForce (Vector3.up * Random.Range(0,1000));
			}

			GetComponentInParent<compteurNINJA> ().Newnin ();

			Destroy (gameObject, 2);
		}
	}
}
