using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerClickHandler
{
	public Transform canvas;

	public Transform old;
	public CharacterInventory characterInvectory;
	public Item item;
	public string typeList;


	void Start ()
	{
		characterInvectory = GameObject.FindGameObjectWithTag ("Player").transform.GetComponent<CharacterInventory> ();
		if (typeList == "")
			typeList = "Ground";
		canvas = GameObject.Find ("Canvas").transform;
	}

	public void OnBeginDrag (PointerEventData eventData)
	{
		old = transform.parent;
		transform.SetParent (canvas);
		GetComponent<CanvasGroup> ().blocksRaycasts = false;
	}

	public void OnDrag (PointerEventData eventData)
	{
		transform.position = Input.mousePosition;
	}

	public void OnEndDrag (PointerEventData eventData)
	{
		GetComponent<CanvasGroup> ().blocksRaycasts = true;
		if (transform.parent == canvas) {
			transform.SetParent (old);
		}
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left) {
			if (typeList == "Inventory") {
				characterInvectory.UseItem (this);
			} else if (typeList == "Ground" && item.typeItem == "Other") {
				characterInvectory.TakeItem (this);
			} else if (typeList == "Ground" && item.typeItem == "First Weapon") {
				characterInvectory.TakeWeapon (this);
			}

			//characterInvectory.RemoveItem (this);
		}
	}
}
