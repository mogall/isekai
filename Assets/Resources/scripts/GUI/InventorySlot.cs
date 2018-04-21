using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {
	public ItemData itemData;
	public Image itemIcon;
	public int stackCount; //TODO - use thiss later to display stack counts on stackable items
	public Button deleteButton;
	public Button dropButton;

	public void AddItemInSlot(ItemData data){
		itemData = data;
		itemIcon.sprite = itemData.uiSprite;
		deleteButton.interactable = true;
		dropButton.interactable = true;
	}
	public void ClearSlotData(){
		itemData = null;
		itemIcon.sprite = null;
		deleteButton.interactable = false;
		dropButton.interactable = false;
	}


	public void DeleteItem(){
		PlayerInventory.instance.Remove (itemData);
	}
	public void DropItem(){
		PlayerInventory.instance.Drop (itemData);
	}
}
