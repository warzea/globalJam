using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCourse : MonoBehaviour
{
	void OnTriggerEnter2D ( Collider2D currCol )
	{
		if (currCol.tag == "Player") 
		{
			foreach (SpawnPinguin thisSP in Manager.MainManager.getCam.GetComponentsInChildren<SpawnPinguin>()) {
				thisSP.StopSpawn ();

				Destroy ( thisSP.gameObject );
			}

			foreach (Pinguin thisPin in Manager.MainManager.getCam.GetComponentsInChildren<Pinguin>()) {
				Destroy ( thisPin.gameObject );
			}
			Manager.MainManager.StartDialogue ();
		} 
	}
}
