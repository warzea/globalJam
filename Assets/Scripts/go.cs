using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class go : MonoBehaviour {

	bool go2 = false;
	// Update is called once per frame
	void Update () {

		if (Input.anyKeyDown && !go2) {
			go2 = true;
			SceneManager.LoadScene (1);
		}
		
	}
}
