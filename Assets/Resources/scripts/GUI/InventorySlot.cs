using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//TODO - make it so that when there's an unstackable item, system won't show the number and also will ignore the stack size when performing operations
public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public int slotID;
	public ItemData itemData;
	public Image itemIcon;
	public Text stackCounter;
	//public int stackCount; 
	//public Button deleteButton;
	//public Button dropButton;

	public void AddItemInSlot(ItemData data, int stackSize){
		itemData = data;
		itemIcon.sprite = itemData.uiSprite;
		stackCounter.text = stackSize.ToString();
		//deleteButton.interactable = true;
		//dropButton.interactable = true;
	}
	public void ClearSlotData(){
		itemData = null;
		itemIcon.sprite = null;
		stackCounter.text = ("0");
		//deleteButton.interactable = false;
		//dropButton.interactable = false;
	}
	public void OnBeginDrag(PointerEventData data){
		//print ("started drag");
	}
	public void OnDrag(PointerEventData data){
		PlayerInventory.instance.SlotDrag (this);
	}
	public void OnEndDrag (PointerEventData data){
		PlayerInventory.instance.EndSlotDrag (this);
	}

	/*public void DragItem(){
		if (itemData != null) {
			if (PlayerGUI.instance.itemDragImage.activeSelf == false) {
				PlayerGUI.instance.itemDragImage.SetActive (true);
				PlayerGUI.instance.itemDragImage.GetComponent<Image> ().sprite = itemIcon.sprite;
				PlayerGUI.instance.itemDragImage.transform.position = Input.mousePosition;
			} else {
				PlayerGUI.instance.itemDragImage.transform.position = Input.mousePosition;
			}
		}
	}
	public void EndDrag(){ //FIX - if you turn off inventory(probably else too), thiss fucks up and leaves the drag image on after turning inventory back on, need a fix
		PlayerGUI.instance.itemDragImage.SetActive (false);
		//TODO - this is the raycast setup that finds the inventory slot gui element - this is extremely crude, so upgrade it later or just move it somewhere
		PointerEventData pointerData = new PointerEventData (EventSystem.current) { 
			pointerId = -1,
		};
		pointerData.position = Input.mousePosition;
		List<RaycastResult> results = new List<RaycastResult> ();
		EventSystem.current.RaycastAll (pointerData, results);
		//print (results.Count);
		if (results.Count == 0) { //TODO - is this really going to return 0 if it doesn't end on any gui element?
			//TODO - DROP ITEM HERE
			print("item dropping is not implemented yet");
		}
		foreach (RaycastResult result in results) {
			if (result.gameObject.name.Contains("Inventory Slot")) {
				InventorySlot resultSlot = result.gameObject.GetComponent<InventorySlot> ();
				if (resultSlot.itemData == null) {
					PlayerInventory.instance.MoveItemFromToSlot (slotID, resultSlot.slotID);
				} else {
					PlayerInventory.instance.SwitchItemsBetweenSlots (slotID, resultSlot.slotID);
				}
			}
		}
	}*/

	/*public void DeleteItem(){ //REMOVE - what are these even for? oh i know, for the buttons, but buttons will prolly disappear, so leave it out for now
		PlayerInventory.instance.Remove (itemData);
	}
	public void DropItem(){
		PlayerInventory.instance.Drop (itemData);
	}*/
}
