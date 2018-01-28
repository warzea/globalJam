using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endEsca : MonoBehaviour {

	void OnTriggerEnter2D ( Collider2D currCol )
	{
		if (currCol.tag == "Player") 
		{
			foreach (Spawnheri thisSP in Manager.MainManager.getCam.GetComponentsInChildren<Spawnheri>()) {
				thisSP.StopSpawn ();

				Destroy ( thisSP.gameObject );
			}

			foreach (heri thisPin in Manager.MainManager.getCam.GetComponentsInChildren<heri>()) {
				Destroy ( thisPin.gameObject );
			}

			Manager.MainManager.StartDialogue ();
		} 
	}
}
