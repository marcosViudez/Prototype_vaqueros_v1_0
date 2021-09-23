using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


[System.Serializable]
public class item {
	// lo que van a llevar los botones
	public string name;								// nombre de imagen
	public Button.ButtonClickedEvent crearItem;		// evento de click
}

public class CreateScrollList : MonoBehaviour {

	public List<item> itemList;

	public GameObject itemTienda;
	public Transform contentPanel;


	void Start()
	{
		populateList ();		// crea la lista de botones de la tienda
	}


	void populateList()
	{
		foreach (var item in itemList) 
		{
			GameObject newSlot = Instantiate (itemTienda) as GameObject;
			ItemTienda itemTiendaScript = newSlot.GetComponent<ItemTienda> ();
			itemTiendaScript.itemTienda.onClick = item.crearItem;
			itemTiendaScript.transform.SetParent (contentPanel);

		}
	}

	public void doSomething()
	{
		Debug.Log("pulsado");
	}
}
