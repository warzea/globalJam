using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class compteurNINJA : MonoBehaviour {

	public int Objectifnin;
	int currCompteur;


	public void Newnin ()
	{
		currCompteur++;

		if (currCompteur == Objectifnin) 
		{
			Manager.MainManager.StartDialogue ();
		}
	}

	public void Reset( )
	{
		currCompteur = 0;
	}
}
