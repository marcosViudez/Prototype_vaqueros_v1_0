using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickItems : MonoBehaviour {

	// variables publicas

	public GameObject letraE;

	// tipo de items
	public enum typeItems
	{
		heart, bullets, weapons
	}
	public typeItems typeItem;

	// variables privadas
	private GameObject items;
	private float distanceItem;
	private float distanceToItem;	


	// Use this for initialization
	void Start () 
	{
		typeItem = typeItems.heart;
		distanceItem = 4.0f;
		letraE.SetActive(false);
		items = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (items != null) 
		{
			// distancia del player al item
			distanceToItem = Vector3.Distance (items.transform.position, transform.position);
		}

		// si la distancia al item es menor de 5 se activa la tecla a pulsar para pickup
		if (distanceToItem < distanceItem) {
			letraE.SetActive (true);
		} else {
			letraE.SetActive (false);
		}

		// pickup items con letra "E" si usa teclado + raton
		if (Input.GetKeyDown (KeyCode.E) && distanceToItem < distanceItem) 
		{
			// elimina el item cogido
			Destroy (gameObject);
		}
	}

	// colisones con las balas en objetos
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Terrain") 
		{
			// Debug.Log ("suelo");
			// cuando los items tocan el suelo desactivamos la gravedad para que no se muevan ni sean empujados por el player
			GetComponent<Rigidbody>().useGravity = false;
			GetComponent<Rigidbody>().isKinematic = true;
		}
	}
}
