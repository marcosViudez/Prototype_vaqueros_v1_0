using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour {

	public Image sight;
    private GameObject target;
    private PlayerController player;
    private Ray ray;
    private RaycastHit hit;
    public LayerMask layerMask;
	private Vector3 targetPosition;
    void Start () {
        target = GameObject.Find("Target");
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    
	void Update () {
//        if (Input.GetMouseButtonDown(0)) {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		// sight.transform.position = Camera.main.WorldToScreenPoint (hit.point);		// posicion de la mirilla en la pantalla

            if(Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask)) {
                if (hit.collider.tag.Equals("Terrain")) {
					targetPosition = hit.point;							// posicion de target al colisionar ray desde camara con el Terrain
                    //targetPosition.y = transform.position.y;					// Bloquea eje Y
                    target.transform.position = targetPosition;				    // Cambia posicion del Target al punto de colisión con Terrain
					player.IsRotate = true;

                }
            }
//      }

        // PARA ANIMACIONES
		if (player.IsRotate) {
			player.changeDirection(targetPosition);
        }  
    }
}
