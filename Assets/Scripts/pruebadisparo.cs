using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pruebadisparo : MonoBehaviour {

	public GameObject bala;
	public bool dispara;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		StartCoroutine(disparar ());
	}

	IEnumerator disparar()
	{
		if (!dispara) 
		{
			Instantiate (bala, transform.position, transform.rotation);
			dispara = true;
			yield return new WaitForSeconds (1.0f);
			dispara = false;
		}
	}
}
