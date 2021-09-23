using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler {

    // Coleccion de items (pila LIFO)
    private Stack<Item> items;
    public Stack<Item> Items {
        get { return items; }
        set { items = value; }
    }

    // inicializa el contador de items a cero
    public bool isEmpty{
        get { return items.Count == 0; }
    }

    // retorna true si hay items
    public bool isAvailable {
         get { return currentItem.maxSize > Items.Count;  }
    }

    // retorna el item de la primera posicion de la pila, sin eliminarlo
    public Item currentItem {
        get { return items.Peek(); } 
    }

    public Text stackText;
    public Sprite slotEmpty;
    public Sprite slotHighlight;

    private void Start() {
        items = new Stack<Item>();
        RectTransform slotRect = GetComponent<RectTransform>();
        RectTransform txtRect = stackText.GetComponent<RectTransform>();

        int txtScaleFactor = (int)(slotRect.sizeDelta.x * .6f);
        stackText.resizeTextMaxSize = txtScaleFactor;
        stackText.resizeTextMinSize = txtScaleFactor;

        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
    }

    public void addItem(Item item) {
        items.Push(item); // inserta un item en la primera posicion de la pila
        if (items.Count > 1) {
            stackText.text = items.Count.ToString();      
        }
        changeSprite(item.spriteNeutral, item.spriteHightlighted);
    }

	// 
	/// <summary>
	/// Se Usa en Clase Inventory para mover el item por el iventario
	/// </summary>
	/// <param name="items">Items.</param>
	public void addItems(Stack<Item> items) {
		this.items = new Stack<Item>(items);
		stackText.text = items.Count > 0 ? items.Count.ToString() : string.Empty;
		changeSprite(currentItem.spriteNeutral, currentItem.spriteHightlighted);
	}

    private void changeSprite(Sprite neutral, Sprite highlight) {
        GetComponent<Image>().sprite = neutral;

        SpriteState st = new SpriteState();
        st.highlightedSprite = highlight;
        st.pressedSprite = neutral;

        GetComponent<Button>().spriteState = st;
    }

	public void clearSlot() { 
		items.Clear();
		changeSprite(slotEmpty, slotHighlight);
		stackText.text = string.Empty;
	}

	private void useItem() {
		if(!isEmpty) {
			items.Pop ().use(); // elimina y retorna el primer objeto de la lista
			stackText.text = items.Count > 0 ? items.Count.ToString() : string.Empty;
			if (isEmpty) {
				changeSprite(slotEmpty, slotHighlight);
				Inventory.EmptySlot++;
			}
		}
	}

	public void OnPointerClick(PointerEventData eventData) {
		if (eventData.button == PointerEventData.InputButton.Right) {
			useItem();
		}
	}

}
