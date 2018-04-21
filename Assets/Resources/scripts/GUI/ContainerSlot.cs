using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerSlot : MonoBehaviour {
	public ItemData itemData;
	public Image itemIcon;
	public int stackCount; //TODO - use thiss later to display stack counts on stackable items

	public void AddItemInSlot(ItemData data){
		itemData = data;
		itemIcon.sprite = itemData.uiSprite;
	}
	public void CleanContainterSlotData(){
		itemData = null;
		itemIcon.sprite = null;
	}
	public void RemoveItemFromContainer(){
		PlayerController.instance.container.items.Remove (itemData); //TODO - can't i have a local reference to the container, setup in the prefab or something? container in playerctrl seems a little silly
		itemData = null;
		itemIcon.sprite = null;
	}
	public void GetItemFromThisSlot(){
		if (itemData != null) {
			PlayerInventory.instance.Add (itemData);
			RemoveItemFromContainer ();
		}
	}
}
