using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHouse : MonoBehaviour {
	
	public GameObject scriptDoor;
	public GameObject letraE;

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			// Debug.Log ("abrir");
			letraE.SetActive (true);
			if (Input.GetKeyDown (KeyCode.E)) 
			{
				// Debug.Log ("ENTRAR");
				// al presionar la tecla "E" se entra en la casa
				scriptDoor.GetComponent<Positioned> ().isPositioned = true;
			}
		} 
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			letraE.SetActive (false);
		}

	}

}
