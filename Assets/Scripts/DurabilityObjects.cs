using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DurabilityObjects : MonoBehaviour {

	public enum typeAssets
	{
		// tipo de objetos en el juego
		barrel, lamp
	}

	public typeAssets assetsAtrezzo;	// tipo de assets del juego

	// variables publicas
	public int durability;				// resistencia del objeto
	public int lifeObject;				// vida restante del objeto

	// variables privadas



	// Use this for initialization
	void Start () 
	{
		assignDurabilityAssets ();
		lifeObject = durability;
	}
	
	// Update is called once per frame
	void Update () 
	{
		assignDurabilityAssets ();

		if (lifeObject <= 0) 
		{
			destroyObject ();
		}
	}

	void subtractDurability(int daño)
	{
		// me resta durabilidad del objeto elegido segun el daño del arma que tengas
		if (lifeObject > 0) 
		{	
			lifeObject = lifeObject - daño;
		}
	}

	void assignDurabilityAssets()
	{
		switch (assetsAtrezzo) 
		{
		case typeAssets.barrel:
			durability = 20;
			break;

		case typeAssets.lamp:
			durability = 15;
			break;


		default:
			break;
		}
	}

	void destroyObject()
	{
		// cambia de sprite a modo destruido
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "bullet") 
		{
			// Debug.Log ("colider planta");
			// resto durabilidad con daño uno
			subtractDurability(1);				
		}
	}
}

