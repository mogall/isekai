using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//TODO - make it so that when there's an unstackable item, system won't show the number and also will ignore the stack size when performing operations
//DONOW - make it so it's not a list(why?) and you can put stuff in the container
public class ContainerSlot : MonoBehaviour {
	public ItemData itemData;
	public Image itemIcon;
	public Text stackCount;

	public void AddItemInSlot(ItemData data, int stackSize){
		itemData = data;
		itemIcon.sprite = itemData.uiSprite;
		stackCount.text = itemData.stackSize.ToString();
	}
	public void CleanContainterSlotData(){
		itemData = null;
		itemIcon.sprite = null;
		stackCount.text = ("0");

	}
	public void RemoveItemFromContainer(){
		PlayerController.instance.container.items.Remove (itemData); //TODO - can't i have a local reference to the container, setup in the prefab or something? container in playerctrl seems a little silly
		CleanContainterSlotData();
		/*itemData = null;
		itemIcon.sprite = null;
		stackCount.text = ("0");*/ //why did i have this separately instead of just using the clear method?
	}
	public void GetItemFromThisSlot(){
		if (itemData != null) {
			PlayerInventory.instance.Add (itemData);
			RemoveItemFromContainer ();
		}
	}
}
