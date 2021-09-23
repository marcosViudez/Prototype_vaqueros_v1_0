using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IA_Globales : MonoBehaviour {

	// variables de clase
	private bool updateIAs;
	private GameObject target;


	public List<GameObject> listEnemiesInScene = new List<GameObject>();


	// Use this for initialization
	void Start () 
	{
		target = GameObject.FindGameObjectWithTag ("Player");
		updateListEnemies ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		// actualizamos parametros de enemigos
		updateParametersIAs ();
	}

	public void updateAll_Ias(bool answer)
	{
		// metodo para acceder a la variable
		updateIAs = answer;
	}

	public void updateListEnemies()
	{
		// borramos y actualizamos
		clearList ();
		// actualiza mi lista de enemigos por fase
		listEnemiesInScene.AddRange (GameObject.FindGameObjectsWithTag ("agenteNavigation"));
	}

	public void clearList()
	{
		// borramos y actualizamos lista
		listEnemiesInScene.Clear();
	}

	public void updateParametersIAs()
	{
		if (updateIAs) 
		{
			for (int i = 0; i < listEnemiesInScene.Count; i++) 
			{
				// pone el parametro de alerta de que estan disparando a true 
				listEnemiesInScene [i].GetComponent<IA_demo_2> ().haveShooting (true);

				// actualizamos las listas de objetos en la escena
				listEnemiesInScene [i].GetComponent<IA_demo_2> ().countObjectsScene ();


				// actualiza las posiciones de las armas que quedan en el suelo una vez actualizadas las armas ya recogidas
				if (listEnemiesInScene [i].GetComponent<IA_demo_2> ().haveWeapon == false) 
				{
					listEnemiesInScene [i].GetComponent<IA_demo_2> ().selectWeapon = false;
					listEnemiesInScene[i].GetComponent<IA_demo_2> ().searchWeapons ();
				}

			}
		}
		updateIAs = false;
	}
}
