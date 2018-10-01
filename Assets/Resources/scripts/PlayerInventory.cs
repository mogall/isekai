using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerInventory : MonoBehaviour {

	public delegate void OnInventoryChanged ();
	public OnInventoryChanged onInventoryChangedCallback;

	//public List<ItemData> inventoryItems = new List<ItemData>(); //TODO - change this along with the gui method so the player can move the inventory items around

	public struct InventorySlotStruct //DONOW REMOVE - if i have inventoryslot class, why have this? remove and just use itemdata as inventory array element
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
	public bool Add (ItemData item){ //adds item to the first free slot in inventory //UPGRADE
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
	public void MoveItemFromToSlot(InventorySlot fromslot, InventorySlot toslot){ //DONOW - implement checking if the item can be equipped //DONOW - swap updating inventory data slots to here?
		if (fromslot.itemData == null) {
			return; //secondary null-drag security
		}
		ItemData data = fromslot.itemData;
		int stackSize = fromslot.itemData.stackSize;
		if (toslot.isEquipmentSlot) {
			if (toslot.itemData != null) {
				print ("MoveItemFromToSlot tried to equip from " + fromslot + " to a nonempty slot " + toslot + ". Aborting");
				return;
			}
			if (toslot.equipmentSlotType == data.equipmentSlotType) {
				toslot.itemData = data;
				//ADD DONOW - implement adding equipment here, check if something is already here, return if wrong equipment type (if(data is weapondata) or something)
			} else {
				return;
			}
		} else {
			if (inventory[toslot.slotID].stackSize > 0) {
				print ("MoveItemFromToSlot tried to move from slot no." + fromslot.slotID + " to a nonempty slot no." + toslot.slotID + ". Aborting"); //TODO - call switch here as backup?
				return;
			}
			inventory [toslot.slotID].item = data;
			inventory [toslot.slotID].stackSize = stackSize;
		}

		if (fromslot.isEquipmentSlot) {
			//ADD DONOW - implement removing equipment here
			fromslot.itemData = null;
		} else {
			inventory [fromslot.slotID] = new InventorySlotStruct ();
		}
		UpdateInventoryUI ();
	}
	public void SwitchItemsBetweenSlots(InventorySlot slot1, InventorySlot slot2){
		if (!CanPutInSlot(slot1, slot2.itemData) || !CanPutInSlot(slot2, slot1.itemData)) {
			return;
		}
		if (slot1.equipmentSlotType == EquipmentSlotType.INVENTORY & slot2.equipmentSlotType == EquipmentSlotType.INVENTORY) {
			ItemData data1 = slot1.itemData;
			int stack1 = slot1.itemData.stackSize;
			ItemData data2 = slot2.itemData;
			int stack2 = slot2.itemData.stackSize;
			inventory [slot1.slotID].item = inventory [slot2.slotID].item;
			inventory [slot1.slotID].stackSize = inventory [slot2.slotID].stackSize;
			inventory [slot2.slotID].item = data1;
			inventory [slot2.slotID].stackSize = stack1;
		} else {
			//DONOW - implement adding equipment here
			return;
		}

		UpdateInventoryUI ();
	}
	bool CanPutInSlot(InventorySlot slot, ItemData data){ //this checks if the item is equipment, slot is equipment and if they match
		if (slot.isEquipmentSlot) {
			if (slot.equipmentSlotType != data.equipmentSlotType) {
				return false;
			}
		}
		return true;
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

	public void EndSlotDrag(InventorySlot originSlot){ 
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
			InventorySlot targetSlot = result.gameObject.GetComponent<InventorySlot> ();
			if (targetSlot != null) { //if you drop on a slot
				if (targetSlot.itemData == null) { //if the slot is empty
					PlayerInventory.instance.MoveItemFromToSlot (originSlot, targetSlot);
				} else {
					PlayerInventory.instance.SwitchItemsBetweenSlots (originSlot, targetSlot);
				}
			}
			/*if (slot.GetType() == typeof(InventorySlot) && result.GetType() == typeof(InventorySlot)) {
				InventorySlot a = slot as InventorySlot;
				InventorySlot b = result as InventorySlot;
				if (b.itemData == null) {
					PlayerInventory.instance.MoveItemFromToSlot (a.slotID, b.slotID);
				} else {
					PlayerInventory.instance.SwitchItemsBetweenSlots (a.slotID, b.slotID);
				}

			}*/
			/*if (result.gameObject.name.Contains("Inventory Slot")) {
				InventorySlot resultSlot = result.gameObject.GetComponent<InventorySlot> ();
				if (resultSlot.itemData == null) {
					PlayerInventory.instance.MoveItemFromToSlot (slot.slotID, resultSlot.slotID);
				} else {
					PlayerInventory.instance.SwitchItemsBetweenSlots (slot.slotID, resultSlot.slotID);
				}
			}*/
		}
	}

}
