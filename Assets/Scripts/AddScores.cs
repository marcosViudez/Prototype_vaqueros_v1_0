using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScores : MonoBehaviour {

	public enum typeObjects
	{
		// tipo de objetos en el juego
		bandit, woman
	}

	public typeObjects typeGameobject;
	
	// Update is called once per frame
	void Update () 
	{

		// segun el tipo de objeto damos una puntuacion y experiencia
		switch (typeGameobject) 
		{
		case typeObjects.bandit:
			GetComponent<ItemsDead>().scoreObjetcs = 5;		
			GetComponent<ItemsDead>().pointsXPEnemies = 5;
			break;

		case typeObjects.woman:
			GetComponent<ItemsDead>().scoreObjetcs = 15;
			GetComponent<ItemsDead>().pointsXPEnemies = 15;
			break;

		default:
			break;
		}
	}
}
