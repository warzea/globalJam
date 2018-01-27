using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPomme : MonoBehaviour 
{
	public GameObject Pomme;
	public float MaxTimeForSpawn = 10;

	bool canSpawn = true;
	public void StartSpawn ( )
	{
		GameObject currGo = (GameObject)Instantiate (Pomme);

		StartCoroutine (waitAndSpawn ());
	}

	public void StopSpawn ( )
	{
		canSpawn = false;
	}

	IEnumerator waitAndSpawn ( )
	{
		WaitForSeconds thisSec = new WaitForSeconds(Random.Range(1, MaxTimeForSpawn ));

		yield return thisSec;

		if (canSpawn) 
		{
			StartSpawn ();
		}
	}
}
