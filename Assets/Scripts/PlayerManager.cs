using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    private Inventory inventory;
    private RaycastHit hit;
    private GameObject pickedUp;
    public LayerMask layerMask;

    // Use this for initialization
    void Start() {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update() {
//        pickUpItem();
    }

//    public GameObject pickUpItem() {
//
//        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
//            if (Input.GetMouseButtonDown(0)) {
//                PlayerController.IsItemPicked = true; 	// Devuelve variable Item Clickado a true
//                pickedUp = hit.collider.gameObject; 	// Guarda en variable GameObject cuando hacemos Click en Item
//                Debug.Log("[INFO] Recoge item " + pickedUp);
//            }
//        }
//
//        return pickedUp;
//    }

    void OnTriggerEnter(Collider collider)
    {
//       float distToPicked = 0f;
        // Cálculo de distancia dentro de la cual el jugador recoge Item con tecla Espacio pulsado
//        if (pickUpItem() != null)
//            distToPicked = Vector3.Distance(pickUpItem().transform.position, collider.transform.position);
        // Recoje Item y lo añade a Inventario
        if ((collider.tag.EndsWith("_collect") /*&& PlayerController.IsItemPicked*/ /*&& distToPicked < Utils.DIST_TO_ITEM*/))
        {
  //          inventory.addItem(pickUpItem().GetComponent<Item>());       // Añade Item a Inventario
            inventory.addItem(collider.GetComponent<Item>());   
  //          pickUpItem().gameObject.SetActive(false);                   // desactiva GameObject
 //           pickUpItem().transform.parent.gameObject.SetActive(false);  // Desactiva el elemento Padre del GameObject
 //           Destroy(collider.gameObject);
            //Destroy(collider.transform.parent.gameObject);
 //           audioCollect.Play(); // Activa audio de recolecta
 //           PlayerController.IsItemPicked = false;
        }

    }

}