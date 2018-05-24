using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipmentSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public EquipmentSlotType equipmentSlotType;
	public ItemData itemData;
	public Image itemIcon;
	//public Text stackCounter //do i need that here? will i have stackable items on inventory?
	// Use this for initialization

	public void AddItemInSlot(ItemData data){
		itemData = data;
		itemIcon.sprite = itemData.uiSprite;
	}
	public void ClearSlotData(){
		itemData = null;
		itemIcon.sprite = null;
	}

	public void OnBeginDrag(PointerEventData data){
		//print ("started drag");
	}
	public void OnDrag(PointerEventData data){ //DONOW - change slots to a base "slot" class, inherit from them and then call slot in inventory when dragging?
		PlayerInventory.instance.SlotDrag (this); //DONOW - print(interactable.GetType().ToString()); - use this to determine the type of slot and then fire stuff accordingly
	}
	public void OnEndDrag (PointerEventData data){
		PlayerInventory.instance.EndSlotDrag (this);
	}

}
