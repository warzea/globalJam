using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour 
{
	public animal SpawnThis;
	public GameObject Ecureuil;
	public GameObject Cerf;
	public bool GoRight;
	public float MaxTimeForSpawn = 10;

	bool canSpawn = true;
	public void StartSpawn ( )
	{
		GameObject currGo ;

		if (SpawnThis == animal.cerf) 
		{
			currGo = (GameObject)Instantiate (Cerf);
		}
		else 
		{
			currGo = (GameObject)Instantiate (Ecureuil);
		}

		currGo.GetComponent<Animal> ().GoRight = GoRight;
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


public enum animal 
{
	cerf,
	ecureuil
}
