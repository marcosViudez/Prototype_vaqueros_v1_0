using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSprite : MonoBehaviour {

	public GameObject objectFather;
	public float zDegrees;
	
	// Update is called once per frame
	void Update () 
	{
		// creamos una rotacion igual pero de sentido contrario que el padre del sprite
		// para mantener el sprite colocado frente a la camara
		transform.rotation = Quaternion.Euler (60, -objectFather.transform.rotation.y, zDegrees);
	}
}
