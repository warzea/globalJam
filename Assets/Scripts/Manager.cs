using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Manager : MonoBehaviour 
{
	public static Manager MainManager;
	public Image BlackScreen;
	public Transform CurrPlayer;
	public Transform TargetHistory;
	public GameObject Dialogue;
	public Text PapyDialogue;
	public AudioClip PapyStory;
	public Animator EnfantAnim;
	public GameObject Credit;
	public HistoryInfo[] AllHistory;
	public Transform ChildHist;

	[HideInInspector]
	public int currHistory = 0;

	int currDialogue = 0;
	float getOrtho;
	bool onDialogue = false;
	bool sleep = false;
	bool jeuTermine = false;

	Player getPlayer;
	AudioSource getAudio;
	public Camera getCam;

	void Awake () 
	{
		getAudio = GetComponent<AudioSource> ();
		MainManager = this;
		getPlayer = CurrPlayer.GetComponent<Player> ();
		getOrtho = getCam.orthographicSize;
		getAudio.clip = PapyStory;
		getAudio.Play();
		CurrPlayer.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Kinematic;
		getPlayer.StopPlayer = true;
		getCam.transform.position = TargetHistory.position; 
		getCam.transform.localPosition = new Vector3 (getCam.transform.localPosition.x, getCam.transform.localPosition.y, -10);

		Dialogue.SetActive(true);

		onDialogue = false;
		PapyDialogue.DOText ( AllHistory [currHistory].DialogueHist [currDialogue], 1 ).OnComplete(()=>
		{
			onDialogue = true;
			currDialogue++;
		});
	}

	public void StartDialogue ( )
	{
		getCam.GetComponent<followPlayer>().FollowX = false;
		getCam.GetComponent<followPlayer>().FollowY = false;
		getAudio.DOPitch (0, 0.5f).OnComplete (() => {
			getAudio.clip = PapyStory;
			getAudio.Play();
			getAudio.DOPitch(1, 0.5f);
		});

		CurrPlayer.GetComponent<CapsuleCollider2D> ().enabled = false;

		BlackScreen.DOKill ();
		BlackScreen.DOFade (1, 0.3f);
		getPlayer.StopPlayer = true;


		CurrPlayer.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Kinematic; 
		CurrPlayer.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;

		getCam.transform.DOMove ( CurrPlayer.position, 0.3f );
		getCam.DOOrthoSize (0, 0.3f).OnComplete (() => {
			getCam.transform.position = TargetHistory.position; 
			getCam.transform.localPosition = new Vector3 (getCam.transform.localPosition.x, getCam.transform.localPosition.y, -10);

			BlackScreen.DOFade (0, 0.3f);
				
			getCam.DOOrthoSize (getOrtho, 0.3f).OnComplete (() => {
				Dialogue.SetActive(true);

				onDialogue = true;

				if ( !jeuTermine)
				{
					PapyDialogue.text = AllHistory[currHistory].DialogueHist[currDialogue];
				}
				else
				{
					if ( jeuTermine )
					{
						PapyDialogue.DOText ("That's all... After that I came home", 1).OnComplete (() => {
							DOVirtual.DelayedCall(3, ()=>
								{
									PapyDialogue.text = string.Empty;
									PapyDialogue.DOText ("but the apples were reduced to porridge...",1).OnComplete (() => {
										DOVirtual.DelayedCall(3, ()=>
											{
												PapyDialogue.text = string.Empty;

												PapyDialogue.DOText ("And that’s how I invented apple compote...",1).OnComplete (() => 
													{
														DOVirtual.DelayedCall(3, ()=>
															{
																Credit.SetActive(true);
															});
													});
											});

									});
								});
						});
					}
				}

				currDialogue ++;
			});
		});;
	}
		
	void endDialogue ( )
	{
		getPlayer.NewScene ();

		getAudio.DOPitch (0, 0.5f).OnComplete (() => {
			getAudio.clip = AllHistory [currHistory].ClipGameplay;
			getAudio.Play();
			getAudio.DOPitch(1, 0.5f);
		});

		BlackScreen.DOKill ();
		BlackScreen.DOFade (1, 0.3f);

		getCam.transform.DOMove ( ChildHist.position, 0.3f );
		getCam.DOOrthoSize (0, 0.3f).OnComplete (() => {
			getCam.transform.DOKill();
			CurrPlayer.position = AllHistory [currHistory].PosGameplay.position;
			getCam.transform.localPosition =new Vector3 (  AllHistory [currHistory].PosGameplay.position.x,  AllHistory [currHistory].PosGameplay.position.y, -10);
			getPlayer.glisse = false;
			getPlayer.JumpForce = 250;

			if ( currHistory == 0 || currHistory == 3 )
			{
				//getCam.transform.localPosition = new Vector3(0,0,-10);
				//getCam.transform.SetParent(null);
				getCam.GetComponent<followPlayer>().FollowX = false;
				getCam.GetComponent<followPlayer>().FollowY = false;
			}
			else if ( currHistory == 1 )
			{
				getPlayer.JumpForce = 175;
				getPlayer.glisse = true;
				getCam.transform.localPosition = new Vector3( getCam.transform.localPosition.x, 1, -10);
				//getCam.transform.SetParent(getPlayer.transform);
				//getCam.transform.localPosition = new Vector3(0,1.1f,-10);
				getCam.GetComponent<followPlayer>().FollowX = true;
				getCam.GetComponent<followPlayer>().FollowY = false;
			}
			else
			{
				//getCam.transform.SetParent(getPlayer.transform);
				//getCam.transform.localPosition = new Vector3(0,0,-10);
				getCam.GetComponent<followPlayer>().FollowX = false;
				getCam.GetComponent<followPlayer>().FollowY = true;
			}

			currHistory++;

			if (currHistory > AllHistory.Length - 1) 
			{
				jeuTermine = true;
				currHistory = 0;

				PapyDialogue.text = string.Empty;
			}


			EnfantAnim.SetBool ("startSleep", false);
			EnfantAnim.SetBool ("sleep", false);

			BlackScreen.DOFade (0, 0.3f);
			getCam.DOOrthoSize (getOrtho, 0.3f).OnComplete (() => {
				getPlayer.StopPlayer = false;
				CurrPlayer.GetComponent<CapsuleCollider2D> ().enabled = true;

				CurrPlayer.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Dynamic;

			});
		});;
	}

	bool checkEnd = false;
	void Update ()
	{
		if ( jeuTermine )
		{
			if (!checkEnd) {
				checkEnd = true;
			}

			return;
		}
		if ( Input.anyKeyDown && onDialogue ) 
		{
			if (AllHistory [currHistory].DialogueHist.Length - 1> currDialogue) 
			{
				onDialogue = false;

				if (Random.Range (0, 2) == 0 && currDialogue > 1 && !sleep) {
					sleep = true;
					//getCam.DOOrthoSize (getOrtho * 0.5f, 0.35f);
					//getCam.transform.DOLocalMoveY(getCam.transform.localPosition.y + 0.1f, 0.3f); 

					EnfantAnim.SetBool ("startSleep", true);

					BlackScreen.DOFade (0.5f, 0.5f).OnComplete (() => {
						//getCam.DOOrthoSize (getOrtho, 0.2f);
						//getCam.transform.DOLocalMoveY (getCam.transform.localPosition.y - 0.1f, 0.2f); 
						BlackScreen.DOFade (0, 0.5f);
						EnfantAnim.SetBool ("startSleep", false);
						PapyDialogue.text = string.Empty;

						PapyDialogue.DOText(AllHistory [currHistory].DialogueHist [currDialogue], 1 ).OnComplete(()=>
						{
							onDialogue = true;
							currDialogue++;
						});
					});
				} 
				else 
				{
					EnfantAnim.SetBool ("startSleep", false);
					sleep = false;
					PapyDialogue.text = string.Empty;
					PapyDialogue.DOText ( AllHistory [currHistory].DialogueHist [currDialogue], 1 ).OnComplete(()=>
					{
						onDialogue = true;
						currDialogue++;
					});
				}
			}
			else 
			{
				onDialogue = false;
				PapyDialogue.text = AllHistory [currHistory].DialogueHist [currDialogue ];
				sleep = false;
				EnfantAnim.SetBool ("sleep", true);

				DOVirtual.DelayedCall(2, ()=>
				{
					Dialogue.SetActive(false);
					currDialogue = 0;
					endDialogue ();

				});
				
			}
		}
	}
}

[System.Serializable]
public class HistoryInfo 
{
	public string[] DialogueHist;
	public Transform PosGameplay;
	public AudioClip ClipGameplay;
}
