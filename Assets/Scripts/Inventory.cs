using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Inventory : MonoBehaviour {

    private RectTransform inventoryRect;
    private float inventoryWidth, inventoryHeight;

    public int slots;
    public int rows;
    public int slotPaddingLeft, slotPaddingTop;
    public float slotSize;
    public GameObject slotPrefab;

	private EventSystem eventSistem;
	private static GameObject clicked;
	public static Slot to, from;
	private static GameObject hoverObject;
	private float hoverYOffSet;

	private Canvas canvas;
	public GameObject iconPrefab;

    public List<GameObject> allSlots;

    private static int emptySlot;
    public static int EmptySlot {
        get { return emptySlot; }
        set { emptySlot = value; }
    }

    private void Start() {
        createLayout();
		eventSistem = GameObject.Find(Utils.OBJECT_EVENT_SYSTEM).GetComponent<EventSystem>();
		canvas = GameObject.Find(Utils.OBJECT_CANVAS).GetComponent<Canvas>() as Canvas;
		// iconPrefab = GameObject.Find(Utils.OBJECT_ICON_PREFAB).GetComponent<GameObject>() as GameObject;


    }

	void Update () {

		if (Input.GetMouseButtonUp(Utils.ZERO)) {
			// Mueve Item fuera de Slot de Inventario
			if (!eventSistem.IsPointerOverGameObject(-1) && from != null)  {
				from.GetComponent<Image>().color = Color.white; // Reset color de Slot
				from.clearSlot(); // Borra el Item del Slot
				Destroy(GameObject.Find(Utils.OBJECT_HOVER)); // Elimina el objeto Hover

				to = null;
				from = null;
				emptySlot++;
			}
		}

		if (hoverObject != null) {
			Vector2 position;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
			position.Set(position.x, position.y - hoverYOffSet);
			hoverObject.transform.position = canvas.transform.TransformPoint(position);
		}

	}

    private void createLayout() {
        allSlots = new List<GameObject>(0);
		hoverYOffSet = slotSize * 0.01f;
        emptySlot = slots;

        inventoryWidth = (slots / rows) * (slotSize +slotPaddingLeft) + slotPaddingLeft;
        inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;

        inventoryRect = GetComponent<RectTransform>();
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);

        int columns = slots / rows;

        for (int y =0; y < rows; y++) {
            for (int x =0; x < columns; x++) {
                GameObject newSlot = Instantiate(slotPrefab) as GameObject;
                RectTransform slotRect = newSlot.GetComponent<RectTransform>();

                slotRect.name = "Slot";
                newSlot.transform.SetParent(this.transform.parent);

                slotRect.localPosition = inventoryRect.localPosition + 
                    new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));

                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

                allSlots.Add(newSlot);
            }

        }

    }

    public bool addItem(Item item) {
        if (item.maxSize == 1) {
            placeEmpty(item);
            return true;
        } else {
            foreach (GameObject slot in allSlots) {
                Slot tmp = slot.GetComponent<Slot>();
                if (!tmp.isEmpty) {
                    if (tmp.currentItem.tag == item.tag && tmp.isAvailable) {
                        tmp.addItem(item);
                        return true;
                    }
                }
            }
            if (emptySlot > 0) {
                placeEmpty(item);
            }
        } return false;
    }

    private bool placeEmpty(Item item) {
        if (emptySlot > 0) {
            foreach (GameObject slot in allSlots) {
                Slot tmp = slot.GetComponent<Slot>();
                if (tmp.isEmpty) {
                    tmp.addItem(item);
                    emptySlot--;
                    return true;
                }
            }
        } return false;
    }

	/// <summary>
	/// Se usa en evento de boton onClick() sobre prefab Slot:
	///  	desde el editor, se asignan: Inventory, prefab Slot y este metodo
	/// En el evento de boton, poner Navigation a none
	/// </summary>
	/// <param name="clicked">Clicked.</param>
	public void moveItem(GameObject clicked) {

		Inventory.clicked = clicked;

		if (from == null) {

			if (!clicked.GetComponent<Slot>().isEmpty) {

				from = clicked.GetComponent<Slot>();
				from.GetComponent<Image>().color = Color.gray;

				hoverObject = Instantiate(iconPrefab) as GameObject;
				hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
				hoverObject.name = Utils.OBJECT_HOVER;

				RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
				RectTransform clickedTransform = clicked.GetComponent<RectTransform>();

				hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
				hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);

				hoverObject.transform.SetParent(GameObject.Find(Utils.OBJECT_CANVAS).transform, true);
				hoverObject.transform.localScale = clicked.gameObject.transform.localScale;



			}
		}
		else {

			if (to == null) { 
				to = clicked.GetComponent<Slot>();
				Destroy(GameObject.Find(Utils.OBJECT_HOVER));

			}

			if(to != null && from != null) {
				Stack<Item> tmpTo = new Stack<Item>(to.Items);
				to.addItems(from.Items);

				if(tmpTo.Count == 0) {
					from.clearSlot();
				} else {
					from.addItems(tmpTo);
				}
			}

			from.GetComponent<Image>().color = Color.white;
			to = null;
			from = null;
			Destroy(GameObject.Find(Utils.OBJECT_HOVER));
		}   

	}

}
