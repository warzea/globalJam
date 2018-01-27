using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCourse : MonoBehaviour
{
	void OnTriggerEnter2D ( Collider2D currCol )
	{
		if (currCol.tag == "Player") 
		{
			Manager.MainManager.StartDialogue ();
		} 
		else if ( currCol.tag =="Pinguin")
		{
			Manager.MainManager.currHistory--;
			Manager.MainManager.StartDialogue ();
		}
	}
}
