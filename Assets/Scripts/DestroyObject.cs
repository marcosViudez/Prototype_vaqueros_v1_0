﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour {

	// variables publicas
	public float timeToDestroy;

	// variables privadas

	
	// Update is called once per frame
	void Update () 
	{
		Destroy (gameObject, timeToDestroy);
	}
}
