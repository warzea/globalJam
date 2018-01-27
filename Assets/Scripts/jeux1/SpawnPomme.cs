using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPomme : MonoBehaviour 
{
	public GameObject Pomme;
	public float Forcestart = 10;
	public float MaxTimeForSpawn = 10;

	bool canSpawn = true;
	public void StartSpawn ( )
	{
		GameObject getObj = (GameObject)Instantiate (Pomme,transform);
		canSpawn = true;

		getObj.transform.localPosition = Vector3.zero;
		getObj.GetComponent<Rigidbody2D>().AddForce(new Vector2 ( Random.Range(-Forcestart, Forcestart), 0));


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
