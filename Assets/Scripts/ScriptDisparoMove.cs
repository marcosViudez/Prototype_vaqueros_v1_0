using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// clase que mueve la bala disparada por el arma
public class ScriptDisparoMove : MonoBehaviour {

	// variables publicas


	// variables privadas 
	private float alturaDisparo;
	private float speed;					// velocidad de la bala
	private GameObject target;
	private Vector3 direccion;

	// Use this for initialization
	void Start () 
	{
		speed = 100.0f;
		alturaDisparo = 0.0f;

		target = GameObject.FindGameObjectWithTag ("Target");

		direccion = target.transform.position - transform.position;			// calculamos la direccion del disparo

	}
	
	// Update is called once per frame
	void Update () 
	{
		// movemos la bala en una altura determinada 
		transform.Translate((new Vector3(direccion.normalized.x,alturaDisparo,direccion.normalized.z)) * speed * Time.deltaTime);
		// se destruye el objeto al segundo de crearse
		Destroy (gameObject, 1);							
	}

	void OnTriggerEnter(Collider other)
	{
		
		// Debug.Log ("me destruyo");

		// destruimos la bala si colisiona con cualquier gameobject del escenario
		if(other.gameObject.tag != "vision")
		{
			// si son diferentes de vision
			Destroy (gameObject);
		}
		
	}
		
}
