using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnheri : MonoBehaviour 
{
	public GameObject Herisson; 

	public float MaxTimeSpawn = 5;
	public float MinTimeSpawn = 1.5f;
	public bool Right = false;

	IEnumerator getEnum;

	public void StartSpawn ()
	{
		getEnum = respawnPinguin();

		StartCoroutine (getEnum);
	}

	public void StopSpawn ()
	{
		if (getEnum != null) {
			StopCoroutine (getEnum);
		}
	}

	IEnumerator respawnPinguin ( )
	{
		yield return new WaitForSeconds (Random.Range (MinTimeSpawn, MaxTimeSpawn));

		getEnum = respawnPinguin ();

		GameObject currGo = (GameObject)Instantiate (Herisson,transform);
		currGo.transform.localPosition = Vector3.zero;
		currGo.GetComponent<heri> ().GoRight = Right;
		if (Right ) 
		{
			currGo.transform.localScale = new Vector3 (-1, 1, 1);
		}

		StartCoroutine (getEnum);
	}
}
