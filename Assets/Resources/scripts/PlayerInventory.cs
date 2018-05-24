using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerInventory : MonoBehaviour {

	public delegate void OnInventoryChanged ();
	public OnInventoryChanged onInventoryChangedCallback;

	//public List<ItemData> inventoryItems = new List<ItemData>(); //TODO - change this along with the gui method so the player can move the inventory items around

	public struct InventorySlotStruct //TODO - if i have inventoryslot class, why have this? and doesn't it get fucked up when i have two things with the same name?
	{
		public ItemData item;
		public int stackSize;
	}
	public int inventorySize = 10;
	public InventorySlotStruct[] inventory;
	//ADD TODO NOTES FOR FUTURE UPGRADES?
	//private Dictionary<ItemData, int> inventorySlot;
	//public InventorySlot[] inventory; inventory is a "list" of inventoryslots. it's an array so you can drag and drop(with list it gets shortened and whatnot.
	//add dropable property to itemdata?
	//from the thread:
	//>Dictionary<Position, ItemSlot> itemSlots
	//>Dictionary<Item, List<ItemSlot>> itemsToItemSlots - that's what he had
	//gethashcode override

	public static PlayerInventory instance;
	void Awake() {
		if (instance != null && instance != this) {
			Destroy (gameObject);
		} else {
			instance = this;
		}
	}
	void Start(){
		inventory = new InventorySlotStruct[inventorySize]; //TODO - make a method for changing inventory size when you pick up an new backpack or something
		PlayerGUI.instance.UpdateGUIInventorySlotNumber(inventorySize); 
	}
	public bool Add (ItemData item){
		int firstempty = -1;
		if (item.stackable) {
			for (int i = 0; i < inventory.Length; i++) {
				if (inventory [i].stackSize > 0 && inventory [i].item.ID == item.ID) {
					inventory [i].stackSize += item.stackSize;
					UpdateInventoryUI ();
					return true;
				} else if (inventory [i].stackSize == 0 && firstempty == -1) {
					firstempty = i;
				}
			}
			if (firstempty > -1) {
				inventory [firstempty].item = item;
				inventory [firstempty].stackSize = item.stackSize;
				UpdateInventoryUI ();
				return true;
			}
		} else {
			for (int i = 0; i < inventory.Length; i++) {
				if (inventory[i].stackSize == 0) {
					inventory [i].item = item;
					inventory [i].stackSize = 1;
					UpdateInventoryUI ();
					return true;
				}
			}
		}
		return false;
	}
	public void RemoveOnSlot(int i, int amount){
		if (amount > 0) {
			inventory [i].stackSize -= amount;
			if (inventory [i].stackSize <= 0) {
				inventory [i] = new InventorySlotStruct ();
			}
		} else {
			inventory [i] = new InventorySlotStruct ();
		}
		UpdateInventoryUI ();
	}
	public bool RemoveOnID(int id, int amount){
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory[i].item != null && inventory[i].item.ID == id) {
				if (amount > 0) {
					inventory [i].stackSize -= amount;
					if (inventory [i].stackSize <= 0) {
						inventory [i] = new InventorySlotStruct ();
					}
				} else {
					inventory [i] = new InventorySlotStruct ();
				}
				UpdateInventoryUI ();
				return true;
			}
		}
		return false;
	}
	public void MoveItemFromToSlot(int fromslot, int toslot){
		ItemData data = inventory [fromslot].item;
		int stackSize = inventory [fromslot].stackSize;
		if (inventory[toslot].stackSize > 0) {
			print ("MoveItemFromToSlot tried to move to a nonempty slot no." + toslot + ". Aborting");
			return;
		}
		inventory [toslot].item = data;
		inventory [toslot].stackSize = stackSize;
		inventory [fromslot] = new InventorySlotStruct ();
		UpdateInventoryUI ();
	}
	public void SwitchItemsBetweenSlots(int slot1, int slot2){
		ItemData data1 = inventory [slot1].item;
		int stack1 = inventory [slot1].stackSize;
		inventory [slot1].item = inventory [slot2].item;
		inventory [slot1].stackSize = inventory [slot2].stackSize;
		inventory [slot2].item = data1;
		inventory [slot2].stackSize = stack1;
		UpdateInventoryUI ();
	}
	public void Drop(ItemData item, int amount){  //TODO - this only takes itemdata, is this safe? will i make new item data sets that will fuck this up? monitor when spawning new items
		bool removed = RemoveOnID (item.ID, amount);
		if (removed) {
			GameObject droppedItem = Instantiate (item.prefab, transform.position, Quaternion.identity); //TODO - figure out a way to drop items without this crude method, and with everything nnicely attached
			PickableItem data = droppedItem.GetComponent<PickableItem> ();
			data.itemData = item;
			data.itemData.stackSize = amount; //TODO - check if this is working correctly
		}
	}
	public bool HasItem(ItemData data){ //UPGRADE - if i invoke this on a recipe, i have to iterate through WHOLE inventory x number of recipe items. it's ridiculous, upgrade this.
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory[i].item != null && inventory[i].item.ID == data.ID) {
				return true;
			}
		}
		return false;
	}
	public bool HasItem(ItemData data, int amount){
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory[i].item != null && inventory[i].item.ID == data.ID && inventory[i].stackSize >= amount) {
				return true;
			}
		}
		return false;
	}
	public bool HasItems(List<ItemData> data){ //FIX - this doesn't check for amounts
		foreach (ItemData dataItem in data) {
			if (!HasItem(dataItem)) {
				return false;
			}
		}
		return true;
	}
	void UpdateInventoryUI(){
		if (onInventoryChangedCallback != null) {
			onInventoryChangedCallback.Invoke ();
		}
	}

	// ITEM SLOTS DRAGGING METHODS //

	public void BeginSlotDrag(){
		//do i need this?
	}

	public void SlotDrag(InventorySlot slot){
		print ("dupa");
		if (slot.itemData != null) {
			if (PlayerGUI.instance.itemDragImage.activeSelf == false) {
				PlayerGUI.instance.itemDragImage.SetActive (true);
				PlayerGUI.instance.itemDragImage.GetComponent<Image> ().sprite = slot.itemIcon.sprite;
				PlayerGUI.instance.itemDragImage.transform.position = Input.mousePosition;
			} else {
				PlayerGUI.instance.itemDragImage.transform.position = Input.mousePosition;
			}
		}
	}

	public void EndSlotDrag(InventorySlot slot){ 
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
					PlayerInventory.instance.MoveItemFromToSlot (slot.slotID, resultSlot.slotID);
				} else {
					PlayerInventory.instance.SwitchItemsBetweenSlots (slot.slotID, resultSlot.slotID);
				}
			}
		}
	}

}
