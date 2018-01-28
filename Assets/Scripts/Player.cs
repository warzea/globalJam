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
	public GameObject FourthGame;
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
	GameObject getObjChild;
	string currTag;

	bool escalade = false;
	bool onObstacle = false;
	bool onGround = false;
	bool useJump = false;
	bool canTakeDmg = true;

	float getDist;
	public bool glisse = false;
	// Use this for initialization
	void Awake () 
	{
		getObjChild = transform.GetChild (0).gameObject;
		currRig = GetComponent<Rigidbody2D> ();
		currTrans = transform;
		getEnumPos = getPos ();
		getAnimator = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetButtonDown ("Fire1") && !escalade && !glisse ) 
		{
			getAnimator.SetTrigger ("punch");
		}
		if (StopPlayer) 
		{
			getAnimator.SetBool ("Walk", false);
			getAnimator.SetBool ("Run", false);
			return;
		}



		float getAxe = Input.GetAxis ("Horizontal");
		float onAir = 1;

		if ( glisse )
		{
			getAnimator.SetBool ( "Glisse", true );
			onAir = 0.45f;
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

		if (getAxe == 0 ) 
		{
			getAnimator.SetBool ("Walk", false);
			getAnimator.SetBool ("Run", false);

			if (escalade) 
			{
				currRig.velocity = new Vector2 ( 0, currRig.velocity.y );
			}
		}

		if (getAxe > 0) {
			//currTrans.localPosition += Vector3.right * MoveSpeed * Time.deltaTime;
			currTrans.localScale = new Vector3 ( 1,1,1);
			if (!escalade) {
				getAnimator.SetBool ("Walk", true);
			}
			if (CurrHist == 2) 
			{
				SecondJeu.UpdateRight (currTrans);
			}
			currRig.AddForce (Vector3.right * MoveSpeed * Time.deltaTime * onAir);
		} else if (getAxe < 0) {
			currTrans.localScale = new Vector3 ( -1,1,1);

			if (!escalade) {
				getAnimator.SetBool ("Walk", true);
			}
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

		if (escalade && currRig.velocity.y > MaxSpeed * 0.25f) 
		{
			currRig.velocity = new Vector2 ( currRig.velocity.x, MaxSpeed * 0.25f );
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

		if (Input.GetButtonDown ("Jump") && !useJump&& !escalade) 
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
			getAnimator.SetTrigger ("dead");

			DOVirtual.DelayedCall (1, () => 
			{
					Manager.MainManager.BlackScreen.DOFade (1, 1.8f).OnComplete(()=>
					{
						SceneManager.LoadScene(SceneManager.GetActiveScene().name);
					});
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

		if (currCol.gameObject.tag == "sword") {
			if (waitDmg) {
				waitDmg = false;
				takeDamage ();
			}
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
		MaxSpeed = 2;
		glisse = false;
		escalade = false;
		currRig.gravityScale = 1;
		getDist = currTrans.localPosition.x;
		getAnimator.SetBool ("grimp", false);
		getAnimator.SetBool ("Glisse", false);
		getAnimator.SetBool ("Walk", false);
		getAnimator.SetBool ("Run", false);
		getAnimator.SetBool ("stopGrimp", true);

		if (CurrHist == 0) {
			FirstGame.SetActive (true);
			FirstGame.GetComponent<compteur> ().Reset ();

			DOVirtual.DelayedCall (1, () => {
				foreach (Spawn thisSpawn in FirstGame.GetComponentsInChildren<Spawn>()) {
					thisSpawn.StartSpawn ();
				}

				foreach (SpawnPomme thisSpawn in FirstGame.GetComponentsInChildren<SpawnPomme>()) {
					thisSpawn.StartSpawn ();
				}
			});
		} else if (CurrHist == 1) {
			glisse = true;
			MaxSpeed = 4;
			getAnimator.SetBool ("Glisse", true);
			FirstGame.SetActive (false);
			SecondJeu.gameObject.SetActive (true);
			StartCoroutine (getEnumPos);
		} else if (CurrHist == 2) {
			glisse = false;
			getAnimator.SetBool ("stopGrimp", false);
			getAnimator.SetBool ("grimp", true);
			SecondJeu.gameObject.SetActive (false);
			ThirdGame.gameObject.SetActive (true);
			currRig.gravityScale = -1;
			escalade = true;
		} 
		else if (CurrHist == 3)
		{
			ThirdGame.gameObject.SetActive (false);
			FourthGame.gameObject.SetActive (true);
		}

		CurrHist++;
	}
}

[System.Serializable]
public class ThoseBack
{
	public Transform[] BackG;
}