using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour 
{
	public float MaxSpeed = 10;
	public float MoveSpeed = 10;
	public float JumpForce = 10;
	public parallax SecondJeu;
	public GameObject FirstGame;
	public GameObject ThirdGame;

	public int Life = 3;

	[HideInInspector]
	public int CurrHist = 0;

	[HideInInspector]
	public bool StopPlayer = false;

	Animator getAnimator;
	Transform currTrans;
	Rigidbody2D currRig;

	IEnumerator currEnum;
	IEnumerator getEnumPos;

	Vector3 lastPos;
	Vector3 newPos;

	string currTag;

	bool onObstacle = false;
	bool onGround = false;
	bool useJump = false;
	bool canTakeDmg = true;

	// Use this for initialization
	void Awake () 
	{
		currRig = GetComponent<Rigidbody2D> ();
		currTrans = transform;
		getEnumPos = getPos ();
		getAnimator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (StopPlayer) 
		{
			return;
		}
		float getAxe = Input.GetAxis ("Horizontal");
		float onAir = 1;

		if (!onGround || currTag == "SnowGround")
		{
			onAir = 0.5f;
		}

		if (getAxe == 0) 
		{
			getAnimator.SetBool ("Walk", false);
			getAnimator.SetBool ("Run", false);
		}

		if (getAxe > 0) {
			//currTrans.localPosition += Vector3.right * MoveSpeed * Time.deltaTime;
			getAnimator.SetBool ("Walk", true);
			if (CurrHist == 1) 
			{
				SecondJeu.UpdateRight (currTrans);
			}
			currRig.AddForce (Vector3.right * MoveSpeed * Time.deltaTime * onAir);
		} else if (getAxe < 0) {
			getAnimator.SetBool ("Walk", true);
			//currTrans.localPosition -= Vector3.right * MoveSpeed * Time.deltaTime;
			if (CurrHist == 1) 
			{
				SecondJeu.UpdateLeft (currTrans);
			}
			currRig.AddForce (-Vector3.right * MoveSpeed * Time.deltaTime * onAir);
		} 
		else if (onGround && currTag == "StandardGround") 
		{
			currRig.velocity *= (1 - Time.deltaTime * 5);
		}

		if ((100 * Mathf.Abs (currRig.velocity.x) / MaxSpeed) > 70 && getAxe != 0) 
		{
			getAnimator.SetBool ("Run", true);
		}
		else 
		{
			getAnimator.SetBool ("Run", false);
		}

		if ( Mathf.Abs ( currRig.velocity.x ) > MaxSpeed) 
		{
			if (currRig.velocity.x < 0)
			{
				currRig.velocity = new Vector2 ( -MaxSpeed, currRig.velocity.y );
			} 
			else 
			{
				currRig.velocity = new Vector2 ( MaxSpeed, currRig.velocity.y );
			}
		}

		if (Input.GetButtonDown ("Jump") && !useJump) 
		{
			useJump = true;
			currEnum = waitJump ();
			StartCoroutine (currEnum);
			currRig.AddForce (Vector3.up * JumpForce);
		}
	}

	void takeDamage ( )
	{
		if (!canTakeDmg) 
		{
			return;
		}

		currRig.velocity = Vector3.zero;
		currRig.bodyType = RigidbodyType2D.Kinematic;
		StopPlayer = true;
		canTakeDmg = false;
		Life--;
		StopCoroutine (getEnumPos);

		if (Life == 0) 
		{
			DOVirtual.DelayedCall (2, () => 
			{
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			});
		} 
		else 
		{
			GetComponent<SpriteRenderer> ().DOFade (0, 0.2f).OnComplete (() => {
				GetComponent<SpriteRenderer> ().DOFade (1, 0.2f);
			});

			if (!onObstacle) 
			{
				onObstacle = false;
				canTakeDmg = true;
				StopPlayer = false;
				getEnumPos = getPos ();
				StartCoroutine (getEnumPos);
				currRig.bodyType = RigidbodyType2D.Dynamic;
				return;
			} 

			Vector3 getBack;
			if (Vector2.Distance (newPos, currTrans.localPosition) < 3) 
			{
				getBack = lastPos;
			} 
			else 
			{
				getBack = newPos;
			}

			if (currTrans.position.x < getBack.x) 
			{
				StartCoroutine (checkDuringDmg (true));
			}
			else
			{
				StartCoroutine (checkDuringDmg (false));
			}

			currTrans.DOLocalMove (getBack, 1).OnComplete (() => 
			{
				canTakeDmg = true;
				StopPlayer = false;
				getEnumPos = getPos();
				currRig.bodyType = RigidbodyType2D.Dynamic;

				StartCoroutine(getEnumPos);
			});
		}
	}

	IEnumerator checkDuringDmg ( bool right )
	{
		WaitForEndOfFrame thisF = new WaitForEndOfFrame ();

		while (!canTakeDmg) 
		{
			yield return thisF;
			if (right)
			{
				SecondJeu.UpdateRight (currTrans);
			} 
			else 
			{
				SecondJeu.UpdateLeft (currTrans);
			}

		}
	}

	IEnumerator waitJump ( )
	{
		WaitForSeconds thisSec = new WaitForSeconds (1);

		yield return thisSec;

		if (onGround) 
		{
			useJump = false;
		}
	}

	IEnumerator getPos ( bool first = true)
	{
		WaitForSeconds thisSec = new WaitForSeconds (2);

		yield return thisSec;

		newPos = currTrans.localPosition;

		if (!first) 
		{
			lastPos = newPos;
		} 

		getEnumPos = getPos (!first);

		StartCoroutine (getEnumPos);
	}

	void OnCollisionEnter2D ( Collision2D thisCol )
	{
		if (thisCol.gameObject.tag == "Ennemis") 
		{
			takeDamage ();
		}
		else if (thisCol.gameObject.tag == "Obsacle") 
		{
			onObstacle = true;
			takeDamage ();
		}
		else if (thisCol.gameObject.tag == "Pomme") 
		{
			FirstGame.GetComponent<compteur> ().NewPomme ();
			Destroy (thisCol.gameObject);
		}
	}

	void OnTriggerEnter2D ( Collider2D currCol )
	{
		if (currCol.gameObject.layer == 9) 
		{
			useJump = false;

			onGround = true;
			currTag = currCol.tag;
		}
	}

	void OnTriggerExit2D ( Collider2D currCol )
	{
		if (currCol.gameObject.layer == 9 && useJump ) 
		{
			onGround = false;
		}	
	}

	public void NewScene ( )
	{
		if (CurrHist == 0) 
		{
			FirstGame.SetActive (true);
			FirstGame.GetComponent<compteur> ().Reset ();

			DOVirtual.DelayedCall (1, () => {
				foreach ( Spawn thisSpawn in FirstGame.GetComponentsInChildren<Spawn>())
				{
					thisSpawn.StartSpawn();
				}
			});
		} 
		else if (CurrHist == 1) 
		{
			FirstGame.SetActive (false);
			SecondJeu.gameObject.SetActive (true);
			StartCoroutine(getEnumPos);
		}
		else if (CurrHist == 2) 
		{
			SecondJeu.gameObject.SetActive (false);
			ThirdGame.gameObject.SetActive (true);
		}

		CurrHist++;
	}
}
