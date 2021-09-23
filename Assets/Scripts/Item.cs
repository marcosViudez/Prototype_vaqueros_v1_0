using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public string itemName;
    public string itemDescription;
    public Sprite spriteNeutral;
    public Sprite spriteHightlighted;
    public int maxSize;
    public bool isDisplayed;

    public GUISkin objectLayer;
    private Color initialColor;

    public enum ItemType
    {
        Planta,
        Other
    };

    public enum ItemAction
    {
        Recoger,
        Examinar
    };
		

    public ItemType type = ItemType.Planta;
	public ItemAction action = ItemAction.Examinar;
    //public Inventario inventory;
    private RaycastHit hit;

    void OnGUI()
    {
        GUI.skin = objectLayer;
        objectNameOnScreen();
    }


    void OnMouseEnter()
    {
        if (GetComponent<Renderer>().material.HasProperty("_Color"))
            initialColor = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = Color.cyan;
        isDisplayed = true;
    }

    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = initialColor;
        isDisplayed = false;
    }

    private void objectNameOnScreen()
    {
        if (isDisplayed)
        {
            GUI.Box(new Rect(Event.current.mousePosition.x + 42, Event.current.mousePosition.y, 160, 42), itemName, "Selected Object");
            GUI.Box(new Rect(Event.current.mousePosition.x + 62, Event.current.mousePosition.y + 40, 160, 42), action.ToString(), "Selected Object");

        }
    }

	public ItemAction use()
    {
        switch (type)
        {
            case ItemType.Planta:
				action = ItemAction.Examinar;
				Debug.Log("[INFO] item: {name: '" + gameObject.name + "', state: '" + action + "'}");
                break;

            case ItemType.Other:
                break;

        } return action;
    } 
}
