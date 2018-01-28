using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPinguin : MonoBehaviour 
{
	public GameObject Pinguin;
	public float MaxTimeSpawn = 5;
	public float MinTimeSpawn = 1.5f;
	public bool Right = false;

	IEnumerator getEnum;

	List<GameObject> gets;
	bool stopSpawn = false;
	public void StartSpawn ()
	{
		gets = new List<GameObject> ();
		stopSpawn = false;
		getEnum = respawnPinguin();

		StartCoroutine (getEnum);
	}

	public void StopSpawn ()
	{
		stopSpawn = true;
		if (getEnum != null) {
			StopCoroutine (getEnum);
		}

		foreach (GameObject thisIn in gets) {
			Destroy (thisIn);
		}
	}

	IEnumerator respawnPinguin ( )
	{
		yield return new WaitForSeconds (Random.Range (MinTimeSpawn, MaxTimeSpawn));

		getEnum = respawnPinguin ();

		if (!stopSpawn) {
			GameObject currGo = (GameObject)Instantiate (Pinguin);
			gets.Add (currGo);
			currGo.transform.position = transform.position;
			currGo.GetComponent<Pinguin> ().GoRight = Right;
			if (Right ) 
			{
				currGo.transform.localScale = new Vector3 (-1, 1, 1);
			}

			StartCoroutine (getEnum);
		}
	
	}
}
