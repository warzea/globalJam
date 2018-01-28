using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCourse : MonoBehaviour 
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
			foreach (SpawnPinguin thisSP in Manager.MainManager.getCam.GetComponentsInChildren<SpawnPinguin>()) {
				thisSP.StartSpawn ();
			};
		} 
	}
}
