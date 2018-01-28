using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startEscalade : MonoBehaviour 
{
	bool start = false;
	void OnTriggerEnter2D ( Collider2D currCol )
	{
		if (start) {
			return;
		}
		start = true;
		if (currCol.tag == "Player") 
		{
			foreach (Spawnheri thisSP in Manager.MainManager.getCam.GetComponentsInChildren<Spawnheri>()) {
				thisSP.StartSpawn ();
			};
		} 
	}
}
