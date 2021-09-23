using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoEnemy : MonoBehaviour {

	private float speedBullet;
	private float lifeBullet;

	void Start()
	{
		speedBullet = 70.0f;		// velocidad de la bala
		lifeBullet = 2.0f;			// tiempo de vida de la bala
	}
	
	// Update is called once per frame
	void Update ()
	{
		// movemos la bala segun la velocidad marcada
		transform.Translate (Vector3.forward * Time.deltaTime * speedBullet);
		// destruimos la bala 
		Destroy (gameObject, lifeBullet);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			// restamos vida al player
			Debug.Log ("impacto !!!!");
		}
	} 
		
}
