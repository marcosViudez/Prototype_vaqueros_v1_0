using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// clase que gira el arma segun donde se encuentre la posicion del raton en pantalla
// segun unos cuadrantes con angulos

public class turnWeapon : MonoBehaviour {

	// variables publicas
	public Image sight; 
	public float degrees;

	// variables privadas
	private RaycastHit hit;
	private Vector3 direccion;
	private Ray ray;
	private GameObject target;


	// Use this for initialization
	void Start () 
	{
		target = GameObject.FindGameObjectWithTag ("Target");
	}
	
	// Update is called once per frame
	void Update () 
	{
		// direccion = mira.transform.position - transform.position;
		direccion = target.transform.position - transform.position;


		// calculo de angulo girado para armas sprites
		float angle = Mathf.Rad2Deg * (Mathf.Atan (direccion.normalized.z / direccion.normalized.x));

		// segun los cuadrantes se calcula el angulo girado
		if (direccion.normalized.x > 0 && direccion.normalized.z > 0) {
			degrees = angle;
		} 
		if (direccion.normalized.x < 0 && direccion.normalized.z > 0) {
			degrees = 180 + angle;
		} 
		if (direccion.normalized.x < 0 && direccion.normalized.z < 0) {
			degrees = 180 + angle;
		} 
		if (direccion.normalized.x > 0 && direccion.normalized.z < 0) {
			degrees = 360 + angle;
		}

		// Debug.Log (grados + ", " + angulo);

		transform.rotation = Quaternion.Euler (0, -degrees, 0);

		ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		// crear un rayo con una altura en coordenada "Y"
		if (Physics.Raycast (transform.position,new Vector3(direccion.x, 1, direccion.z), 100.0f)) 
		{

		}

		Debug.DrawRay (transform.position,new Vector3(direccion.x, 1, direccion.z) * 1000, Color.red, 0.1f);

	}
}
