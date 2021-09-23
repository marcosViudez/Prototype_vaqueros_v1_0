using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponCollider : MonoBehaviour {

	private GameObject iaControl;
	public bool dropWeapon;

	void Start()
	{
		iaControl = GameObject.Find("Interface IA enemigos");
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "maloIA") 
		{
			// Debug.Log (other.gameObject.name);

			dropWeapon = true;

			// activamos que tiene el arma el enemigo que colisiona con ella 
			other.gameObject.GetComponent<referenciasMalos> ().miNavigate.GetComponent<IA_demo_2> ().haveAWeapon (dropWeapon);

			// actualizamos las listas
			iaControl.GetComponent<IA_Globales>().updateAll_Ias(true);

			// Debug.Log ("pick weapon");
			Destroy(gameObject);
		}
	}
}
