using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class spawnNin : MonoBehaviour 
{
	public GameObject ninja;
	public float MaxTimeForSpawn = 10;

	bool canSpawn = true;
	void Awake ( )
	{
		DOVirtual.DelayedCall (0.5f, () => {
			StartSpawn ();
		});
	}

	public void StartSpawn ( )
	{
		GameObject getObj = (GameObject)Instantiate (ninja,transform);
		canSpawn = true;

		getObj.transform.localPosition = Vector3.zero;

		StartCoroutine (waitAndSpawn ());
	}

	public void StopSpawn ( )
	{
		canSpawn = false;
	}

	IEnumerator waitAndSpawn ( )
	{
		WaitForSeconds thisSec = new WaitForSeconds(Random.Range(2, MaxTimeForSpawn ));

		yield return thisSec;

		if (canSpawn) 
		{
			StartSpawn ();
		}
	}

}
