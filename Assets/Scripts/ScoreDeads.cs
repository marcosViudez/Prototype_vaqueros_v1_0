using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDeads : MonoBehaviour {

	// variables publicas


	// variables privadas


	public void addPoints(int points)
	{
		// marca los puntos de cada gameobject segun el int de los ptos
		this.GetComponent<TextMesh>().text = points.ToString();

	}

	public void removePoints()
	{
		// desaparece los puntos
		Destroy (gameObject);
	}
}
