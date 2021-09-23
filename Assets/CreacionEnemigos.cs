using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreacionEnemigos : MonoBehaviour {

	public GameObject enemigos;
	public int totalEnemigos;

	// Use this for initialization
	void Start () 
	{
		for (int i = 0; i < totalEnemigos; i++) 
		{
			GameObject malos = Instantiate (enemigos, new Vector3(Random.Range(0,100),1,Random.Range(0,100)), transform.rotation) as GameObject;
			malos.name = "Enemigo";
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
