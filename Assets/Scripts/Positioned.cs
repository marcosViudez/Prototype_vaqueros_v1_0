using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Positioned : MonoBehaviour {

	public GameObject player;
	public GameObject outerDoor;
	public Image fade;
	private GameObject scriptCamera;
	public GameObject letraE;

	public string objeto;
	public bool isPositioned;
	public bool iscloseDoor;
	private float smoothCamera;

	// Use this for initialization
	void Start ()
	{
		smoothCamera = 0.3f;
		scriptCamera = GameObject.FindGameObjectWithTag ("MainCamera");
	}
	// Update is called once per frame
	void Update () 
	{
		if (isPositioned) 
		{
			StartCoroutine(ChangeSpeedCamera ());
			player.transform.position = transform.position + new Vector3 (0, 0.5f, 0);
			isPositioned = false;
			iscloseDoor = true;
		}
	}

	void fadeOffScreen(byte bitStart)
	{
		// oscurece la pantalla al entrar en la habitacion
		fade.GetComponent<Image> ().color = new Color32 (0, 0, 0, bitStart);
	}

	IEnumerator ChangeSpeedCamera()
	{
		// cambio de velocidad del seguimiento de la camara para cambiar de habitacion
		scriptCamera.GetComponent<CameraController> ().smooth = 30f;
		fadeOffScreen (255);
		yield return new WaitForSeconds (smoothCamera);
		scriptCamera.GetComponent<CameraController> ().smooth = 1.5f;
		fadeOffScreen (0);
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player" && !iscloseDoor) 
		{
			// Debug.Log ("salgo");
			letraE.SetActive (true);
			if (Input.GetKeyDown (KeyCode.E)) 
			{
				StartCoroutine (ChangeSpeedCamera ());
				player.transform.position = outerDoor.transform.position + new Vector3 (0, 0.5f, 0);
				iscloseDoor = true;
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			iscloseDoor = false;
			// Debug.Log ("entro");
		}
	}
}
