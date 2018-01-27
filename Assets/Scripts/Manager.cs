using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Manager : MonoBehaviour 
{
	public static Manager MainManager;
	public Transform CurrPlayer;
	public Transform TargetHistory;
	public GameObject Dialogue;
	public Text PapyDialogue;

	public HistoryInfo[] AllHistory;

	[HideInInspector]
	public int currHistory = 0;

	int currDialogue = 0;
	bool onDialogue = false;

	Player getPlayer;

	void Awake () 
	{
		MainManager = this;
		Dialogue.SetActive (false);
		getPlayer = CurrPlayer.GetComponent<Player> ();
		StartDialogue ();
	}

	public void StartDialogue ( )
	{
		getPlayer.StopPlayer = true;
		CurrPlayer.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Kinematic; 
		CurrPlayer.DOMove (TargetHistory.position, 1).OnComplete (() => 
		{
				Dialogue.SetActive(true);

				onDialogue = true;

				PapyDialogue.text = AllHistory[currHistory].DialogueHist[currDialogue];
				currDialogue ++;
		});
	}

	void endDialogue ( )
	{
		getPlayer.NewScene ();
		CurrPlayer.DOMove (AllHistory [currHistory].PosGameplay.position, 1).OnComplete (() => 
		{
			getPlayer.StopPlayer = false;
			CurrPlayer.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Dynamic; 
		});
	}

	void Update ()
	{
		if ( Input.GetKeyDown(KeyCode.Return) && onDialogue ) 
		{
			if (AllHistory [currHistory].DialogueHist.Length > currDialogue) {
				PapyDialogue.text = AllHistory [currHistory].DialogueHist [currDialogue];
				currDialogue++;
			}
			else 
			{
				Dialogue.SetActive(false);
				onDialogue = false;
				currDialogue = 0;
				endDialogue ();
				currHistory++;

				if (currHistory > AllHistory.Length - 1) 
				{
					currHistory = 0;
				}
			}
		}
	}
}

[System.Serializable]
public class HistoryInfo 
{
	public string[] DialogueHist;
	public Transform PosGameplay;
}
