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
			currGo = (GameObject)Instantiate (Cerf,transform);
		}
		else 
		{
			currGo = (GameObject)Instantiate (Ecureuil,transform);
		}

		currGo.transform.localPosition = Vector3.zero + Vector3.up * Random.Range(-0.15f, 0.15f);

		currGo.GetComponent<Animal> ().GoRight = GoRight;

		if (!GoRight && SpawnThis == animal.cerf) 
		{
			currGo.transform.localScale = new Vector3 (-1, 1, 1);
		}
		else if (GoRight && SpawnThis == animal.ecureuil) 
		{
			currGo.transform.localScale = new Vector3 (-1, 1, 1);
		}
		StartCoroutine (waitAndSpawn ());

		Destroy (currGo, 15);
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
