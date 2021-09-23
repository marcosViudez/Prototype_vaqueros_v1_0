using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IA_demo_2 : MonoBehaviour {

	private NavMeshAgent agent;
	private int thingsInFloor;
	private bool countingThings;

	public GameObject target;
	public GameObject AmmoMaloIA;
	public GameObject spriteAmmoIA;
	private float degrees;
	public bool isbadShoot;
	public bool canShoot;

	public Vector3 waypoint;
	public Vector3 currentWaypoint;

	public bool haveSheriffs;
	public bool haveWeapon;
	public bool haveScarf;
	public bool shooting;
	public bool isWalking;
	public bool isArrive;
	public bool countingAll;
	public bool selectWeapon;
	public int numWaypoint;
	public int currentNumWaipoint;
	public int numWeponsDropped;

	private float distNavegacion;

	// listas de objetos que hay en el escenario
	public List<GameObject> listSheriffsInFloor = new List<GameObject>();
	public List<Vector3> posSheriffsInFloor = new List<Vector3>();

	public List<GameObject> listWaypointsScene = new List<GameObject>();
	public List<Vector3> posWaypointsScene = new List<Vector3>();

	public List<GameObject> listWeaponsScene = new List<GameObject>();
	public List<Vector3> posWeaponsScene = new List<Vector3>();



	void Start () 
	{
		currentNumWaipoint = 0;
		distNavegacion = 5;
		// iniciar componente de navegacion
		agent = GetComponent<NavMeshAgent> ();

		// actualizamos listas de objetos
		countingAll = true;
	}

	void Update () 
	{
		brain ();	
	}

	public bool statusAreThereShots()
	{
		return shooting;
	}

	public void canIShotToPlayer(bool answer)
	{
		canShoot = answer;
	}

	public void haveShooting(bool answer)
	{
		shooting = answer;
	}

	public void haveAWeapon(bool answer)
	{
		haveWeapon = answer;
	}

	public bool getWeaponIA()
	{
		return haveWeapon;
	}

	public void brain()
	{
		if (GameObject.FindGameObjectsWithTag ("sheriff").Length != 0) {
			haveSheriffs = true;
		} else {
			haveSheriffs = false;
		}

		if (countingAll) 
		{
			countObjectsScene ();
			countingAll = false;
		}


			
		if (!shooting && !haveScarf) 
		{
			// si no hay disparos ni tiene el jugador el pañuelo de ataque puesto
			if (currentWaypoint == Vector3.zero) {
				// elijo un waypoint
				newWaypoint ();
				startWalkAgent (waypoint,distNavegacion,transform.position,waypoint);
				// Debug.Log ("fase1");
			} else if (currentWaypoint != Vector3.zero && !isArrive) {
				waypoint = currentWaypoint;
				startWalkAgent (waypoint,distNavegacion,transform.position,waypoint);
				// Debug.Log ("fase2");
			} else if (currentWaypoint != Vector3.zero && isArrive) {
				newWaypoint ();
				startWalkAgent (waypoint,distNavegacion,transform.position,waypoint);
				// Debug.Log ("fase3");
			}
		}

		if (shooting || haveScarf) {
			// me paro donde este y antes guardo waypoint 
			// pruebaValentia();
			saveWaypoint (waypoint);

			if (!haveWeapon && listWeaponsScene.Count != 0) {
				searchWeapons ();
			} else {
				stopWalkAgent ();
			}

			if (haveWeapon) 
			{
				// atacar
				attackPlayer();
			}


		}
	}

	public void attackPlayer()
	{
		// Debug.Log ("atacando...");

		// angulo del target a la IA
		Vector3 angleEnemy = target.transform.position - transform.position;
		// rotamos el arma de la IA
		// calculo de angulo girado para armas sprites
		float angle = Mathf.Rad2Deg * (Mathf.Atan (angleEnemy.normalized.z / angleEnemy.normalized.x));

		// segun los cuadrantes se calcula el angulo girado
		if (angleEnemy.normalized.x > 0 && angleEnemy.normalized.z > 0) {
			degrees = angle;
		} 
		if (angleEnemy.normalized.x < 0 && angleEnemy.normalized.z > 0) {
			degrees = 180 + angle;
		} 
		if (angleEnemy.normalized.x < 0 && angleEnemy.normalized.z < 0) {
			degrees = 180 + angle;
		} 
		if (angleEnemy.normalized.x > 0 && angleEnemy.normalized.z < 0) {
			degrees = 360 + angle;
		}

		// waypoint de donde esta el target
		waypoint = target.transform.position;

		// me muevo hacia el 
		startWalkAgent (waypoint, 10, transform.position, target.transform.position);

		// si la distancia es menor que la indicada disparo al player
		if (Vector3.Distance (transform.position, target.transform.position) <= 80 && isWalking) 
		{
			// Debug.Log ("disparo");
			StartCoroutine(shootingPlayer ());
		}

		// variamos la coordenada Z para colocar sprite frente a la camara
		spriteAmmoIA.GetComponent<RotateSprite> ().zDegrees = degrees;

		// dibujamos un rayo con la direccion del disparo
		// Debug.DrawRay (new Vector3(transform.position.x, 0.0f,transform.position.z), angleMalo, Color.red, 0.1f);
	}

	public IEnumerator shootingPlayer()
	{
		if (!isbadShoot && canShoot) 
		{
			// instanciamos el sprite de la bala
			GameObject bulletEnemy = Instantiate (AmmoMaloIA, transform.position,transform.rotation) as GameObject;
			// subimos un poco coordenada "y" para que no colisione con el suelo
			bulletEnemy.transform.position = new Vector3 (transform.position.x, 1.0f,transform.position.z);
			isbadShoot = true;
			// disparamos cada segundo 1.0f
			yield return new WaitForSeconds (1.0f);
			isbadShoot = false;
		}
	}
		
	public void searchWeapons()
	{
		// busco arma tirada en el suelo 
		// Debug.Log("buscando weapons...");
		// Debug.Log(listWeaponsScene.Count);

		if (!selectWeapon)
		{
			numWeponsDropped = Random.Range (0, posWeaponsScene.Count);
			selectWeapon = true;
		}

		if (posWeaponsScene.Count != 0) {
			// cogemos la posicion del arma elegida 
			waypoint = posWeaponsScene [numWeponsDropped];

			// nos dirigimos al arma escogida
			startWalkAgent (waypoint, 2.0f, transform.position, waypoint);
		} else {
			// si no hay armas tiradas
			// Debug.Log("no hay armas !!!");
			// searchSheriffs ();
		}
	}
		
	int numberRandom()
	{
		// generamos un numero random
		numWaypoint = Random.Range (0, posWaypointsScene.Count);		// total de waypoints en la lista
		// Debug.Log ("num = " + numWaypoint);

		// creamos un numero random diferente al que tenemos en currentNumWaipoint
		if (currentNumWaipoint != numWaypoint) 
		{
			return numWaypoint;		// retornamos el numero obtenido
		} else {
			numberRandom ();		// llamamos recursivamente para crear otro numWaypoint diferente
		}
		return 0;
	}

	void newWaypoint()
	{
		// elijo un waypoint aleatoriamente y me desplazo
		if (!isWalking) 
		{	
			numberRandom();						// buscamos el numero random obtenido del metodo
			currentNumWaipoint = numWaypoint;	// grabamos el numero obtenido en una variable

			// waypoint de la lista 
			waypoint = posWaypointsScene [numWaypoint];
		}
	}

	void saveWaypoint(Vector3 saveWaypoint)
	{
		// guardamos waypoint
		isArrive = false;
		currentWaypoint = saveWaypoint;		
	}

	public void stopWalkAgent()
	{
		// detiene el moviemiento del agente
		agent.speed = 0;
		agent.acceleration = 0;
		isWalking = false;
		agent.SetDestination (transform.position);	
		// Debug.Log ("parado");
	}

	void startWalkAgent(Vector3 destino, float distanciaParada, Vector3 objetoOrigen, Vector3 objetoDestino)
	{
		isWalking = true;
		// se mueve hacia el target
		agent.speed = 10;
		agent.acceleration = 5;

		// calculamos el angulo entre el player y el malo para que se miren entre ellos
		/*
		Vector3 angulo = transform.position - destino;
		Quaternion direccion = Quaternion.LookRotation (angulo);
		transform.rotation = Quaternion.Slerp (transform.rotation, direccion, velocidadGiro * Time.deltaTime);
		*/

		// movemos al agente
		agent.SetDestination (destino);	
		// Debug.Log ("andando");

		// objetoOrigen es el malo, objetoDestino a donde quiere llegar
		if (Vector3.Distance (objetoOrigen, objetoDestino) <= distanciaParada && isWalking) {
			isArrive = true;
			stopWalkAgent ();		// nos paramos al llegar al waypoint

		}
	}


	public void countObjectsScene()
	{
		// borramos listas
		clearListsScene ();

		// contamos todos los objetos en la escena segun su tag
		countingThings = false;
		countThings("sheriff");
		countingThings = false;
		countThings("waypoint");
		countingThings = false;
		countThings("weapon");
	}

	private void clearListsScene()
	{
		// borramos las listas para actualizarlas
		listSheriffsInFloor.Clear();
		posSheriffsInFloor.Clear ();
		listWaypointsScene.Clear ();
		posWaypointsScene.Clear ();
		listWeaponsScene.Clear ();
		posWeaponsScene.Clear ();

	}
		
	public void countThings(string objetos)
	{
		// metodo contar un tipo de objetos que este sobre el escenario
		if (!countingThings) 
		{
			if (GameObject.FindGameObjectsWithTag (objetos).Length != 0) 
			{
				// contamos el numero de sheriffs en escenario
				thingsInFloor = GameObject.FindGameObjectsWithTag (objetos).Length;
				// Debug.Log (objetos + "= " + thingsInFloor);


				if (objetos == "sheriff") 
				{
					// añadimos todos los sheriffs a la lista
					listSheriffsInFloor.AddRange (GameObject.FindGameObjectsWithTag (objetos));

					for (int i = 0; i < thingsInFloor; i++) {
						// coseguimos las posiciones de los sheriffs
						posSheriffsInFloor.Add (listSheriffsInFloor [i].transform.position);
					}
				}

				if (objetos == "waypoint") 
				{
					// añadimos todos los sheriffs a la lista
					listWaypointsScene.AddRange (GameObject.FindGameObjectsWithTag (objetos));

					for (int i = 0; i < thingsInFloor; i++) {
						// coseguimos las posiciones de los sheriffs
						posWaypointsScene.Add (listWaypointsScene [i].transform.position);
					}
				}

				if (objetos == "weapon") 
				{
					// añadimos todos los sheriffs a la lista
					listWeaponsScene.AddRange (GameObject.FindGameObjectsWithTag (objetos));

					for (int i = 0; i < thingsInFloor; i++) {
						// coseguimos las posiciones de los sheriffs
						posWeaponsScene.Add (listWeaponsScene [i].transform.position);
					}
				}

				countingThings = true;
			}
		}
	}
}
