using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class positionNav : MonoBehaviour {

	public GameObject NAV;

	// Update is called once per frame
	void Update () 
	{
		// igualamos la posicion del malo con la de su navegacion 
		transform.position = new Vector3(NAV.transform.position.x, 0.6f, NAV.transform.position.z);
	}
}
