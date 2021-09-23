using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Control de movimientos y animaciones para el personaje principal
/// </summary>
public class PlayerController : MonoBehaviour {

    private Animator anim;              // Parámetro para cargar las animaciones
    private Transform target;           // Parámetro que almacena valores de Target
	// Variables de control para los INPUT en movimiento con Teclado
    private float Input_X;          	
    private float Input_Z;
    
	// Parámetros de control para las animaciones (cambia según posición en eje (+)(-)X o (+)(-)Z)
    private float posx; float posz; 
	public float speed ;        // Velocidad para movimiento

    private bool isKeyPressed;          // Boolean para determinar si el Teclado es pulsado
	public bool isQuietlyLooking;		// Variable booleana para lowSpeed
	private bool isRotate;       		// Boolean para determinar si el Player gira
    public bool IsRotate {
        get { return isRotate; }
        set { isRotate = value; }
    }
    private static bool isAwake = true; // Boolean para determinar si el Player está despierto
    public static bool IsAwake {
        get { return isAwake; }
        set { isAwake = value; }
    }
	private string lookAt;				// 
	public LayerMask layerMask;                 

    void Start () {
        anim = GetComponentInChildren<Animator>();
        target = GameObject.Find("Target").transform;
    }

	void Update () {
		loadParamsBoolAnimator ();
		if (isMovingOnKeypressed()) moveKeyPressed(Input_X, Input_Z);                 // Mueve con teclado
		loadParamsFloatAnimator(posx, posz);
		rotatePlayer();
		changeSpeed (Input_X, Input_Z);

	}

	/// <summary>
	/// Loads the parameters bool animator.
	/// </summary>
	public void loadParamsBoolAnimator() {
		anim.SetBool("isKeyPressed", isKeyPressed);
		//anim.SetBool("isRotate", isRotate);
	}

	/// <summary>
	/// Loads the parameters float animator.
	/// </summary>
	/// <param name="px">Px.</param>
	/// <param name="pz">Pz.</param>
	public void loadParamsFloatAnimator(float px, float pz) {
		anim.SetFloat("x", px);
		anim.SetFloat("z", pz);
	}

	/// <summary>
	/// Changes the speed.
	/// </summary>
	/// <param name="posx">Posx.</param>
	/// <param name="posz">Posz.</param>
	public void changeSpeed(float posx, float posz) {
		speed = ((posx == 1f && posz == 0f) && lookAt.Equals("LEFT") || (posx == -1f && posz == 0f) && lookAt.Equals("RIGHT") 
			|| (posx == 0f && posz == -1f) && lookAt.Equals("BACK") ||  (posx == 0f && posz == 1f) && lookAt.Equals("FRONT") 
			|| isQuietlyLooking) ? 6.5f : 11.0f;
	}

	/// <summary>
	/// Ised keypress.
	/// </summary>
	/// <returns><c>true</c>, if keypress was ised, <c>false</c> otherwise.</returns>
	public bool isMovingOnKeypressed() {
		Input_X = Input.GetAxisRaw("Horizontal");
		Input_Z = Input.GetAxisRaw("Vertical");
		return isKeyPressed = (Mathf.Abs(Input_X) + Mathf.Abs(Input_Z)) > 0.5f;    // mueve si tecla pulsada
	}
		
	/// <summary>
	/// Moves the key pressed.
	/// </summary>
	/// <param name="posx">Posx.</param>
	/// <param name="posz">Posz.</param>
	private void moveKeyPressed(float posx, float posz) {
        isRotate = false;
		loadParamsFloatAnimator (posx, posz);
        transform.Translate(new Vector3(posx,0f,posz).normalized * speed * Time.deltaTime, Space.World);
    }

	/// <summary>
	/// Moves the mouse clicked.
	/// </summary>
	private void rotatePlayer() {
        float distTargetToPlayer = Vector3.Distance(transform.position, target.position);
		if (distTargetToPlayer > 0.1f) {
			return;
		} else
			isRotate = false;
    }

	/// <summary>
	/// Changes the direction.
	/// </summary>
	/// <param name="posTarget">Position target.</param>
    public void changeDirection(Vector3 posTarget) {
		float distPlayer = Vector3.Distance(transform.position, posTarget);
		//if (distPlayer > 0.5f) {         
			Vector3 pos = this.transform.position - posTarget;
			// Animacion atras
			if ((Mathf.Abs(pos.x) < Mathf.Abs(pos.z)) && pos.z < 0f) {
				posx = 0f; posz = 1f; lookAt = "BACK";
				// Debug.Log ("Animacion caminando hacia BACK");
			}
			// Animacion adelante
			if ((Mathf.Abs(pos.x) < Mathf.Abs(pos.z)) && pos.z > 0f) {
				posx = 0f; posz = -1f; lookAt = "FRONT";
				// Debug.Log ("Animacion caminando hacia FRONT");
			}
			// Animacion derecha ()
			if ((Mathf.Abs(pos.x) > Mathf.Abs(pos.z)) && pos.x < 0f) {
				posx = 1f; posz = 0f; lookAt = "RIGHT";
				// Debug.Log ("Animacion caminando hacia DERECHA");
			}
			// Animacion izquierda ()
			if ((Mathf.Abs(pos.x) > Mathf.Abs(pos.z)) && pos.x > 0f) {
				posx = -1f; posz = 0f; lookAt = "LEFT";
				// Debug.Log ("Animacion caminando hacia IZQUIERDA");
			}


        //}
    }
}
