using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlTriggerPlayer : MonoBehaviour {

	// variables publicas
	public GameObject normalShot;
	public GameObject triggerGun;
	public GameObject SpriteBullet;
	public Text textBullets;
	public int numberBullets;

	// variables privadas
	private GameObject target;
	private GameObject weapon;
	private GameObject enemy;
	private GameObject iaControl;
	private float bulletDegrees;


	void Start()
	{
		target = GameObject.FindGameObjectWithTag ("Target");
		weapon = GameObject.FindGameObjectWithTag ("Pistola");
		enemy = GameObject.FindGameObjectWithTag ("agenteNavigation");
		iaControl = GameObject.Find("Interface IA enemigos");
		numberBullets = 0;

	}


	// Update is called once per frame
	void Update () 
	{
		// actualizanos lista de enemigos 
		iaControl.GetComponent<IA_Globales> ().updateListEnemies ();

		bulletDegrees = weapon.GetComponent<turnWeapon> ().degrees;
		// Debug.Log (gradosBala);

		// calculamos la distancia desde la pistola al punto de mira
		float distShot = Vector3.Distance(weapon.transform.position,target.transform.position);
		// Debug.Log (distDisparo);


		// con distDisparo >= 10 bloqueamos el disparo a esa distancia para evitar errores
		if (Input.GetMouseButtonDown (0) && distShot >= 10) 
		{
			// calculamos angulo en que giraremos el sprite de la bala
			SpriteBullet.transform.rotation = Quaternion.Euler (60, 0, bulletDegrees);
			// creamos una bala al presionar el boton izquierdo del raton
			GameObject bullet = Instantiate (normalShot,triggerGun.transform.position,Quaternion.identity) as GameObject;
			// Debug.Log("gradosBala: " + SpriteBullet.transform.rotation);
			// cambiamos nombre a los gameObjects creados
			bullet.name = "bullet";
			// activamos la IA en modo ataque de hayDisparos
			iaControl.GetComponent<IA_Globales> ().updateAll_Ias(true);
			//suma bala creada
			addBullets ();
		}

		// convierte el numero de balas a string para escribir marcador
		textBullets.text = numberBullets.ToString ();
	}

	void addBullets()
	{
		// suma balas al marcador
		numberBullets++;
	}
}
