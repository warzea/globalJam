using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class pinguinSpe : MonoBehaviour 
{

	Animator thisAnim;
	void Awake () 
	{
		thisAnim = GetComponent<Animator> ();
		LoopAnim ();
	}

	void LoopAnim ( )
	{
		DOVirtual.DelayedCall (Random.Range (0.5f, 2.5f), () => {
			thisAnim.SetBool("saut",false);
			thisAnim.SetBool("coucou", false);	
			int getRange = Random.Range(0,3 ) ;
			if ( getRange == 1 )
			{
				thisAnim.SetBool("saut",true);
			}
			else if ( getRange == 0)
			{
				thisAnim.SetBool("coucou", true);	
			}
		}).OnComplete (LoopAnim);
	}
}
