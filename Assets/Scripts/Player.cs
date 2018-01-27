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
	public ThoseBack[] Montagne;

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

	float getDist;
	public bool glisse = false;
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
			getAnimator.SetBool ("Walk", false);
			getAnimator.SetBool ("Run", false);
			return;
		}

		float getAxe = Input.GetAxis ("Horizontal");
		float onAir = 1;

		if (!onGround) {

		} 

		if ( glisse )
		{
			getAnimator.SetBool ( "Glisse", true );
			onAir = 0.35f;
		}
		else if ( !onGround)
		{
			onAir = 0.5f;
		}

		if (CurrHist == 2) 
		{
			checkPos (getDist - currTrans.localPosition.x);
			getDist = currTrans.localPosition.x;
		}

		if (getAxe == 0) 
		{
			getAnimator.SetBool ("Walk", false);
			getAnimator.SetBool ("Run", false);
		}

		if (getAxe > 0) {
			//currTrans.localPosition += Vector3.right * MoveSpeed * Time.deltaTime;
			currTrans.localScale = new Vector3 ( 1,1,1);
			getAnimator.SetBool ("Walk", true);
			if (CurrHist == 2) 
			{
				SecondJeu.UpdateRight (currTrans);
			}
			currRig.AddForce (Vector3.right * MoveSpeed * Time.deltaTime * onAir);
		} else if (getAxe < 0) {
			currTrans.localScale = new Vector3 ( -1,1,1);

			getAnimator.SetBool ("Walk", true);
			//currTrans.localPosition -= Vector3.right * MoveSpeed * Time.deltaTime;
			if (CurrHist == 2) 
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

	float speed = 0.25f;
	void checkPos ( float newPos )
	{
		if ( newPos == 0 )
		{
			return;
		}
		if ( newPos > 0 )
		{
			speed = -0.25f;
		}
		float a = 0;
		foreach (ThoseBack thisBack in Montagne) 
		{
			foreach (Transform thisT in thisBack.BackG) 
			{
				thisT.localPosition += new Vector3 (speed * a * Time.deltaTime, 0, 0);

				if (newPos < 0) 
				{
					if ( Mathf.Abs ( currTrans.localPosition.x - thisT.localPosition.x ) > 7 && currTrans.localPosition.x > thisT.localPosition.x ) 
					{
						thisT.localPosition += new Vector3 (18.66f, 0,0);
					}
				} 
				else if (newPos > 0) 
				{
					if ( Mathf.Abs ( currTrans.localPosition.x - thisT.localPosition.x ) > 7 && currTrans.localPosition.x < thisT.localPosition.x ) 
					{
						thisT.localPosition -= new Vector3 (18.66f, 0,0);
					}
				}
			}

			a+=0.25f;
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
			Manager.MainManager.BlackScreen.DOFade (1, 1.8f);

			DOVirtual.DelayedCall (2, () => 
			{
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			});
		} 
		else 
		{
			GetComponent<SpriteRenderer> ().DOFade (0, 0.3f).OnComplete (() => {
				GetComponent<SpriteRenderer> ().DOFade (1, 0.3f);
			});

			StartCoroutine (getDmg ());

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

	IEnumerator getDmg()
	{
		yield return new WaitForSeconds (1);

		waitDmg = true;
	}

	bool waitDmg = true;
	void OnCollisionEnter2D ( Collision2D thisCol )
	{
		if (!waitDmg) {
			return;
		}
		if (thisCol.gameObject.tag == "Ennemis") 
		{
			waitDmg = false;
			takeDamage ();
			Physics2D.IgnoreCollision (GetComponent<Collider2D> (), thisCol.gameObject.GetComponent<Collider2D>(), true);
		}
		else if (thisCol.gameObject.tag == "Obsacle") 
		{
			onObstacle = true;
			waitDmg = false;
			takeDamage ();

			Physics2D.IgnoreCollision (GetComponent<Collider2D> (), thisCol.gameObject.GetComponent<Collider2D>(), true);
		}
		else if (thisCol.gameObject.tag == "Pomme") 
		{
			Physics2D.IgnoreCollision (GetComponent<Collider2D> (), thisCol.gameObject.GetComponent<Collider2D>(), true);
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
		Life = 3;
		getDist = currTrans.localPosition.x;
		if (CurrHist == 0) 
		{
			FirstGame.SetActive (true);
			FirstGame.GetComponent<compteur> ().Reset ();

			DOVirtual.DelayedCall (1, () => {
				foreach ( Spawn thisSpawn in FirstGame.GetComponentsInChildren<Spawn>())
				{
					thisSpawn.StartSpawn();
				}

				foreach ( SpawnPomme thisSpawn in FirstGame.GetComponentsInChildren<SpawnPomme>())
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

[System.Serializable]
public class ThoseBack
{
	public Transform[] BackG;
}