using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisonEnemy : MonoBehaviour {

	public  GameObject enemyIA;

	void Start()
	{
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player") {
			// Debug.Log ("te tengo");
			enemyIA.GetComponent<IA_demo_2> ().canIShotToPlayer(true);

			// devuelve el nombre del enemy que me encuentra
			// Debug.Log (gameObject.transform.parent.transform.parent.name);

			// color de vision se cambia a rojo alerta
			if (enemyIA.GetComponent<IA_demo_2> ().statusAreThereShots()) {
				GetComponent<Renderer> ().material.color = new Color32 (255, 0, 0, 50);
			}
		} 
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player") {
			// Debug.Log ("te tengo");
			enemyIA.GetComponent<IA_demo_2> ().canIShotToPlayer(false);
			// color de vision se cambia a verde se mantiene tranquilo
			GetComponent<Renderer>().material.color = new Color32(0,255,0,50);

		} 
	}
}
