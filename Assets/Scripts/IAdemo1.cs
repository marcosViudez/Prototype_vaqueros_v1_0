using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAdemo1 : MonoBehaviour {

	private NavMeshAgent agente;

	public GameObject target;
	public GameObject AmmoMaloIA;
	public GameObject spriteAmmoIA;
	public Vector3 waypoint;
	public Vector3 currentWaypoint;
	private float distNavegacion;
	private float degrees;
	private float changeAngle;
	private int cantidadArmasEncontradas;
	private int cantidadSheriffsPueblo;
	private int cantidadWaypointsPueblo;
	private int numArmasTiradas;

	[Space]
	public int numWaypoint;
	public int currentNumWaipoint = 0;
	public float velocidadGiro;

	[Space]
	[Header ("Listas Objetos : ")]
	// lista de waypoints que rrecorre el malo
	// public GameObject[] listaWaypoints = new GameObject[3];

	// lista de waypoints que rrecorre el malo
	public List<GameObject> listaWaypointsEscenario = new List<GameObject>();
	public List<Vector3> posicionesWaypointsEscenario = new List<Vector3>();

	// listas de armas que hay tiradas en el escenario
	public List<GameObject> listaArmasEscenario = new List<GameObject>();
	public List<Vector3> posicionArmasEscenario = new List<Vector3>();

	// lista de sherrifs en el pueblo
	public List<GameObject> listaSheriffsEscenario = new List<GameObject>();
	public List<Vector3> posicionSheriffsEscenario = new List<Vector3>();

	[Space]
	[Header ("Preguntas de la IA : ")]
	// preguntas de la IA
	public bool isDead;
	public bool hayDisparos;
	public bool hayPañuelo;
	public bool meCruzoNPC;
	public bool reconocePlayer;
	public bool tengoArma;
	public bool haySheriff;
	public bool encuentroArma;
	public bool estoyMuerto;
	public bool moviendose;
	public bool estoyEnDestino;
	public bool contarArmas;
	public bool contarSheriffs;
	public bool contarWaypoint;
	public bool isbadShoot;
	public bool canShoot;
	public bool armaSeleccionada;
	public bool tirarValentia;

	[Space]
	[Header ("Caracteristicas IA : ")]

	[Range(0,100)]
	public int valentia;		// valor del personaje si es 100 es valiente si es 0 es timido
	public int tiradaValentia;

	[Range(0,100)]
	public int paciencia;		// si es 100 es muy nervioso y puede disparar con facilidad

	[Range(0,100)]
	public int sociable;		// si es muy sociable se para hablar con la gente




	// Use this for initialization
	void Start () 
	{
		velocidadGiro = 3.0f;
		// listaWaypoints = new GameObject[totalWaypoints];
		agente = GetComponent<NavMeshAgent> ();									// iniciar componente de navegacion
		distNavegacion = 5;
		agente.stoppingDistance = distNavegacion;

		// asiganamos valores a las caracteristicas de cada agente
		asignarCaracteristicas ();
		// añadimos todos los waypoints al iniciar el pueblo
		contarWaypoints ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Debug.Log("waypoints: " + posicionesWaypointsEscenario.Count);
		// metodo de decisiones del agente
		cerebro ();	

		if (isDead) {
			pararse ();
		}
	}

	void asignarCaracteristicas()
	{
		valentia = Random.Range (0, 100);
		paciencia = Random.Range (0, 100);
		sociable = Random.Range (0, 100);
	}

	// ------------------------------------------------
	// metodos set y get de algunas variables privadas
	public void setTengoArma(bool estado)
	{
		tengoArma = estado;
	}

	public bool estadoHayDisparos()
	{
		return hayDisparos;
	}

	public void estanDisparando(bool respuesta)
	{
		hayDisparos = respuesta;
	}

	public void puedoDisparar(bool respuesta)
	{
		canShoot = respuesta;
	}
	// ------------------------------------------------
	// ------------------------------------------------

	void cerebro()
	{

		contarArmasTiradas();
		contarSheriff();
		// comprobamos si hay sheriffs en el pueblo
		if (GameObject.FindGameObjectsWithTag ("sheriff").Length != 0) {
			haySheriff = true;
		} else {
			haySheriff = false;
		}

		if (!hayDisparos && !hayPañuelo) 
		{
			if (currentWaypoint == Vector3.zero) {
				// elijo un waypoint
				nuevoWaypoint ();
				moverse (waypoint,distNavegacion,transform.position,waypoint);
				// Debug.Log ("fase1");
			} else if (currentWaypoint != Vector3.zero && !estoyEnDestino) {
				waypoint = currentWaypoint;
				moverse (waypoint,distNavegacion,transform.position,waypoint);
				// Debug.Log ("fase2");
			} else if (currentWaypoint != Vector3.zero && estoyEnDestino) {
				nuevoWaypoint ();
				moverse (waypoint,distNavegacion,transform.position,waypoint);
				// Debug.Log ("fase3");
			}

			if (meCruzoNPC) 
			{
				// guardo waypoint
				guardarWaypoint(waypoint);
				// hablar o no depende random
				// cargo ultimo waypoint
			}

			if (reconocePlayer) 
			{
				// tengo armas
			}
		}

		if (hayDisparos || hayPañuelo) 
		{
			// me paro donde este y antes guardo waypoint 
			pruebaValentia();
			guardarWaypoint(waypoint);
			pararse ();

			if (!tengoArma) {
				if (!haySheriff) {
					// busco arma tirada
					// contarArmasTiradas();
					buscarArmas();
				} else {
					// busco al sheriff waypoint
					// contarSheriff();
					buscarSheriffs ();
				}
			} else {
				// ataco busco y persigo
				atacar();
			}
		}
	}

	void pruebaValentia()
	{
		if (!tirarValentia) 
		{
			// calculamos un intento de valentia para el agente
			tirarValentia = true;
			tiradaValentia = Random.Range (0, 100);
		}
	}

	void guardarWaypoint(Vector3 saveWaypoint)
	{
		estoyEnDestino = false;
		currentWaypoint = saveWaypoint;		// guardamos waypoint
	}

	void contarWaypoints()
	{
		// añadimos waypoints de la escena a la lista
		if (!contarWaypoint) 
		{
			if (GameObject.FindGameObjectsWithTag ("waypoint").Length != 0) 
			{
				// cantidad de waypoints en escenario
				cantidadWaypointsPueblo = GameObject.FindGameObjectsWithTag ("waypoint").Length;

				// añado a la lista todos los gameobjects waypoints
				listaWaypointsEscenario.AddRange(GameObject.FindGameObjectsWithTag ("waypoint"));

				for(int i = 0; i < cantidadWaypointsPueblo ; i++)
				{
					// coseguimos las posiciones de los waypoints
					posicionesWaypointsEscenario.Add(listaWaypointsEscenario[i].transform.position);
				}

				// terminamos de contar
				contarWaypoint = true;

			}
		}

	}

	void contarSheriff()
	{
		// contamos cuantos sheriffs hay en el escenario
		if (!contarSheriffs) 
		{
			if (GameObject.FindGameObjectsWithTag ("sheriff").Length != 0) 
			{
				// cantidad de sheriffs en escenario
				cantidadSheriffsPueblo = GameObject.FindGameObjectsWithTag ("sheriff").Length;

				// añado a la lista todos los gameobjects sheriffs
				listaSheriffsEscenario.AddRange(GameObject.FindGameObjectsWithTag ("sheriff"));

				for(int i = 0; i < cantidadSheriffsPueblo ; i++)
				{
					// coseguimos las posiciones de los sheriffs
					posicionSheriffsEscenario.Add(listaSheriffsEscenario[i].transform.position);
				}

				// terminamos de contar
				contarSheriffs = true;
			}
		}
	}

	void buscarSheriffs()
	{
		Debug.Log ("estoy buscando sheriff.....");

	}

	void contarArmasTiradas()
	{
		// contamos las armas que estan tiradas por el escenario 
		if (!contarArmas) 
		{
			if (GameObject.FindGameObjectsWithTag ("armas").Length != 0) 
			{
				// cantidad de armas en escenario
				cantidadArmasEncontradas = GameObject.FindGameObjectsWithTag ("armas").Length;

				// añado armas del escenario tiradas
				listaArmasEscenario.AddRange(GameObject.FindGameObjectsWithTag ("armas"));
			
				for(int i = 0; i < cantidadArmasEncontradas ; i++)
				{
					// coseguimos las posiciones de las armas
					posicionArmasEscenario.Add(listaArmasEscenario[i].transform.position);
				}

				// finalizo el conteo de armas
				contarArmas = true;
				// Debug.Log("cantidad: " + GameObject.FindGameObjectsWithTag ("armas").Length);
			}
		}

	}
		
	void buscarArmas()
	{
		// Debug.Log ("busco armas...");
		// busco arma tirada en el suelo 
		armasTiradas();

	}

	void armasTiradas()
	{
		if (!armaSeleccionada)
		{
			numArmasTiradas = Random.Range (0, posicionArmasEscenario.Count);
			armaSeleccionada = true;
		}

		if (posicionArmasEscenario.Count != 0) {
			// cogemos la posicion del arma elegida 
			waypoint = posicionArmasEscenario [numArmasTiradas];

			// nos dirigimos al arma escogida
			moverse (waypoint, 2.0f, transform.position, waypoint);
		} else {
			// si no hay armas tiradas
			Debug.Log("no hay armas !!!");
			buscarSheriffs ();
		}
	}

	void atacar()
	{
		// Debug.Log ("atacando...");

		// angulo del target a la IA
		Vector3 angleMalo = target.transform.position - transform.position;
		// rotamos el arma de la IA
		// calculo de angulo girado para armas sprites
		float angle = Mathf.Rad2Deg * (Mathf.Atan (angleMalo.normalized.z / angleMalo.normalized.x));

		// segun los cuadrantes se calcula el angulo girado
		if (angleMalo.normalized.x > 0 && angleMalo.normalized.z > 0) {
			degrees = angle;
		} 
		if (angleMalo.normalized.x < 0 && angleMalo.normalized.z > 0) {
			degrees = 180 + angle;
		} 
		if (angleMalo.normalized.x < 0 && angleMalo.normalized.z < 0) {
			degrees = 180 + angle;
		} 
		if (angleMalo.normalized.x > 0 && angleMalo.normalized.z < 0) {
			degrees = 360 + angle;
		}

		// waypoint de donde esta el target
		waypoint = target.transform.position;

		// me muevo hacia el 
		moverse (waypoint, 10, transform.position, target.transform.position);

		// si la distancia es menor que la indicada disparo al player
		if (Vector3.Distance (transform.position, target.transform.position) <= 80 && moviendose) 
		{
			// Debug.Log ("disparo");
			StartCoroutine(disparar ());
		}

		// variamos la coordenada Z para colocar sprite frente a la camara
		spriteAmmoIA.GetComponent<RotateSprite> ().zDegrees = degrees;

		// dibujamos un rayo con la direccion del disparo
		// Debug.DrawRay (new Vector3(transform.position.x, 0.0f,transform.position.z), angleMalo, Color.red, 0.1f);
	}

	IEnumerator disparar()
	{
		if (!isbadShoot && canShoot) 
		{
			// instanciamos el sprite de la bala
			GameObject balaMalo = Instantiate (AmmoMaloIA, transform.position,transform.rotation) as GameObject;
			// subimos un poco coordenada "y" para que no colisione con el suelo
			balaMalo.transform.position = new Vector3 (transform.position.x, 1.0f,transform.position.z);
			isbadShoot = true;
			// disparamos cada segundo 1.0f
			yield return new WaitForSeconds (1.0f);
			isbadShoot = false;
		}
	}

	void hablarConNPCs()
	{
		Debug.Log ("estoy hablando...");
	}

	int numeroRandom()
	{
		// creamos un numero random
		numWaypoint = Random.Range (0, posicionesWaypointsEscenario.Count);		// total de waypoints en la lista
		// Debug.Log ("num = " + numWaypoint);

		// creamos un numero random diferente al que tenemos en currentNumWaipoint
		if (currentNumWaipoint != numWaypoint) 
		{
			return numWaypoint;		// retornamos el numero obtenido
		} else {
			numeroRandom ();		// llamamos recursivamente para crear otro numWaypoint diferente
		}
		return 0;
	}

	void nuevoWaypoint()
	{
		// elijo un waypoint aleatoriamente y me desplazo
		if (!moviendose) 
		{	
			numeroRandom();						// buscamos el numero random obtenido del metodo
			currentNumWaipoint = numWaypoint;	// grabamos el numero obtenido en una variable
			// estoyEnDestino = false;
			// Debug.Log ("curr = " + currentNumWaipoint);
			// waypoint = listaWaypoints [numWaypoint].transform.position;

			// waypoint de la lista 
			waypoint = posicionesWaypointsEscenario [numWaypoint];
				
		}
	}

	void pararse()
	{
		// detiene el moviemiento del agente
		agente.speed = 0;
		agente.acceleration = 0;
		moviendose = false;
		agente.SetDestination (transform.position);	
		// Debug.Log ("parado");
	}

	void moverse(Vector3 destino, float distanciaParada, Vector3 objetoOrigen, Vector3 objetoDestino)
	{
		moviendose = true;
		// se mueve hacia el target
		agente.speed = 10;
		agente.acceleration = 5;

		// calculamos el angulo entre el player y el malo para que se miren entre ellos
		/*
		Vector3 angulo = transform.position - destino;
		Quaternion direccion = Quaternion.LookRotation (angulo);
		transform.rotation = Quaternion.Slerp (transform.rotation, direccion, velocidadGiro * Time.deltaTime);
		*/

		// movemos al agente
		agente.SetDestination (destino);	
		// Debug.Log ("andando");

		// objetoOrigen es el malo, objetoDestino a donde quiere llegar
		if (Vector3.Distance (objetoOrigen, objetoDestino) <= distanciaParada && moviendose) {
			estoyEnDestino = true;
			pararse ();		// nos paramos al llegar al waypoint

		}
	}
}
