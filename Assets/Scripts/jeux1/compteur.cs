using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class compteur : MonoBehaviour 
{
	public int ObjectifPome;
	int currCompteur;


	public void NewPomme ()
	{
		currCompteur++;

		if (currCompteur == ObjectifPome) 
		{
			Manager.MainManager.StartDialogue ();
		}
	}

	public void Reset( )
	{
		currCompteur = 0;
	}
}
