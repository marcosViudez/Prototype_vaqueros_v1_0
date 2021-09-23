using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// clase para hacer saltar items de un objeto

public class ItemsDead : MonoBehaviour {

	// variables publicas
	public bool isDead;
	public GameObject itemUp;		// sprite gameobject item
	public GameObject bloodParticles;
	public GameObject score;
	public GameObject agentNAV;
	public Canvas miCanvas;
	public int scoreObjetcs;
	public int pointsXPEnemies;

	// variables privadas
	private bool isJumpEnable;		// booleana de que saltes
	private float timeCollider;
	private float jumpSpeed;			// velocidad de fuerza

	// rayo
	private RaycastHit hit;
	private Vector3 rayDirection;
	private Ray ray;

	// estructuras y arrays publicos
	public int quantityItems;			// array de items que guarda un NPJ y los suelta al morir

	[System.Serializable]
	public struct mochilaItems
	{
		public bool isJumping;
		public GameObject Items;
		public LayerMask layerItem;
	};
		
	public mochilaItems[] ItemsJump;

	// Use this for initialization
	void Start () 
	{
		timeCollider = 0.1f;
		jumpSpeed = 20f;										// velocidad con la que salta
		ItemsJump = new mochilaItems[quantityItems];			// inicializamos el array segun el numero de items
		score.SetActive(false);									// desactivamos la puntuacion del objeto

		for (int i = 0; i < quantityItems; i++) 
		{
			ItemsJump [i].Items = itemUp;								// añadimos nuestro sprite
			ItemsJump[i].isJumping = true;								// activamos true al jumping
			ItemsJump[i].layerItem = LayerMask.GetMask("items");			// seleccionamos layer items
		}
	}

	void Update()
	{
		StartCoroutine (ejectItems ());

		// Debug.Log (transform.position);
	}

	IEnumerator ejectItems()
	{
		if (isDead) 
		{
			for (int j = 0; j < quantityItems; j++) 
			{
				ItemsJump[j].Items = Instantiate (ItemsJump [j].Items, transform.position, transform.rotation) as GameObject;
				ItemsJump[j].Items.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(4,12),Random.Range(25,35),Random.Range(4,12)) * jumpSpeed);
				ItemsJump [j].Items.GetComponent<SphereCollider> ().isTrigger = true;
				// ItemsJump [j].Items.GetComponent<Transform> ().Rotate (new Vector3(60,0,0));		// cambiamos el angulo segun el que tenga la mainCamera
				ItemsJump [j].isJumping = false;
			}

			isDead = false;

			for (int k = 0; k < quantityItems; k++) 
			{
				yield return new WaitForSeconds (timeCollider);
				ItemsJump [k].Items.GetComponent<SphereCollider> ().isTrigger = false;
			}
		}
	}


	// colisones con las balas en objetos
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "bala") 
		{
			// Debug.Log ("disparado");
		}
	}

	// colisiones con objetos del escenario
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "bullet") 
		{
			// Debug.Log ("muerto");
			isDead = true;
			// contar puntos al morir y hacerlo hijo del canvas para visualizarlo
			// activamos puntuacion
			score.SetActive(true);									
			score.GetComponent<ScoreDeads> ().addPoints (scoreObjetcs);
			// desactivamos el trigguer del gameobject
			GetComponent<SphereCollider>().enabled = false;
			// instanciamos la sangre al morir
			Instantiate (bloodParticles, transform.position, transform.rotation);
			// destruimos el gameobject despues de la animacion hay que contar con el tiempo de animacion de los puntos
			Destroy(gameObject,0.5f);			// destruimos el gameobject2D
			Destroy(agentNAV,0.5f);				// destruimos la IA 3D
		}
	}
		
}
	